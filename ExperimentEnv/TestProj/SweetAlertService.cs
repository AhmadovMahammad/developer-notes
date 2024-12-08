namespace TestProj;

public class SweetAlertService
{
    public static AlertConfig CreateAlert(string type)
    {
        return new AlertConfig()
            .SetType(type);
    }
}

public static class AlertConfigExtensions
{
    public static AlertConfig WithConfig(this AlertConfig alert, Action<AlertConfig> configure)
    {
        configure(alert);
        return alert;
    }
}
