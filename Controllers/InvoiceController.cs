using Artiligence.InvoiceSystem.Web.Data;
using Artiligence.InvoiceSystem.Web.Models;
using Artiligence.InvoiceSystem.Web.Models.ViewModels;
using Artiligence.InvoiceSystem.Web.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Artiligence.InvoiceSystem.Web.Controllers;

public class InvoicesController : Controller
{
    private readonly AppDbContext _context;
    private readonly InvoiceService _invoiceService;
    private readonly CustomerService _customerService;

    public InvoicesController(AppDbContext context, InvoiceService invoiceService, CustomerService customerService)
    {
        _context = context;
        _invoiceService = invoiceService;
        _customerService = customerService;
    }

    public async Task<IActionResult> Index()
    {
        var invoices = await _context.Invoices
            .Include(i => i.Customer)
            .Include(i => i.Payments)
            .OrderByDescending(i => i.InvoiceDate)
            .Select(i => new InvoiceListItemViewModel
            {
                Id = i.Id,
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                CustomerName = i.Customer != null ? i.Customer.Name : string.Empty,
                CustomerEmail = i.Customer != null ? i.Customer.Email : null,
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
            .ToListAsync();

        return View("~/Views/Invoice/List.cshtml", invoices);
    }

    public async Task<IActionResult> Add()
    {
        var model = new InvoiceCreateViewModel
        {
            InvoiceDate = DateTime.UtcNow,
            VatRate = 0.2m,
            ShowVat = true,
            Lines = new List<InvoiceLineViewModel>
            {
                new()
            }
        };
        ViewBag.Customers = await _customerService.GetCustomersAsync();
        return View("~/Views/Invoice/Add.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(InvoiceCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await _customerService.GetCustomersAsync();
            return View("~/Views/Invoice/Add.cshtml", model);
        }

        var invoice = new Invoice
        {
            CustomerId = model.CustomerId,
            InvoiceDate = model.InvoiceDate,
            ShowVat = model.ShowVat,
            VatRate = model.VatRate,
            Notes = model.Notes,
            Currency = model.Currency,
            DeliveryStatus = model.DeliveryStatus,
            Lines = model.Lines.Select(l => new InvoiceLine
            {
                ProductName = l.ProductName,
                Description = l.Description,
                Colour = l.Colour,
                Detailing = l.Detailing,
                Size = l.Size,
                Orientation = l.Orientation,
                Notes = l.Notes,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity,
                DiscountType = l.DiscountType,
                DiscountValue = l.DiscountValue
            }).ToList()
        };

        var created = await _invoiceService.CreateInvoiceAsync(invoice);
        return RedirectToAction(nameof(Details), new { id = created.Id });
    }

    public async Task<IActionResult> Edit(int id)
    {
        var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        var model = new InvoiceCreateViewModel
        {
            Id = invoice.Id,
            CustomerId = invoice.CustomerId,
            InvoiceDate = invoice.InvoiceDate,
            ShowVat = invoice.ShowVat,
            VatRate = invoice.VatRate,
            Notes = invoice.Notes,
            Currency = invoice.Currency,
            DeliveryStatus = invoice.DeliveryStatus,
            Lines = invoice.Lines.Select(l => new InvoiceLineViewModel
            {
                Id = l.Id,
                ProductName = l.ProductName,
                Description = l.Description,
                Colour = l.Colour,
                Detailing = l.Detailing,
                Size = l.Size,
                Orientation = l.Orientation,
                Notes = l.Notes,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity,
                DiscountType = l.DiscountType,
                DiscountValue = l.DiscountValue
            }).ToList()
        };

        ViewBag.Customers = await _customerService.GetCustomersAsync();
        return View("~/Views/Invoice/Edit.cshtml", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, InvoiceCreateViewModel model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Customers = await _customerService.GetCustomersAsync();
            return View("~/Views/Invoice/Edit.cshtml", model);
        }

        var invoice = new Invoice
        {
            Id = model.Id.GetValueOrDefault(),
            CustomerId = model.CustomerId,
            InvoiceDate = model.InvoiceDate,
            ShowVat = model.ShowVat,
            VatRate = model.VatRate,
            Notes = model.Notes,
            Currency = model.Currency,
            DeliveryStatus = model.DeliveryStatus,
            Lines = model.Lines.Select(l => new InvoiceLine
            {
                Id = l.Id.GetValueOrDefault(),
                ProductName = l.ProductName,
                Description = l.Description,
                Colour = l.Colour,
                Detailing = l.Detailing,
                Size = l.Size,
                Orientation = l.Orientation,
                Notes = l.Notes,
                UnitPrice = l.UnitPrice,
                Quantity = l.Quantity,
                DiscountType = l.DiscountType,
                DiscountValue = l.DiscountValue
            }).ToList()
        };

        await _invoiceService.UpdateInvoiceAsync(invoice);
        return RedirectToAction(nameof(Details), new { id });
    }

    public async Task<IActionResult> Details(int id)
    {
        var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        return View("~/Views/Invoice/Preview.cshtml", invoice);
    }

    public async Task<IActionResult> Preview(int id) => await Details(id);

    public async Task<IActionResult> Print(int id)
    {
        var invoice = await _invoiceService.GetInvoiceWithDetailsAsync(id);
        if (invoice == null)
        {
            return NotFound();
        }

        return View("~/Views/Invoice/Print.cshtml", invoice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateDeliveryStatus(int id, DeliveryStatus status)
    {
        await _invoiceService.UpdateDeliveryStatusAsync(id, status);
        return RedirectToAction(nameof(Details), new { id });
    }
}
