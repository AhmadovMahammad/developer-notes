namespace TestProj;

public class AlertConfig
{
    public string Type { get; private set; } = string.Empty;
    public int AutoCloseDuration { get; private set; }

    public AlertConfig SetType(string type)
    {
        Type = type;
        return this;
    }

    public AlertConfig SetAutoClose(int duration)
    {
        AutoCloseDuration = duration;
        return this;
    }
}
