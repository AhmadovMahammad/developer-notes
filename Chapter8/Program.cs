using System.Net.NetworkInformation;

namespace Chapter8
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // Using EF Core

            /* EF Core Entity Classes
             
            EF Core lets you use any class to represent data, 
            as long as it contains a public property for each column that you want to query.
            
            For instance, we could define the following entity class to query and update a Customers table in the database:

            public class Customer
            {
                public Guid CustomerID { get; set; }
                public string FirstName { get; set; } = string.Empty;
                public string LastName { get; set; } = string.Empty;
                public string Address { get; set; } = string.Empty;
            }

            */

            /* DbContext
             
            After defining entity classes, the next step is to subclass DbContext. 
            An instance of that class represents your sessions working with the database.

            Typically, your DbContext subclass will contain one DbSet<T> property 
            for each entity in your model:

            public class NutshellContext : DbContext
            {
                public DbSet<Customer> Customers { get; set; } = null!;
                // properties for other tables
            }

            A DbContext object does three things:

            1. It acts as a factory for generating DbSet<> objects that you can query.
            2. It keeps track of any changes that you make to your entities so that you can write them back.
            3. It provides virtual methods that you can override to configure the connection and model.

            */

            /* Configuring the connection
             
            By overriding the OnConfiguring method, you can specify the database provider and connection string:
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(@"Server=(local);Database=Nutshell;Trusted_Connection=True");
                }
            }

            In this example, the connection string is specified as a string literal. 
            Production applications would typically retrieve it from a configuration file such as appsettings.json.

            If you’re using ASP.NET, you can allow its dependency injection framework to preconfigure optionsBuilder; 
            in most cases, this lets you avoid overriding OnConfiguring altogether.

            In the OnConfiguring method, you can enable other options, including lazy loading.

            */

            /* Configuring the Model
             
            By default, EF Core is convention based, 
            meaning that it infers the database schema from your class and property names.

            You can override the defaults by overriding OnModelCreating and 
            calling extension methods on the ModelBuilder parameter.

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Customer>()
                    .ToTable("Customer", "DefaultSchema");
            }

            Without this code, EF Core would map this entity to a table named “Customers” rather than “Customer”, 
            because we have a DbSet<Customer> property in our DbContext called Customers:

            */

            /* Creating the database
             
            A better approach is to use EF Core’s migrations feature, 
            which not only creates the database but configures it such that 
            EF Core can automatically update the schema in the future when your entity classes change.

            Install-Package Microsoft.EntityFrameworkCore.Tools
            Add-Migration InitialCreate
            Update-Database

            1. The first command installs tools to manage EF Core from within Visual Studio
            2. The second command generates a special C# class known as a code migration that 
               contains instructions to create the database.
            3. The final command runs those instructions against the database connection string specified in 
               the project’s application configuration file.
            */

            /* Using DbContext
            

            */

        }
    }
}