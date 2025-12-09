using ATM2026.Data;
using ATM2026.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM2026.Services
{
    public class PaymentService
    {
        private readonly AppDbContext _context;

        public PaymentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentAsync(PaymentTransaction payment)
        {
            payment.PaymentDate = payment.PaymentDate == default ? DateTime.UtcNow : payment.PaymentDate;
            _context.PaymentTransactions.Add(payment);
            await _context.SaveChangesAsync();
            await RecalculateInvoicePaymentStatusAsync(payment.InvoiceId);
        }

        private async Task RecalculateInvoicePaymentStatusAsync(int invoiceId)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
            {
                throw new InvalidOperationException("Invoice not found");
            }

            var totalPaid = invoice.Payments.Sum(p => p.Amount);
            var totalDue = invoice.GrandTotal;

            invoice.PaymentStatus = totalPaid <= 0
                ? PaymentStatus.Unpaid
                : totalPaid < totalDue
                    ? PaymentStatus.Partial
                    : PaymentStatus.Full;

            await _context.SaveChangesAsync();
        }
    }
}
