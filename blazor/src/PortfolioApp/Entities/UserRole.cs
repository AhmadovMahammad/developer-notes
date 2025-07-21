namespace PortfolioApp.Entities;

public class UserRole  // Junction Table
{
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;

    public int RoleId { get; set; }
    public virtual Role Role { get; set; } = null!;

    public DateTime AssignedDate { get; set; }
}