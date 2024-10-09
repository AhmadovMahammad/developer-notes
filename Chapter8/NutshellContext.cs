using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Chapter8
{
    public record Customer
    {
        public Guid CustomerID { get; init; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
    }

    public class NutshellContext : DbContext
    {
        public NutshellContext(DbContextOptions<NutshellContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(local);Database=Nutshell;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>()
                .ToTable("Customer", "DefaultSchema");
        }

        public DbSet<Customer> Customers { get; set; } = null!;
        // properties for other tables
    }
}