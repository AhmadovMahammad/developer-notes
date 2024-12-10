using AdvancedThreading_ch21;

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

        The Monitor.Enter and Monitor.Exit methods are the foundation upon which C#’s lock statement is built. 
        These methods are used to create critical sections in your code, 
        ensuring that only one thread can execute the protected block of code at a time. 
        This mechanism helps prevent issues like race conditions when multiple threads attempt to access shared resources.

        To better understand how Monitor.Enter and Monitor.Exit work, let's break it down:

        When you use the lock statement in C#, the compiler automatically translates it into calls to Monitor.Enter and Monitor.Exit, 
        wrapped in a try/finally block to ensure proper release of the lock, even if an exception occurs.

        Monitor.Enter(_locker); // Acquires a lock on the specified object (_locker).
        try
        {
            // Protected code goes here.
            if (_val2 != 0) 
                Console.WriteLine(_val1 / _val2); // Example of a critical section.
            _val2 = 0;
        }
        finally
        {
            Monitor.Exit(_locker); // Releases the lock to allow other threads to acquire it.
        }

        ----- Key Points about Monitor.Enter and Monitor.Exit:
        1. Monitor.Enter acquires a lock on the specified object. 
        Only one thread at a time can acquire the lock on the same object. 
        Any other thread attempting to acquire the lock will block until the lock is released.

        2. Monitor.Exit releases the lock. This allows another thread to acquire the lock and proceed with execution.

        3. The try/finally block is critical because it ensures that the lock is always released, 
        even if an exception occurs within the critical section. 
        Failing to release the lock would cause a deadlock, preventing other threads from continuing.

        4. If Monitor.Exit is called without a prior call to Monitor.Enter on the same object, an exception is thrown. 
        This ensures that locks are correctly paired with their acquisition and release.

        Class:
        public class SharedResource
        {
            private readonly object _lock = new object();
            private int _counter;
        
            public void Increment()
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
        }

        var resource = new SharedResource();

        Thread thread1 = new Thread(resource.Increment);
        Thread thread2 = new Thread(resource.Increment);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        If it is called without Lock or Monitor statement, Output can be like this.

        // OUTPUT:
        // Value incremented to: 2
        // Value incremented to: 2

        But in this example:
        Monitor.Enter ensures that only one thread can increment the shared resource’s value at a time.
        Monitor.Exit guarantees the lock is released regardless of 
        what happens inside the critical section.


        ----- Why Use Monitor.Enter/Exit Instead of Lock?
        While the lock statement is easier and safer to use, Monitor.Enter and Monitor.Exit offer more flexibility. 
        For instance, you might use them directly if you need finer control over the locking mechanism, 
        or if you want to add custom logic before or after acquiring/releasing the lock.

        */

        /* The lockTaken overloads
        
        The lockTaken overloads of Monitor.Enter help address a subtle but important issue that could lead to a deadlock 
        if an exception occurs between calling Monitor.Enter and entering the try block. 

        Without these overloads, if an exception (like an OutOfMemoryException) is thrown before the lock is acquired, 
        the lock could be left unreleased, leading to a situation where subsequent threads are unable to acquire the lock.
        
        --- Monitor.Enter with lockTaken
        The Monitor.Enter method has an overload that takes a ref bool lockTaken parameter. 
        This overload ensures that you can safely check if the lock was successfully acquired, 
        even if an exception is thrown during the process. 
        
        The lockTaken parameter will be set to false if the lock wasn't acquired (for example, if an exception occurred), 
        which allows you to handle the situation more robustly.

        ---
        In simple terms, when we say that a lock was successfully acquired,
        it means that the thread requesting the lock was able to gain exclusive access to the resource (in this case, the object being locked).
        Here’s a breakdown:
        
            1. The lock object is like a key to a resource, and when a thread wants to access the resource, it needs to have the key.
            2. When Monitor.Enter is called, it’s like the thread trying to grab the key. 
               If no other thread is using the key, the thread gets the key and is allowed to access the resource.
            3. Successfully acquired means the thread was able to grab the key (the lock) and now has exclusive access to the resource. 
               No other thread can access it until the thread that holds the lock is done and releases it.
        ---

        public class SharedResource
        {
            private readonly object _lock = new object();
            private bool lockTaken = false;
            private int _counter;
        
            public void Increment()
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
        }


        */

        /* Monitor TryEnter Method
         
        In addition to the Enter method, Monitor provides the TryEnter method, which adds a level of flexibility 
        by allowing you to specify a timeout for acquiring the lock. 
        The TryEnter method attempts to acquire the lock and returns a bool indicating whether the lock was successfully obtained. 
        This allows your code to handle situations where acquiring the lock may take too long, instead of blocking indefinitely.

        1. TryEnter() – This version tries to acquire the lock and returns true if the lock is successfully acquired, 
        or false if the lock is unavailable.

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
                    // Ensure the lock is released only if it was acquired
                    if (lockTaken)
                    {
                        Monitor.Exit(_lock);
                    }
                }
            }
            else
            {
                Console.WriteLine("Could not get Lock object.");
            }
        }

        OUTPUT:
        Value incremented to: 1
        Could not get Lock object.


        2. TryEnter(int milliseconds) – This version allows you to specify a timeout in milliseconds. 
        If the lock cannot be acquired within the specified time, it will return false. 
        If the lock is acquired within the time frame, it returns true.

        public void Increment_v4()
        {
            if (Monitor.TryEnter(_lock, 1000))
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


        3. TryEnter(TimeSpan timeout) – This version allows specifying a timeout as a TimeSpan instead of milliseconds. 
        It works similarly to the millisecond-based overload, but it allows for more granular control over the timeout period.


        NOTES: Summary of Key Differences:
        1. Monitor.Enter requires manual management of the lock state, often paired with a try/finally block to ensure the lock is released.
        
        2. Monitor.TryEnter offers a non-blocking approach, allowing you to specify a timeout for acquiring the lock. 
           It returns a bool indicating whether the lock was successfully obtained, 
           providing a way to avoid indefinitely blocking threads when the lock isn't available.

        3. Monitor.Enter with lockTaken ensures that even if an exception occurs while trying to acquire the lock, 
           you can safely determine if the lock was actually obtained before calling Monitor.Exit.

        */

        /* Choosing the Synchronization Object
        In C#, synchronization ensures that only one thread can access certain resources at a time. 
        To manage this, we use synchronization objects that help control access to shared data or critical sections of code. 
        
        The synchronization object must be a reference type, 
        meaning it needs to be an object and not a value type like int or struct.


        ----- Key Points about Synchronization Objects:
        1. Reference Type Requirement: The reference type requirement means that 
        the object used in a lock must be something that all threads can recognize and reference consistently. 
        In simple terms, a reference type like an object or class instance allows threads to "agree" on what they're locking.
        
        For example, if you use a value type like an int, each thread might get its own copy, 
        which defeats the purpose of locking. 
        
        But with a reference type (like object _locker = new object();), 
        all threads use the same shared lock, ensuring proper synchronization.

        2. Private Synchronization Object: Typically, the synchronization object is kept private, which helps encapsulate the locking logic. 
        This ensures that no other code can accidentally lock on the same object and potentially cause issues, such as deadlocks.

        3. Synchronizing Object Can Be the Protected Resource: 
        Sometimes, the object that’s being protected by the lock can also serve as the synchronization object itself. 
        For example, in the following code:

        class ThreadSafe
        {
            List<string> _list = new List<string>();
            void Test()
            {
                lock (_list)
                {
                    _list.Add("Item 1");
                    // other operations
                }
            }
        }
        
        Here, the _list field is both the object being protected (from concurrent access) and the synchronization object. 
        However, this method can sometimes make it harder to manage the locking behavior and 
        to avoid potential issues like deadlocks.

        --- What Locking Doesn’t Do:
        Locking does not prevent other threads from calling methods on the synchronization object itself. 
        For example, if lock (x) is used, it only blocks other threads from entering the critical section protected by that lock. 
        It does not prevent them from calling other methods (like ToString()) on the same object. 

        */

        /* Nested Locking: The Concept of Reentrancy
        C#'s lock is reentrant, meaning if a thread holds a lock and tries to acquire the same lock again (nested), 
        it won’t block itself.

        ----------------

        lock (lockObject)
        {
            Console.WriteLine("Outer lock acquired");

            lock (lockObject)  // Nested lock
            {
                Console.WriteLine("Inner lock acquired");
            }

            Console.WriteLine("Inner lock released");
        }

        Console.WriteLine("Outer lock released");

        ----------------

        --- What Happens Here?

        1. The first lock on lockObject is acquired by the thread.
        
        2. The thread then enters the inner lock block.
        Even though it’s trying to acquire the same lock (lockObject), it doesn’t block itself. 
        This is because C# allows reentrant locks. So, the thread can acquire the lock again without waiting.
        
        3. Once the thread exits the inner lock, it releases the lock for the second time.
        4. Finally, when it exits the outer lock, the lock is fully released, allowing other threads to enter.


        --- Why Is Nested Locking Important?
        Nested locking is useful when you have methods that call other methods that also need to lock shared resources. For example:

        public class Bank
        {
            private readonly object _lock = new object();
            public double balance { get; private set; } = 1500;
        
            public void WithdrawWithLock(double amount)
            {
                lock (_lock)
                {
                    if (balance >= amount)
                    {
                        Console.WriteLine($"Transaction started: Withdrawing {amount}");
                        balance -= amount;
                        Console.WriteLine($"Transaction completed: {amount} withdrawn. Current balance: {balance}");
                        LogTransaction($"Withdrawn: {amount}");
                    }
                    else
                    {
                        Console.WriteLine("Insufficient funds!");
                    }
                }
            }
        
            private void LogTransaction(string message)
            {
                lock (_lock) // Same lock, reentrant behavior allows it
                {
                    Console.WriteLine($"Logging transaction: {message}");
                    Thread.Sleep(100);
                }
            }
        }

        Here, the Withdraw method calls LogTransaction, and both methods need to acquire the balanceLocker. 
        Without nested locking, the second lock on balanceLocker in the LogTransaction method would block, causing a deadlock. 
        But since C# supports reentrant locking, the thread doesn't block itself, and both operations can proceed without issues.


        --- The Pitfalls of Using Two Locks
        While nested locking is useful, you often need two or more locks in more complex scenarios to protect different resources.
        Here's why:

        1. Granularity of Locking: When dealing with multiple resources (e.g., modifying a balance and logging a transaction), 
        you may want to lock only the critical sections, not the entire method. 

        This helps with performance by reducing the time threads are blocked, 
        allowing other threads to work on different parts of the program.

        2. Avoiding Deadlocks: If you use multiple locks, always acquire them in a consistent order. 
        If two threads acquire locks in the opposite order, it can lead to a deadlock, 
        where both threads are waiting on each other to release a lock.

        // Thread 1: locks balance first, then transaction
        lock (balanceLocker) { lock (transactionLocker) { } }
        
        // Thread 2: locks transaction first, then balance
        lock (transactionLocker) { lock (balanceLocker) { } }  // This causes deadlock!


        */

        /* What Is a Deadlock?
        A deadlock occurs when two or more threads in a program are stuck waiting for each other to release resources, 
        and as a result, they are unable to proceed. 
        
        It is a situation where threads block each other indefinitely because each thread is holding a resource the other thread needs. 
        To illustrate this in simple terms:

        Thread 1 locks Resource A and needs Resource B.
        Thread 2 locks Resource B and needs Resource A.
        Now, both threads are waiting on the other to release a lock, but neither can proceed, 
        and they’re stuck in a circular waiting pattern. This is a deadlock.


        --- Deadlock Example in Code
        Imagine two resources, locker1 and locker2, and two threads. 
        Each thread tries to acquire both locks, but in reverse order. 
        This creates a deadlock because each thread is waiting for the other to release a lock.

        object locker_1 = new object();
        object locker_2 = new object();

        Thread thread_1 = new Thread(() =>
        {
            lock (locker_1)
            {
                Thread.Sleep(1000); //  simulate some work...
                Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: within locker 1");

                lock (locker_2)
                {
                    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: within locker 1");
                }
            }
        });

        Thread thread_2 = new Thread(() =>
        {
            lock (locker_2)
            {
                Thread.Sleep(1000); //  simulate some work...
                Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: within locker 2");

                lock (locker_1)
                {
                    Console.WriteLine($"Thread {Environment.CurrentManagedThreadId}: within locker 1");
                }
            }
        });

        thread_1.Start();
        thread_2.Start();

        --- What's Happening Here?

        Thread 1 locks locker1 and waits for locker2, which is held by Thread 2.
        Thread 2 locks locker2 and waits for locker1, which is held by Thread 1.

        Both threads are stuck in a circular waiting pattern. 
        Neither can proceed because each is waiting on the other to release the lock it needs. 
        This is a classic example of a deadlock.

        --- Why Is This Dangerous?
        Deadlocks can cause a program to freeze, leading to:

        1. Infinite waiting: The threads involved will never complete their tasks.
        2. Performance degradation: The program becomes unresponsive, as it’s stuck waiting for resources that will never be released.
        3. Difficult debugging: Deadlocks can be hard to detect and reproduce because they might only occur in specific timing conditions.

        --- How to Prevent Deadlocks?

        1. Locking in a Consistent Order
        
        One of the simplest ways to avoid deadlocks is to acquire locks in a consistent order. 
        If both threads always acquire the locks in the same order, 
        they won’t end up in a situation where one thread holds a lock and is waiting for the other.

        2. Timeout for Lock Acquisition

        Another method is to set a timeout for acquiring a lock. 
        If a thread cannot acquire a lock within a certain period, it can give up or try again later. 
        This prevents threads from waiting indefinitely.

        object locker_1 = new object();
        object locker_2 = new object();
        bool lockAcquired = false;

        Thread thread_1 = new Thread(() =>
        {
            while (!lockAcquired)
            {
                if (Monitor.TryEnter(locker_1, TimeSpan.FromSeconds(1))) // Try to lock locker1 within 1 second
                {
                    try
                    {
                        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
                        Thread.Sleep(TimeSpan.FromSeconds(1));
                        if (Monitor.TryEnter(locker_2, TimeSpan.FromSeconds(1))) // Try to lock locker2 within 1 second
                        {
                            try
                            {
                                Console.WriteLine($"{Environment.CurrentManagedThreadId}");
                                lockAcquired = true;
                            }
                            finally
                            {
                                Monitor.Exit(locker_2);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(locker_1);
                    }
                }
            }
        });

        Thread thread_2 = new Thread(() =>
        {
            while (!lockAcquired)
            {
                if (Monitor.TryEnter(locker_2, TimeSpan.FromSeconds(1))) // Try to lock locker1 within 1 second
                {
                    try
                    {
                        Console.WriteLine($"{Environment.CurrentManagedThreadId}");
                        Thread.Sleep(TimeSpan.FromMilliseconds(500));
                        if (Monitor.TryEnter(locker_1, TimeSpan.FromSeconds(1))) // Try to lock locker2 within 1 second
                        {
                            try
                            {
                                Console.WriteLine($"{Environment.CurrentManagedThreadId}");
                                lockAcquired = true;
                            }
                            finally
                            {
                                Monitor.Exit(locker_1);
                            }
                        }
                    }
                    finally
                    {
                        Monitor.Exit(locker_2);
                    }
                }
            }
        });

        thread_1.Start();
        thread_2.Start();

        With this approach, if a thread cannot acquire both locks within the timeout, 
        it will revert and try again later, potentially preventing a deadlock situation.

        */

        /* Mutex in C#
         
        */

        #region codeExamples
        //Bank bank = new Bank();

        //Thread thread1 = new Thread(() => bank.WithdrawWithLock(300));  // Withdraw 300 from account
        //Thread thread2 = new Thread(() => bank.WithdrawWithLock(500));  // Withdraw 500 from account

        //thread1.Start();
        //thread2.Start();

        //thread1.Join();
        //thread2.Join();

        //Console.WriteLine("Final Balance (with lock): " + bank.balance);

        // ------------------------------------------

        //var resource = new SharedResource();

        //Thread thread1 = new Thread(resource.Increment_v3);
        //Thread thread2 = new Thread(resource.Increment_v3);

        //Thread thread1 = new Thread(resource.Increment_v4);
        //Thread thread2 = new Thread(resource.Increment_v4);

        //thread1.Start();
        //thread2.Start();

        //thread1.Join();
        //thread2.Join();


        // ----------------------------------------------------

        //var resource = new SharedResource();

        //Thread thread1 = new Thread(resource.Increment);
        //Thread thread2 = new Thread(resource.Increment);

        //thread1.Start();
        //thread2.Start();

        //thread1.Join();
        //thread2.Join();

        //OUTPUT:
        //Value incremented to: 2
        //Value incremented to: 2

        // ----------------------------------------------------

        //var account = new BankAccount(1000);

        //// Create multiple threads performing operations on the same account
        //var thread1 = new Thread(() =>
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        account.Deposit(100);
        //    }
        //});
        //// initial deposit: 1000 
        //// thread 1 deposits 1000

        //var thread2 = new Thread(() =>
        //{
        //    for (int i = 0; i < 10; i++)
        //    {
        //        account.Withdraw(50);
        //    }
        //});
        //// thread 2 withdraws 500
        //// should be 1500

        //thread1.Start();
        //thread2.Start();

        //thread1.Join();
        //thread2.Join();

        //// Print final balance
        //Console.WriteLine($"Final Balance: {account.GetBalance()}");
        #endregion
    }
}