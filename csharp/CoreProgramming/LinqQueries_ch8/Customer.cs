namespace Chapter8;

public record Customer
{
    public Guid CustomerID { get; init; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Address { get; init; } = string.Empty;

    // Navigation property: A customer can have many purchases
    public virtual IEnumerable<Purchase> Purchases { get; } = new List<Purchase>();
}