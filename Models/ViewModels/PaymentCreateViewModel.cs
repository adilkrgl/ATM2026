using System.ComponentModel.DataAnnotations;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Models.ViewModels
{
    public class PaymentCreateViewModel
    {
        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "GBP";

        public DateTime? PaymentDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string? CardLast4 { get; set; }
        public string? BankReference { get; set; }
        public string? FinanceProvider { get; set; }
        public string? Notes { get; set; }
    }
}
