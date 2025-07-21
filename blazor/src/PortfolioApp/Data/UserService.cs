using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PortfolioApp.Entities;

namespace PortfolioApp.Data;

//public class UserService
//{
//    private int _nextId;

//    private readonly Dictionary<int, string> _users = new()
//    {
//        { 0, "Mahammad Ahmadov" },
//        { 1, "Lagertha" },
//        { 2, "Bjorn Ironside" }
//    };

//    public event Action? OnUserChanged;

//    public string GetUser(int id)
//    {
//        return _users[id];
//    }

//    public string GetCurrentUser()
//    {
//        return _users[_nextId];
//    }

//    public void SetCurrentUser(string name)
//    {
//        _nextId = _users.Count;
//        _users.Add(++_nextId, name);
//        OnUserChanged?.Invoke();
//    }

//    public IEnumerable<string> GetUsers()
//    {
//        return _users.Values;
//    }

//    public void AddUser(int id, string name)
//    {
//        _users.Add(id, name);
//    }

//    public void RemoveUser(int id)
//    {
//        _users.Remove(id);
//    }

//    public void UpdateUser(int id, string name)
//    {
//        _users[id] = name;
//    }

//    public bool UserExists(int id)
//    {
//        return _users.ContainsKey(id);
//    }

//    public bool UserExists(string name)
//    {
//        return _users.ContainsValue(name);
//    }

//    public int GetUserCount()
//    {
//        return _users.Count;
//    }

//    public void ClearUsers()
//    {
//        _users.Clear();
//    }
//}

public class UserService
{
    private readonly AppDbContext _context;

    public UserService(AppDbContext context)
    {
        _context = context;
        //TestState();
    }

    private void TestState()
    {
        var user = _context.Users.FirstOrDefault(u => u.Id == 1);
        if (user == null) return;

        Console.WriteLine($"Before Change: {user.Name}, State: {_context.Entry(user).State}");

        user.Name = "Updated Name";
        Console.WriteLine($"After Change: {user.Name}, State: {_context.Entry(user).State}");

        var newUser = new User { Name = "Ahmadov", Email = "dev.ahmadov.mahammad@gmail.com" };
        _context.Users.Add(newUser);
        Console.WriteLine($"New User: {newUser.Name}, State: {_context.Entry(newUser).State}");

        foreach (var entry in _context.ChangeTracker.Entries<User>())
        {
            Console.WriteLine($"Entity: {entry.Entity.Name}, State: {entry.State}");
        }

        _context.SaveChanges();

        foreach (var entry in _context.ChangeTracker.Entries<User>())
        {
            Console.WriteLine($"After Save - Entity: {entry.Entity.Name}, State: {entry.State}");
        }
    }

    public async Task<List<User>> GetUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Profile)
            .ToListAsync();
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Profile)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task AddUserAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateUserAsync(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user is not null)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }
    }
}
