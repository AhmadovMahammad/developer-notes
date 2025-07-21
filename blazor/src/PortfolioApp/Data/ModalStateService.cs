namespace PortfolioApp.Data;

public class ModalStateService
{
    public bool IsModalVisible { get; private set; }
    public event Action? OnModalVisibilityChanged;

    public void ShowModal()
    {
        IsModalVisible = true;
        NotifyStateChanged();
    }

    public void HideModal()
    {
        IsModalVisible = false;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnModalVisibilityChanged?.Invoke();
}
