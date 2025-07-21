namespace AdvancedThreading_ch21;

public class WebServer
{
    private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(10); // allow 10 concurrent requests at a time
    private readonly object _lock = new object();
    private readonly List<string> _res = new List<string>();

    public async Task Run()
    {
        Task[] tasks = new Task[20];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = ProcessRequestAsync(i + 1);
        }

        await Task.WhenAll(tasks);
    }

    private async Task ProcessRequestAsync(int reqID)
    {
        // Wait for a slot to process the request
        await _semaphoreSlim.WaitAsync();
        Console.WriteLine($"Request: {reqID} is waiting for a slot to process the request.");

        try
        {
            // Simulate processing work
            await Task.Delay(2000);

            lock (_lock)
            {
                Console.WriteLine($"Request: {reqID} is processed.");
                _res.Add($"Request: {reqID} is processed.");
            }
        }
        finally
        {
            // Release the semaphore slot
            _semaphoreSlim.Release();
            Console.WriteLine($"Request: {reqID} is leaving.");
        }
    }

    private void HandleResults(List<string> results)
    {
        //string aggregatedResults = string.Join(", ", results);
        //Console.WriteLine(aggregatedResults);
    }
}
