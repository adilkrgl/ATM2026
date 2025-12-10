using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Models.ViewModels
{
    public class InvoiceLineViewModel
    {
        public int? Id { get; set; }
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
    }
}
