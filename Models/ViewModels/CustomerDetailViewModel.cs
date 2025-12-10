namespace Artiligence.InvoiceSystem.Web.Models.ViewModels
{
    public class CustomerDetailViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? TaxNumber { get; set; }
        public IEnumerable<AddressViewModel> Addresses { get; set; } = Enumerable.Empty<AddressViewModel>();
        public IEnumerable<InvoiceListItemViewModel> Invoices { get; set; } = Enumerable.Empty<InvoiceListItemViewModel>();
    }
}
