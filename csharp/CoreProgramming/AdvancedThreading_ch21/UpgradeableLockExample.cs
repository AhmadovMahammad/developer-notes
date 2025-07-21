using System.ComponentModel;

namespace AdvancedThreading_ch21;
public class UpgradeableLockExample
{
    private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
    private readonly List<int> _sharedList = new List<int>() { 1, 2, 3, 4, 5 };
    private readonly Random _random = new Random();

    public void StartThreads()
    {
        new Thread(Read).Start();
        new Thread(Read).Start();
        new Thread(Write).Start("Writer A");
        new Thread(Write).Start("Writer B");
    }

    private void Read()
    {
        while (true)
        {
            _lockSlim.EnterReadLock();
            try
            {
                Console.WriteLine("Reader: Reading the list...");
                foreach (var num in _sharedList)
                {
                    Console.WriteLine($"Reader: {num}");
                    Thread.Sleep(200);
                }
            }
            finally
            {
                _lockSlim.ExitReadLock();
            }
        }
    }

    private void Write(object? writerName)
    {
        if (writerName is null) return;

        while (true)
        {
            int newNumber = _random.Next(100);
            _lockSlim.EnterUpgradeableReadLock();

            try
            {
                if (!_sharedList.Contains(newNumber))
                {
                    _lockSlim.EnterWriteLock();
                    try
                    {
                        _sharedList.Add(newNumber);
                        Console.WriteLine($"{writerName}: added {newNumber}");
                    }
                    finally
                    {
                        _lockSlim.ExitWriteLock();
                    }
                }
                else
                {
                    Console.WriteLine($"{writerName}: {newNumber} already exists.");
                }
            }
            finally
            {
                _lockSlim.ExitUpgradeableReadLock();
            }

            Thread.Sleep(2000);
        }
    }
}