namespace AdvancedThreading_ch21;
public class ReaderWriterLockExample
{
    private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
    private readonly List<int> _sharedList = new List<int>() { 1, 2, 3, 4, 5 };
    private readonly Random _random = new Random();

    public void StartThreads()
    {
        bool readHeld = _lockSlim.IsReadLockHeld;
        bool writerHeld = _lockSlim.IsWriteLockHeld;

        int curReadCount = _lockSlim.CurrentReadCount;

        int waitingReadCount = _lockSlim.WaitingReadCount;
        int waitingWriteCount = _lockSlim.WaitingWriteCount;

        new Thread(Read).Start();
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
                _sharedList.ForEach(num =>
                {
                    Console.WriteLine($"Reader: {num}");
                    Thread.Sleep(20);
                });
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
            int newVal = _random.Next(200);
            _lockSlim.EnterWriteLock();

            try
            {
                _sharedList.Add(newVal);
                Console.WriteLine($"{writerName}: added {newVal}");
            }
            finally
            {
                _lockSlim.ExitWriteLock();
            }

            Thread.Sleep(2000);
        }
    }
}
