using Microsoft.EntityFrameworkCore;

namespace PortfolioApp.Data;

public class AppUserService
{
    private readonly IDbContextFactory<AppDbContext> _dbContextFactory;

    public AppUserService(IDbContextFactory<AppDbContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<List<string>> GetUsersAsync()
    {
        using var context = await _dbContextFactory.CreateDbContextAsync();
        //return await context.Users.ToListAsync();

        return new List<string> { "Admin" };
    }
}
