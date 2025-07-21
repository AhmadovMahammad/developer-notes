namespace PortfolioApp.Lifecycle;

public interface ITransientService
{
    string GetGuid();
}

public class TransientService : ITransientService
{
    private readonly string _guid = Guid.NewGuid().ToString();
    public string GetGuid() => _guid;
}