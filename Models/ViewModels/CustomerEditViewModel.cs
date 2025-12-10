using System.ComponentModel.DataAnnotations;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Models.ViewModels
{
    public class CustomerEditViewModel
    {
        public int? Id { get; set; }

        [Required]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? TaxNumber { get; set; }

        public List<AddressViewModel> Addresses { get; set; } = new();
    }

    public class AddressViewModel
    {
        public int? Id { get; set; }
        public AddressType Type { get; set; }
        public string Line1 { get; set; } = string.Empty;
        public string? Line2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string Postcode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
    }
}
