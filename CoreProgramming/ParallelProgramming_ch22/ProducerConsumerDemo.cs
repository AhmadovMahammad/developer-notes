using System.Collections.Concurrent;

namespace ParallelProgramming_ch22;
public class ProducerConsumerDemo
{
    private readonly CancellationTokenSource _cts;
    private readonly IProducerConsumerCollection<int> _queue;
    private readonly Random _random;

    public ProducerConsumerDemo()
    {
        _cts = new CancellationTokenSource();
        _queue = new ConcurrentQueue<int>();
        _random = new Random();

        // Start multiple producers
        var producerTasks = new Task[3];
        for (int i = 0; i < producerTasks.Length; i++)
        {
            int producerId = i + 1;
            producerTasks[i] = Task.Run(() => Producer(_queue, _cts.Token, producerId));
        }

        // Start multiple consumers
        var consumerTasks = new Task[2];
        for (int i = 0; i < consumerTasks.Length; i++)
        {
            int consumerId = i + 1;
            consumerTasks[i] = Task.Run(() => Consumer(_queue, _cts.Token, consumerId));
        }

        // Let the producers and consumers run for 5 seconds
        Task.Delay(5000).Wait();
        _cts.Cancel();

        // Wait for all tasks to complete
        Task.WhenAll(producerTasks).Wait();
        Task.WhenAll(consumerTasks).Wait();

        Console.WriteLine("Processing complete.");
    }

    private void Producer(IProducerConsumerCollection<int> queue, CancellationToken token, int producerId)
    {
        while (!token.IsCancellationRequested)
        {
            int item = _random.Next(1, 100);
            if (_queue.TryAdd(item))
            {
                Console.WriteLine($"Producer {producerId} added: {item}");
                Task.Delay(100).Wait();
            }
        }
    }

    private void Consumer(IProducerConsumerCollection<int> queue, CancellationToken token, int consumerId)
    {
        while (_queue.TryTake(out int item) && !token.IsCancellationRequested)
        {
            Console.WriteLine($"Consumer {consumerId} processed: {item}");
            Task.Delay(100).Wait();
        }
    }
}
