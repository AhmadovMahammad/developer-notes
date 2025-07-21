using Microsoft.EntityFrameworkCore;
using PortfolioApp.Entities;

namespace PortfolioApp;

public class AppDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)  
    {
        base.OnModelCreating(modelBuilder);

        // Primary Key configuration
        modelBuilder.Entity<UserRole>().HasKey(ur => new { ur.UserId, ur.RoleId });  // Composite Primary Key
        modelBuilder.Entity<User>().HasKey(u => u.Id);  // Primary key for User entity
        modelBuilder.Entity<UserProfile>().HasKey(up => up.Id);  // Primary key for UserProfile entity
        modelBuilder.Entity<Order>().HasKey(o => o.Id);  // Primary key for Order Entity

        // Relationship configuration
        modelBuilder.Entity<User>()
            .HasOne(u => u.Profile)  // User has one UserProfile
            .WithOne(up => up.User)   // UserProfile has one User
            .HasForeignKey<UserProfile>(up => up.UserId); // Foreign key in UserProfile

        modelBuilder.Entity<User>()
            .HasMany(u => u.Orders)  // User has many Orders
            .WithOne(o => o.User)    // Each Order belongs to one User
            .HasForeignKey(o => o.UserId); // Foreign key in Order table

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        //modelBuilder.Entity<User>()
        //    .HasMany(u => u.Roles)
        //    .WithMany(r => r.Users)
        //    .UsingEntity(junction => junction.ToTable("UserRoles")); // EF Core will create "UserRoles" table automatically

        modelBuilder.Entity<Order>()
            .Property(o => o.Name)
            .HasMaxLength(100);

        modelBuilder.Entity<Order>()
            .Property(o => o.Price)
            .HasPrecision(18, 4);

        modelBuilder.Entity<UserProfile>()
            .Property(u => u.Bio)
            .IsRequired();

        modelBuilder.Entity<UserRole>()
            .Property(ur => ur.AssignedDate)
            .HasDefaultValueSql("GETDATE()");

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .HasDatabaseName("IX_User_Email");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Order> Orders { get; set; }
}