using TestProj;

internal class Program
{
    private static void Main(string[] args)
    {
        //AlertConfig config = new AlertConfig()
        //    .SetType("Error")
        //    .SetAutoClose(TimeSpan.FromSeconds(4).Seconds);

        //Console.WriteLine($"Alert Type: {config.Type}, Auto Close: {config.AutoCloseDuration}s");

        AlertConfig config = SweetAlertService.CreateAlert("Warning")
            .WithConfig(config =>
            {
                config.SetAutoClose(5);
            });

        Console.WriteLine($"Alert Type: {config.Type}, Auto Close: {config.AutoCloseDuration}s");
    }
}