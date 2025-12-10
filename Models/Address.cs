using System.ComponentModel.DataAnnotations;

namespace Artiligence.InvoiceSystem.Web.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        public AddressType Type { get; set; }

        [Required]
        [StringLength(200)]
        public string Line1 { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Line2 { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Postcode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        public Customer? Customer { get; set; }
    }
}
