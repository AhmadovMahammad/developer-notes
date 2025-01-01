namespace AdvancedThreading_ch21;
public class CountdownEventExample
{
    private const int _threadCount = 3;
    private readonly CountdownEvent _countdownEvent = new CountdownEvent(_threadCount);
    private readonly object _lock = new object();

    public void Main()
    {
        Console.WriteLine("The main thread will carry out its work when every thread has said 'hi'.");

        for (int i = 1; i <= _threadCount; i++)
        {
            int capturedThreadId = i;
            new Thread(() => Speaker(capturedThreadId)) { IsBackground = false }.Start();
        }

        _countdownEvent.Wait();
        //_countdownEvent.AddCount(1);
        //if (_countdownEvent.TryAddCount(1))
        //{
        //    Console.WriteLine("Cannot add count because the countdown has reached zero.");
        //}
        Console.WriteLine("Now that every thread has said 'hi', the main thread will carry out its execution.");
    }

    private void Speaker(int threadId)
    {
        lock (_lock)
        {
            string input = "";
            while (!string.Equals(input, "hi", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine($"Thread #{threadId} > You should write 'hi' to invoke signal.");
                input = Console.ReadLine() ?? string.Empty;
            }

            _countdownEvent.Signal();
        }
    }
}
