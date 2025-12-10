using Artiligence.InvoiceSystem.Web.Data;
using Artiligence.InvoiceSystem.Web.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Artiligence.InvoiceSystem.Web.Services
{
    public class InvoiceService
    {
        private readonly AppDbContext _context;
        private readonly InvoiceNumberService _invoiceNumberService;

        public InvoiceService(AppDbContext context, InvoiceNumberService invoiceNumberService)
        {
            _context = context;
            _invoiceNumberService = invoiceNumberService;
        }

        public async Task<Invoice> CreateInvoiceAsync(Invoice invoice)
        {
            invoice.InvoiceNumber = await _invoiceNumberService.GetNextInvoiceNumberAsync();
            invoice.PaymentStatus = PaymentStatus.Unpaid;
            invoice.DeliveryStatus = DeliveryStatus.NotDelivered;

            CalculateTotals(invoice);

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        public async Task<Invoice?> GetInvoiceWithDetailsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Lines)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task UpdateInvoiceAsync(Invoice invoiceWithLinesFromView)
        {
            var existingInvoice = await _context.Invoices
                .Include(i => i.Lines)
                .Include(i => i.Payments)
                .FirstOrDefaultAsync(i => i.Id == invoiceWithLinesFromView.Id);

            if (existingInvoice == null)
            {
                throw new InvalidOperationException("Invoice not found");
            }

            existingInvoice.CustomerId = invoiceWithLinesFromView.CustomerId;
            existingInvoice.InvoiceDate = invoiceWithLinesFromView.InvoiceDate;
            existingInvoice.ShowVat = invoiceWithLinesFromView.ShowVat;
            existingInvoice.VatRate = invoiceWithLinesFromView.VatRate;
            existingInvoice.Notes = invoiceWithLinesFromView.Notes;
            existingInvoice.Currency = invoiceWithLinesFromView.Currency;
            existingInvoice.DeliveryStatus = invoiceWithLinesFromView.DeliveryStatus;

            // Update lines
            var incomingLineIds = invoiceWithLinesFromView.Lines.Where(l => l.Id != 0).Select(l => l.Id).ToHashSet();
            var linesToRemove = existingInvoice.Lines.Where(l => !incomingLineIds.Contains(l.Id)).ToList();
            _context.InvoiceLines.RemoveRange(linesToRemove);

            foreach (var incomingLine in invoiceWithLinesFromView.Lines)
            {
                var existingLine = existingInvoice.Lines.FirstOrDefault(l => l.Id == incomingLine.Id);
                if (existingLine == null)
                {
                    existingInvoice.Lines.Add(new InvoiceLine
                    {
                        ProductName = incomingLine.ProductName,
                        Description = incomingLine.Description,
                        Colour = incomingLine.Colour,
                        Detailing = incomingLine.Detailing,
                        Size = incomingLine.Size,
                        Orientation = incomingLine.Orientation,
                        Notes = incomingLine.Notes,
                        UnitPrice = incomingLine.UnitPrice,
                        Quantity = incomingLine.Quantity,
                        DiscountType = incomingLine.DiscountType,
                        DiscountValue = incomingLine.DiscountValue
                    });
                }
                else
                {
                    existingLine.ProductName = incomingLine.ProductName;
                    existingLine.Description = incomingLine.Description;
                    existingLine.Colour = incomingLine.Colour;
                    existingLine.Detailing = incomingLine.Detailing;
                    existingLine.Size = incomingLine.Size;
                    existingLine.Orientation = incomingLine.Orientation;
                    existingLine.Notes = incomingLine.Notes;
                    existingLine.UnitPrice = incomingLine.UnitPrice;
                    existingLine.Quantity = incomingLine.Quantity;
                    existingLine.DiscountType = incomingLine.DiscountType;
                    existingLine.DiscountValue = incomingLine.DiscountValue;
                }
            }

            CalculateTotals(existingInvoice);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDeliveryStatusAsync(int invoiceId, DeliveryStatus status)
        {
            var invoice = await _context.Invoices.FindAsync(invoiceId);
            if (invoice == null)
            {
                throw new InvalidOperationException("Invoice not found");
            }

            invoice.DeliveryStatus = status;
            await _context.SaveChangesAsync();
        }

        public void CalculateTotals(Invoice invoice)
        {
            foreach (var line in invoice.Lines)
            {
                var lineTotal = line.UnitPrice * line.Quantity;
                decimal discountAmount = line.DiscountType switch
                {
                    DiscountType.Amount => line.DiscountValue,
                    DiscountType.Percent => lineTotal * (line.DiscountValue / 100),
                    _ => 0
                };

                line.LineTotalAfterDiscount = lineTotal - discountAmount;
            }

            invoice.TotalBeforeDiscount = invoice.Lines.Sum(l => l.UnitPrice * l.Quantity);
            invoice.TotalAfterDiscount = invoice.Lines.Sum(l => l.LineTotalAfterDiscount);
            invoice.TotalDiscount = invoice.TotalBeforeDiscount - invoice.TotalAfterDiscount;

            invoice.TotalVatAmount = invoice.ShowVat && invoice.VatRate > 0
                ? invoice.TotalAfterDiscount * invoice.VatRate
                : 0;

            invoice.GrandTotal = invoice.TotalAfterDiscount + invoice.TotalVatAmount;
        }
    }
}
