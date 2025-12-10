using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Artiligence.InvoiceSystem.Web.Data;
using Artiligence.InvoiceSystem.Web.Models.Entities;

namespace Artiligence.InvoiceSystem.Web.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>()))
            {
                if (context == null)
                {
                    throw new ArgumentNullException("Null database context");
                }

                if (context.Customers.Any())
                {
                    return;
                }

                var customer = new Customer
                {
                    Name = "Acme Fencing Ltd",
                    Email = "accounts@acmefencing.test",
                    Phone = "+44 20 7946 0000",
                    TaxNumber = "GB123456789",
                    Addresses = new List<Address>
                    {
                        new()
                        {
                            Type = AddressType.Billing,
                            Line1 = "1 High Street",
                            City = "London",
                            Postcode = "SW1A 1AA",
                            Country = "United Kingdom"
                        },
                        new()
                        {
                            Type = AddressType.Delivery,
                            Line1 = "Warehouse 4",
                            Line2 = "Industrial Estate",
                            City = "Croydon",
                            Postcode = "CR0 1AA",
                            Country = "United Kingdom"
                        }
                    }
                };

                context.Customers.Add(customer);
                context.SaveChanges();

                var invoice = new Invoice
                {
                    CustomerId = customer.Id,
                    InvoiceNumber = "20240001",
                    InvoiceDate = DateTime.UtcNow.Date,
                    ShowVat = true,
                    VatRate = 0.2m,
                    Notes = "Sample invoice seeded for demo",
                    Currency = "GBP",
                    DeliveryStatus = DeliveryStatus.BookedForDelivery,
                    Lines = new List<InvoiceLine>
                    {
                        new()
                        {
                            ProductName = "Timber fence panel",
                            Description = "Featheredge treated",
                            Colour = "Brown",
                            Size = "6ft x 6ft",
                            UnitPrice = 45.00m,
                            Quantity = 5,
                            DiscountType = DiscountType.None,
                            DiscountValue = 0
                        },
                        new()
                        {
                            ProductName = "Galvanised nails",
                            Description = "50mm pack",
                            UnitPrice = 4.50m,
                            Quantity = 3,
                            DiscountType = DiscountType.Percent,
                            DiscountValue = 10
                        }
                    }
                };

                var invoiceService = new Services.InvoiceService(context, new Services.InvoiceNumberService(context));
                invoiceService.CalculateTotals(invoice);
                context.Invoices.Add(invoice);
                context.SaveChanges();

                var payment = new PaymentTransaction
                {
                    InvoiceId = invoice.Id,
                    Amount = invoice.GrandTotal / 2,
                    Currency = "GBP",
                    PaymentDate = DateTime.UtcNow,
                    PaymentMethod = PaymentMethod.Card,
                    CardLast4 = "4242",
                    Notes = "Deposit"
                };

                context.PaymentTransactions.Add(payment);
                context.SaveChanges();
            }
        }
    }
}
