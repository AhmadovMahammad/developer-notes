namespace PortfolioApp.Entities;

public class Order
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }

    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
}