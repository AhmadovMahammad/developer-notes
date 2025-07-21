namespace PortfolioApp.Lifecycle;

public interface IScopedService
{
    string GetGuid();
}

public class ScopedService : IScopedService
{
    private readonly string _guid = Guid.NewGuid().ToString();
    public string GetGuid()=> _guid;
}