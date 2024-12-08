namespace Chapter8;

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
