using ATM2026.Models;

namespace ATM2026.Models.ViewModels
{
    public class InvoiceCreateViewModel
    {
        public int? Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;
        public bool ShowVat { get; set; }
        public decimal VatRate { get; set; }
        public string? Notes { get; set; }
        public List<InvoiceLineViewModel> Lines { get; set; } = new();
        public string Currency { get; set; } = "GBP";
        public DeliveryStatus DeliveryStatus { get; set; } = DeliveryStatus.NotDelivered;
    }
}
