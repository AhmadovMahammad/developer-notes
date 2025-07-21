namespace PortfolioApp.Lifecycle;

public interface ISingletonService
{
    string GetGuid();
}

public class SingletonService : ISingletonService
{
    private readonly string _guid = Guid.NewGuid().ToString();
    public string GetGuid() => _guid;
}