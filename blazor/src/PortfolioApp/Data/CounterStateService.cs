namespace PortfolioApp.Data;

public class CounterStateService
{
    public int Count { get; private set; } = 0; // Shared state

    public event Action? OnChange;  // Notifies components when state changes

    public void IncrementCount()
    {
        Count++;
        OnChange?.Invoke();
    }
}
