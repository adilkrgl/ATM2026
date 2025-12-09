using ATM2026.Data;
using ATM2026.Models;
using Microsoft.EntityFrameworkCore;

namespace ATM2026.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            return await _context.Customers
                .Include(c => c.Addresses)
                .Include(c => c.Invoices)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerAsync(int id)
        {
            return await _context.Customers
                .Include(c => c.Addresses)
                .Include(c => c.Invoices)
                .ThenInclude(i => i.Payments)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task UpdateCustomerAsync(Customer customerFromView)
        {
            var existingCustomer = await _context.Customers
                .Include(c => c.Addresses)
                .FirstOrDefaultAsync(c => c.Id == customerFromView.Id);

            if (existingCustomer == null)
            {
                throw new InvalidOperationException("Customer not found");
            }

            existingCustomer.Name = customerFromView.Name;
            existingCustomer.Email = customerFromView.Email;
            existingCustomer.Phone = customerFromView.Phone;
            existingCustomer.TaxNumber = customerFromView.TaxNumber;
            existingCustomer.DefaultBillingAddressId = customerFromView.DefaultBillingAddressId;
            existingCustomer.DefaultDeliveryAddressId = customerFromView.DefaultDeliveryAddressId;

            var incomingAddressIds = customerFromView.Addresses.Where(a => a.Id != 0).Select(a => a.Id).ToHashSet();
            var addressesToRemove = existingCustomer.Addresses.Where(a => !incomingAddressIds.Contains(a.Id)).ToList();
            _context.Addresses.RemoveRange(addressesToRemove);

            foreach (var address in customerFromView.Addresses)
            {
                var existingAddress = existingCustomer.Addresses.FirstOrDefault(a => a.Id == address.Id);
                if (existingAddress == null)
                {
                    address.CustomerId = existingCustomer.Id;
                    existingCustomer.Addresses.Add(address);
                }
                else
                {
                    existingAddress.Type = address.Type;
                    existingAddress.Line1 = address.Line1;
                    existingAddress.Line2 = address.Line2;
                    existingAddress.City = address.City;
                    existingAddress.Postcode = address.Postcode;
                    existingAddress.Country = address.Country;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
