namespace PortfolioApp.Entities;

public class UserProfile
{
    public int Id { get; set; }
    public string Bio { get; set; } = string.Empty;

    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
}