using Artiligence.InvoiceSystem.Web.Data;
using Artiligence.InvoiceSystem.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace Artiligence.InvoiceSystem.Web.Services
{
    public class InvoiceNumberService
    {
        private readonly AppDbContext _context;

        public InvoiceNumberService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<string> GetNextInvoiceNumberAsync()
        {
            var currentYear = DateTime.UtcNow.Year;
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var sequence = await _context.InvoiceNumberSequences
                .SingleOrDefaultAsync(s => s.Year == currentYear);

            if (sequence == null)
            {
                sequence = new InvoiceNumberSequence
                {
                    Year = currentYear,
                    LastNumber = 0
                };
                _context.InvoiceNumberSequences.Add(sequence);
            }

            sequence.LastNumber += 1;
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return $"{currentYear}{sequence.LastNumber:D4}";
        }
    }
}
