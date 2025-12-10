using Microsoft.EntityFrameworkCore;
using Artiligence.InvoiceSystem.Web.Models;

namespace Artiligence.InvoiceSystem.Web.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Address> Addresses => Set<Address>();
        public DbSet<Invoice> Invoices => Set<Invoice>();
        public DbSet<InvoiceLine> InvoiceLines => Set<InvoiceLine>();
        public DbSet<PaymentTransaction> PaymentTransactions => Set<PaymentTransaction>();
        public DbSet<InvoiceNumberSequence> InvoiceNumberSequences => Set<InvoiceNumberSequence>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Addresses)
                .WithOne(a => a.Customer!)
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Invoices)
                .WithOne(i => i.Customer!)
                .HasForeignKey(i => i.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Invoice>()
                .Property(i => i.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("GBP");

            modelBuilder.Entity<PaymentTransaction>()
                .Property(p => p.Currency)
                .HasMaxLength(3)
                .HasDefaultValue("GBP");

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Lines)
                .WithOne(l => l.Invoice!)
                .HasForeignKey(l => l.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Invoice>()
                .HasMany(i => i.Payments)
                .WithOne(p => p.Invoice!)
                .HasForeignKey(p => p.InvoiceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<InvoiceNumberSequence>()
                .HasIndex(s => s.Year)
                .IsUnique();
        }
    }
}
