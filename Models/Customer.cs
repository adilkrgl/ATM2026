using System.ComponentModel.DataAnnotations;

namespace Artiligence.InvoiceSystem.Web.Models.Entities
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? TaxNumber { get; set; }

        public int? DefaultBillingAddressId { get; set; }
        public int? DefaultDeliveryAddressId { get; set; }

        public ICollection<Address> Addresses { get; set; } = new List<Address>();
        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
