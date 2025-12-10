using System.ComponentModel.DataAnnotations;

namespace Artiligence.InvoiceSystem.Web.Models.Entities
{
    public class PaymentTransaction
    {
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [StringLength(3)]
        public string Currency { get; set; } = "GBP";

        public DateTime PaymentDate { get; set; }

        public PaymentMethod PaymentMethod { get; set; }
        public string? CardLast4 { get; set; }
        public string? BankReference { get; set; }
        public string? FinanceProvider { get; set; }
        public string? Notes { get; set; }

        public Invoice? Invoice { get; set; }
    }
}
