using Microsoft.EntityFrameworkCore;

namespace Chapter8
{
    public class NutshellContext : DbContext
    {
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