using ATM2026.Models;
using ATM2026.Models.ViewModels;
using ATM2026.Services;
using Microsoft.AspNetCore.Mvc;

namespace ATM2026.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomersController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetCustomersAsync();
            return View("~/Views/Ecommerce/CustomerAll.cshtml", customers);
        }

        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerDetailViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                TaxNumber = customer.TaxNumber,
                Addresses = customer.Addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    Type = a.Type,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    City = a.City,
                    Postcode = a.Postcode,
                    Country = a.Country
                }),
                Invoices = customer.Invoices.Select(i => new InvoiceListItemViewModel
                {
                    Id = i.Id,
                    InvoiceNumber = i.InvoiceNumber,
                    InvoiceDate = i.InvoiceDate,
                    CustomerName = customer.Name,
                    CustomerEmail = customer.Email,
                    GrandTotal = i.GrandTotal,
                    PaymentStatus = i.PaymentStatus,
                    DeliveryStatus = i.DeliveryStatus,
                    Last4CardOrMethod = i.Payments
                        .OrderByDescending(p => p.PaymentDate)
                        .Select(p => p.PaymentMethod == PaymentMethod.Card && !string.IsNullOrWhiteSpace(p.CardLast4)
                            ? $"Card •••• {p.CardLast4}"
                            : p.PaymentMethod.ToString())
                        .FirstOrDefault()
                })
            };

            return View("~/Views/Ecommerce/CustomerDetailsOverview.cshtml", model);
        }

        public IActionResult Add()
        {
            var model = new CustomerEditViewModel
            {
                Addresses = new List<AddressViewModel>
                {
                    new()
                }
            };
            return View("~/Views/Ecommerce/CustomerAll.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CustomerEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Ecommerce/CustomerAll.cshtml", model);
            }

            var customer = new Customer
            {
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                TaxNumber = model.TaxNumber,
                Addresses = model.Addresses.Select(a => new Address
                {
                    Type = a.Type,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    City = a.City,
                    Postcode = a.Postcode,
                    Country = a.Country
                }).ToList()
            };

            await _customerService.CreateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetCustomerAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            var model = new CustomerEditViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                Phone = customer.Phone,
                TaxNumber = customer.TaxNumber,
                Addresses = customer.Addresses.Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    Type = a.Type,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    City = a.City,
                    Postcode = a.Postcode,
                    Country = a.Country
                }).ToList()
            };

            return View("~/Views/Ecommerce/CustomerAll.cshtml", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerEditViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View("~/Views/Ecommerce/CustomerAll.cshtml", model);
            }

            var customer = new Customer
            {
                Id = model.Id.GetValueOrDefault(),
                Name = model.Name,
                Email = model.Email,
                Phone = model.Phone,
                TaxNumber = model.TaxNumber,
                Addresses = model.Addresses.Select(a => new Address
                {
                    Id = a.Id.GetValueOrDefault(),
                    Type = a.Type,
                    Line1 = a.Line1,
                    Line2 = a.Line2,
                    City = a.City,
                    Postcode = a.Postcode,
                    Country = a.Country
                }).ToList()
            };

            await _customerService.UpdateCustomerAsync(customer);
            return RedirectToAction(nameof(Index));
        }
    }
}
