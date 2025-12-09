using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ATM2026.Models;

namespace ATM2026.Data
{
    public class ATM2026Context : DbContext
    {
        public ATM2026Context(DbContextOptions<ATM2026Context> options)
            : base(options)
        {
        }
        public DbSet<ATM2026.Models.Transactions> Transactions { get; set; } = default!;
    }
}
