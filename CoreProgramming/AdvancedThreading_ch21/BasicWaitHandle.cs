
namespace AdvancedThreading_ch21;
public class BasicWaitHandle
{
    private readonly EventWaitHandle _waitHandle = new AutoResetEvent(false);

    public void Main()
    {
        new Thread(Waiter).Start();
        string input = string.Empty;

        while (!string.Equals(input, "3", StringComparison.OrdinalIgnoreCase))
        {
            Console.WriteLine("You should input 3 for activating 'Waiter' thread");
            input = Console.ReadLine() ?? string.Empty;
        }

        _waitHandle.Set();
    }

    private void Waiter()
    {
        Console.WriteLine("Waiting for the number: 3");

        if (!_waitHandle.WaitOne(5 * 1000)) Console.WriteLine("Timeout: No signal received.");
        else Console.WriteLine("Signal received within timeout.");
    }
}