namespace AdvancedThreading;

public class SharedResource
{
    private readonly object _lock = new object();
    private bool lockTaken = false;
    private int _counter;

    public void Increment_v1()
    {
        Monitor.Enter(_lock);
        try
        {
            _counter++;
            Console.WriteLine($"Value incremented to: {_counter}");
        }
        finally
        {
            Monitor.Exit(_lock);
        }
    }

    public void Increment_v2()
    {
        Monitor.Enter(_lock, ref lockTaken); // Try to acquire the lock
        try
        {
            _counter++;
            Console.WriteLine($"Value incremented to: {_counter}");
        }
        finally
        {
            // Ensure the lock is released only if it was acquired
            if (lockTaken)
            {
                Monitor.Exit(_lock);
            }
        }
    }

    public void Increment_v3()
    {
        if (Monitor.TryEnter(_lock))
        {
            try
            {
                _counter++;
                Console.WriteLine($"Value incremented to: {_counter}");
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }
        else
        {
            Console.WriteLine("Could not get Lock object.");
        }
    }

    public void Increment_v4()
    {
        if (Monitor.TryEnter(_lock, 1000))
        {
            try
            {
                _counter++;
                Thread.Sleep(1500);
                Console.WriteLine($"Value incremented to: {_counter}");
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }
        else
        {
            Console.WriteLine("Could not get Lock object.");
        }
    }

    public void Increment_v5()
    {
        if (Monitor.TryEnter(_lock, TimeSpan.FromSeconds(1)))
        {
            try
            {
                _counter++;
                Thread.Sleep(1500);
                Console.WriteLine($"Value incremented to: {_counter}");
            }
            finally
            {
                Monitor.Exit(_lock);
            }
        }
        else
        {
            Console.WriteLine("Could not get Lock object.");
        }
    }
}
