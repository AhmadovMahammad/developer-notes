namespace AdvancedThreading_ch21;
public class ManualResetEventExample
{
    private readonly EventWaitHandle _gate = new EventWaitHandle(false, EventResetMode.ManualReset);

    public void Main()
    {
        for (int i = 1; i <= 3; i++)
        {
            new Thread(() => WorkerThread(i)).Start();
        }

        Console.WriteLine("Main thread: Performing initialization...");
        Thread.Sleep(2000);
        Console.WriteLine("Main thread: Initialization complete. Opening the gate!");

        _gate.Set();

        // Allow time for threads to complete their tasks
        Thread.Sleep(2000);

        Console.WriteLine("Main Thread: Closing the gate...");
        _gate.Reset();
    }

    public void WorkerThread(int threadNumber)
    {
        Console.WriteLine($"Worker: {threadNumber}: waiting for the gate");
        _gate.WaitOne();
        Console.WriteLine($"Worker: {threadNumber}: entered the gate");
    }
}
