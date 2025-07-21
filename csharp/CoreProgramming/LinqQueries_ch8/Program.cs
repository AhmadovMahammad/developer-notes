using Microsoft.EntityFrameworkCore;

namespace Chapter8;

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
        
        After you’ve defined Entity classes and subclassed DbContext, 
        you can instantiate your DbContext and query the database, as follows:

        using var dbContext = new NutshellContext();
        Console.WriteLine(dbContext.Customers.Count());
        // Executes "SELECT COUNT(*) FROM [Customer] AS [c]"

        You can also use your DbContext instance to write to the database. 
        The following code inserts a row into the Customer table:
        
        using var dbContext = new NutshellContext();
        Customer cust = new Customer()
        {
            Name = "Sara Wells"
        };
        
        dbContext.Customers.Add (cust);
        dbContext.SaveChanges(); // Writes changes back to database
        */

        /* Object Tracking
        
        Object tracking in Entity Framework Core (EF Core) refers to the ability of the DbContext 
        to keep track of the state of entity objects retrieved from the database.

        1. Header: Entity State Management

        When you retrieve entities from the database, EF Core tracks the entity's state 
        (Added, Unchanged, Modified, Deleted, or Detached). 
        
        This state management allows EF Core to know if an entity needs to be 
        1. inserted, 2. updated, or 3. deleted when SaveChanges is called.

        using var dbContext = new NutshellContext();

        // fake db seed
        var customer = new Customer
        {
            CustomerID = Guid.NewGuid(),
            FirstName = "Mahammad",
            LastName = "Ahmadov",
            Address = "Azerbaijan"
        };

        var purchase = new Purchase
        {
            ID = Guid.NewGuid(),
            Description = "Sample description",
            Price = 99.99M,
            Date = DateTime.Now,
            CustomerID = customer.CustomerID,
        };

        dbContext.Customers.Add(customer);
        dbContext.Purchases.Add(purchase);
        dbContext.SaveChanges();

        //---------- First version with tracking

        //// 1. EF Core tracks the customer after retrieving it from the database
        //var customerFromDb = dbContext.Customers.First();
        //Console.WriteLine($"First fetch: {customerFromDb.FirstName} {customerFromDb.LastName}");

        //// 2. Change the customer's name (EF Core will mark it as Modified)
        //customerFromDb.LastName = "Ahmadli";

        //// Check the entity state before calling SaveChanges
        //var entry = dbContext.Entry(customerFromDb);
        //Console.WriteLine($"Customer State: {entry.State}"); // Output: Modified

        //// Save the changes, so the modification is saved in the database
        //dbContext.SaveChanges();

        //// 3. Fetch the customer again, object tracking ensures we get the updated data
        //var updatedCustomer = dbContext.Customers.First();
        //Console.WriteLine($"Updated fetch: {updatedCustomer.FirstName} {updatedCustomer.LastName}");

        //---------- Second version with no tracking

        // 4. No-tracking query: fetch without tracking
        
        //var customerNoTracking = dbContext.Customers.AsNoTracking().First();
        //Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");

        //// Modify the entity (though it's not being tracked)
        //customerNoTracking.LastName = "Updated LastName";

        //// Attempt to check the entity state
        //var entry = dbContext.Entry(customerNoTracking);

        //// Checking the entity state will raise an exception or report "Detached" since it's not tracked.
        //Console.WriteLine($"Customer State: {entry.State}"); // Output: Detached

        2. Header: Identity Map Pattern

        EF Core follows the Identity Map pattern to ensure that each entity is unique within the scope of a DbContext

        using var dbContext = new NutshellContext();

        Customer a = dbContext.Customers.OrderBy(c => c.FirstName).First();
        Customer b = dbContext.Customers.OrderBy(c => c.CustomerID).First();
        Console.WriteLine(Object.ReferenceEquals(a, b)); // True

        The query may retrieve the same customer from the database twice, 
        but EF Core ensures that it gives you the same instance of the Customer object. 
        This helps in preventing inconsistent data states across your application.

        3. Header: AsNoTracking Queries

        If you are fetching data for read-only purposes and you don't need to track changes to entities, 
        using the AsNoTracking method improves performance by not keeping track of the entities returned by the query.

        No Changes Are Tracked: Any modifications you make to this entity will not be tracked by EF Core. 
        Therefore, if you try to call SaveChanges(), 
        EF Core won't know that anything has changed because the entity is not in the Modified state.

        Read-Only Purpose: AsNoTracking() is primarily intended for read-only purposes where 
        you don't intend to make changes to the data or persist those changes back to the database. 
        It improves performance by avoiding the overhead of tracking entities.

        using var dbContext = new NutshellContext();

        var customerNoTracking = dbContext.Customers.AsNoTracking().First();
        Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");

        // Modify the entity (though it's not being tracked)
        customerNoTracking.LastName = "Updated LastName";

        // Attempt to check the entity state
        var entry = dbContext.Entry(customerNoTracking);
        Console.WriteLine($"Customer State: {entry.State}"); // Output: Detached

        // No changes will be saved because customer is not tracked.
        dbContext.SaveChanges();

        If you want to persist changes to a non-tracked entity, you should change its state manually

        entry.State = EntityState.Modified;
        dbContext.SaveChanges(); // Now data is changed
        */

        /* Change Tracking
        
        Change tracking in EF Core is a mechanism that monitors changes to the data in your entities (objects) 
        once they're retrieved from the database. Here's a simplified explanation of the key points:

        1. Snapshot and comparison.

        When you load an entity from the database, EF Core takes a snapshot of its initial state.
        If you modify the entity, EF Core compares the current values with the original snapshot 
        when you call SaveChanges.

        2. Tracked changes:

        EF Core tracks all the changes made to the entity's properties. 
        If any changes are detected, it generates the appropriate SQL statements 
        (e.g., INSERT, UPDATE, DELETE) to reflect those changes in the database.
        
        3. SaveChanges: 
        
        When you call SaveChanges, EF Core takes the information it gathered from change tracking 
        and constructs the necessary SQL commands to update the database.

        You can enumerate the tracked changes in a DbContext as follows:

        ---CODE EXAMPLE: 

        using var dbContext = new NutshellContext();

        // EF Core tracks the customer after retrieving it from the database
        var customerFromDb = dbContext.Customers.First();
        Console.WriteLine($"First fetch: {customerFromDb.FirstName} {customerFromDb.LastName}");

        customerFromDb.FirstName = "Updated Name";

        foreach (EntityEntry entityEntry in dbContext.ChangeTracker.Entries())
        {
            Console.WriteLine($"{entityEntry.Entity.GetType().Name} is {entityEntry.State}");
            // Customer is Unchanged

            foreach (MemberEntry m in entityEntry.Members)
            {
                Console.WriteLine($" {m.Metadata.Name}: '{m.CurrentValue}' modified: {m.IsModified}");
            }
        }

        */

        /* Navigation Properties in EF Core
         
        Navigation properties in Entity Framework Core allow you to work with related entities (tables) 
        in a more intuitive way, without having to write complex SQL joins or manually handle foreign keys.

        1. Example: One-to-Many Relationship

        Customer: A customer can have many purchases (one-to-many relationship).
        Purchase: A purchase is associated with one customer.

        public class Purchase
        {
            public Guid ID { get; set; }
            public DateTime Date { get; set; }
            public string Description { get; set; } = string.Empty;
            public decimal Price { get; set; }

            // Foreign key to Customer
            public Guid CustomerID { get; set; } // Follows a common naming convention
            public Customer Customer { get; set; } = null!; // Parent navigation property
        }

        public record Customer
        {
            public Guid CustomerID { get; init; }
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Address { get; init; } = string.Empty;

            // Navigation property: A customer can have many purchases
            public virtual IEnumerable<Purchase> Purchases { get; } = new List<Purchase>();
        }

        When you generate a database schema from this model, 
        EF Core will automatically create a foreign key relationship between Purchase. 
        CustomerID and Customer.ID.

        --Writing Queries with Navigation Properties

        using var dbContext = new NutshellContext();
        var customersWithPurchases = dbContext.Customers
            .Where(c => c.Purchases.Any())
            .ToList();

        NOTE: 
        When EF Core populates an entity, it does not (by default) populate its navigation properties:
        
        var cust = dbContext.Customers.First();
        Console.WriteLine(cust.Purchases.Count()); // Always 0

        Solution #1:
        is to use the Include extension method, 
        which instructs EF Core to eagerly load navigation properties:

        var cust = dbContext.Customers
            .Include(c => c.Purchases);

        It loads all the properties of included entity.

        Solution #2:
        is to use a projection. This technique is particularly useful 
        when you need to work with only some of the entity properties, because it reduces data transfer:

        var customer = dbContext.Customers
            .Where(c => c.CustomerID != Guid.Empty)
            .Select(c => new
            {
                FullName = $"{c.FirstName} {c.LastName}",
                Purchases = c.Purchases.Select(p => new
                {
                    Description = p.Description,
                })
            }).First();

        */

        /* What is virtual?

        1. In C#, virtual is used to allow a method or property to be overridden in derived classes. 
        However, in the context of EF Core, 
        it serves a different purpose for navigation properties.

        2. By marking a navigation property as virtual, you allow EF Core to use lazy loading on that property.
        Lazy loading means that the related data (such as purchases for a customer) 
        is loaded from the database only when it is accessed, not when the entity is first queried.

        --Lazy Loading on Customer.Purchases
        
        The virtual keyword is applied to the Purchases collection in Customer 
        because you want lazy loading for the list of purchases. 
        This means EF Core will load the Purchases collection only when it is accessed, 
        not when the Customer is first retrieved from the database.

        var customer = dbContext.Customers.First(); // Purchases not loaded yet
        var purchases = customer.Purchases; // Purchases are now loaded

        The reason the Customer property inside Purchase isn't marked as virtual is because typically:
        It’s a reference to a single entity, 
        and loading a single related entity (like a parent) doesn’t incur much overhead.
         
        */

        // Code Examples
        using var dbContext = new NutshellContext();

        var customer = dbContext.Customers
            .Where(c => c.CustomerID != Guid.Empty)
            .Select(c => new
            {
                FullName = $"{c.FirstName} {c.LastName}",
                Purchases = c.Purchases.Select(p => new
                {
                    Description = p.Description,
                })
            }).First();


        var customerFromDb = dbContext.Customers.First();
        Console.WriteLine($"First fetch: {customerFromDb.FirstName} {customerFromDb.LastName}");

        var customerNoTracking = dbContext.Customers.AsNoTracking().First();
        Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");
    }
}