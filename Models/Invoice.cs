using System.ComponentModel.DataAnnotations;

namespace ATM2026.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string InvoiceNumber { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public int? BillingAddressId { get; set; }
        public int? DeliveryAddressId { get; set; }

        public string? Notes { get; set; }

        public bool ShowVat { get; set; }
        public decimal VatRate { get; set; }
        public decimal TotalBeforeDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalAfterDiscount { get; set; }
        public decimal TotalVatAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public string Currency { get; set; } = "GBP";

        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.NotDelivered;

        public Customer? Customer { get; set; }
        public ICollection<InvoiceLine> Lines { get; set; } = new List<InvoiceLine>();
        public ICollection<PaymentTransaction> Payments { get; set; } = new List<PaymentTransaction>();
    }
}
