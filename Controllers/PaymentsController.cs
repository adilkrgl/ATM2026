using Artiligence.InvoiceSystem.Web.Models.Entities;
using Artiligence.InvoiceSystem.Web.Models.ViewModels;
using Artiligence.InvoiceSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Artiligence.InvoiceSystem.Web.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PaymentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Details", "Invoices", new { id = model.InvoiceId });
            }

            var payment = new PaymentTransaction
            {
                InvoiceId = model.InvoiceId,
                Amount = model.Amount,
                Currency = model.Currency,
                PaymentDate = model.PaymentDate ?? DateTime.UtcNow,
                PaymentMethod = model.PaymentMethod,
                CardLast4 = model.CardLast4,
                BankReference = model.BankReference,
                FinanceProvider = model.FinanceProvider,
                Notes = model.Notes
            };

            await _paymentService.AddPaymentAsync(payment);
            return RedirectToAction("Details", "Invoices", new { id = model.InvoiceId });
        }
    }
}
