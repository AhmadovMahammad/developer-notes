using AdvancedThreading;

internal class Program
{
    private static void Main(string[] args)
    {
        /* Synchronization Overview
        
        Synchronization is a foundational concept in multithreaded programming, 
        aimed at coordinating the execution of concurrent actions to ensure predictable and reliable outcomes. 
        When multiple threads share and modify the same data, synchronization becomes critical 
        to prevent race conditions, data corruption, and other concurrency issues.

        At its core, synchronization ensures that access to shared resources or critical sections of code is properly managed, 
        allowing threads to work together harmoniously rather than interfering with one another. 
        Without proper synchronization, even seemingly simple tasks can lead to unpredictable behavior, 
        making debugging complex multithreaded applications challenging.

        Modern .NET provides various tools to simplify synchronization.
        However, there are scenarios where these higher-level abstractions are insufficient, 
        and lower-level synchronization constructs are necessary to maintain control over thread interactions.

        Synchronization constructs can be categorized into three main groups, each serving a distinct purpose:

        1. Exclusive Locking:
        Exclusive locking mechanisms, such as lock, Mutex, and SpinLock, are used to ensure that 
        only one thread at a time can access a critical section of code or shared data. 
        These constructs are essential for managing state modifications from simultaneous updates.

        2. Nonexclusive Locking:
        Nonexclusive locking, provided by constructs like Semaphore(Slim) and ReaderWriterLock(Slim), allows controlled concurrency. 
        For example, a semaphore can limit the number of threads accessing a resource simultaneously, 
        while a reader-writer lock enables multiple readers to access shared data concurrently, but only one writer at a time.

        3. Signalling Mechanisms:
        Signaling constructs, such as ManualResetEvent(Slim), AutoResetEvent, CountdownEvent, and Barrier, facilitate communication between threads. 
        These constructs allow one thread to notify others when specific conditions are met, 
        enabling synchronized coordination in complex workflows. 
        Event wait handles like ManualResetEvent and AutoResetEvent are particularly useful for blocking threads until a signal is received.


        In addition to these, advanced synchronization techniques exist that bypass traditional locking mechanisms. 
        These nonblocking synchronization tools, such as 
            1. Thread.MemoryBarrier, 
            2. Thread.VolatileRead, 
            3. Thread.VolatileWrite, 
            4. the volatile keyword, and 
            5. the Interlocked class, 
        are designed for highly optimized, low-latency concurrent operations. 
        While powerful, they demand a deep understanding of memory models and threading to use effectively. 

        */

        /* Exclusive Locking - The lock Statement
        The lock statement is a fundamental construct in C# used for synchronization in multithreaded programs. 
        Its primary purpose is to prevent race conditions by ensuring that only one thread at a time can execute a specific section of code. 
        This is particularly important when multiple threads need to access or modify shared data, 
        as unsynchronized access can lead to unpredictable results and errors.

        To understand the importance of the lock statement, consider a scenario where two threads are working with shared variables. 
        Without synchronization, these threads can interfere with each other’s execution, leading to unexpected behavior. 
        For example, in the following code, two threads may simultaneously access shared fields _val1 and _val2:

        public class BankAccount
        {
            private int _balance;

            public BankAccount(int initialBalance)
            {
                _balance = initialBalance;
            }

            public void Deposit(int amount)
            {
                Thread.Sleep(1);
                _balance += amount;
            }

            public void Withdraw(int amount)
            {
                Thread.Sleep(1);
                if (_balance >= amount) _balance -= amount;
            }

            public int GetBalance() => _balance;
        }

        var account = new BankAccount(1000);

        // Create multiple threads performing operations on the same account
        var thread1 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account.Deposit(100);
            }
        });
        // initial deposit: 1000 
        // thread 1 deposits 1000

        var thread2 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account.Withdraw(50);
            }
        });
        // thread 2 withdraws 500
        // should be 1500

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        // Print final balance
        Console.WriteLine($"Final Balance: {account.GetBalance()}");

        This code appears straightforward but is not thread-safe. 
        The lock statement solves this problem by ensuring that only one thread can execute the critical section of code at a time. 
        To achieve this, the programmer wraps the sensitive code in a lock block and uses an object, 
        such as _locker, as the synchronization mechanism. Here’s the corrected version of the code:

        public void Deposit(int amount)
        {
            lock (_lock)
            {
                Thread.Sleep(1);
                _balance += amount;
            }
        }

        public void Withdraw(int amount)
        {
            lock (_lock)
            {
                Thread.Sleep(1);
                if (_balance >= amount) _balance -= amount;
            }
        }


        When a thread enters the lock block, it acquires a lock on the _locker object. 
        While the lock is held, no other thread can enter the same lock block with the same synchronization object. 
        This ensures that the critical section is executed by only one thread at a time, thereby preventing race conditions. 
        Any thread that attempts to acquire the lock while it is already held is blocked until the lock is released.

        However, while the lock statement is highly effective for synchronization, it is not without its challenges. 
        The performance of a program can be affected if many threads are competing for the same lock, 
        as blocked threads have to wait their turn, which can lead to contention. 
        
        In addition, improper use of locks, such as acquiring multiple locks in an inconsistent order, 
        can lead to deadlocks where threads are indefinitely waiting for each other to release locks.

        */

        /* Monitor.Enter and Monitor.Exit
         
        */

        #region codeExamples
        var account = new BankAccount(1000);

        // Create multiple threads performing operations on the same account
        var thread1 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account.Deposit(100);
            }
        });
        // initial deposit: 1000 
        // thread 1 deposits 1000

        var thread2 = new Thread(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                account.Withdraw(50);
            }
        });
        // thread 2 withdraws 500
        // should be 1500

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        // Print final balance
        Console.WriteLine($"Final Balance: {account.GetBalance()}");
        #endregion
    }
}