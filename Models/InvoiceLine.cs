using System.ComponentModel.DataAnnotations;

namespace Artiligence.InvoiceSystem.Web.Models
{
    public class InvoiceLine
    {
        public int Id { get; set; }

        [Required]
        public int InvoiceId { get; set; }

        [Required]
        [StringLength(200)]
        public string ProductName { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? Colour { get; set; }
        public string? Detailing { get; set; }
        public string? Size { get; set; }
        public string? Orientation { get; set; }
        public string? Notes { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public decimal LineTotalAfterDiscount { get; set; }

        public Invoice? Invoice { get; set; }
    }
}
