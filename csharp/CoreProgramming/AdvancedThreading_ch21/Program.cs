﻿using AdvancedThreading_ch21;
using System.Security.AccessControl;

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
        
        A Mutex (short for Mutual Exclusion) is a synchronization object that is used to manage access to a resource 
        by multiple threads or processes. 
        
        It is conceptually similar to a lock but has the additional power of working across multiple processes. 

        --- Key Features of Mutex
        
        1. Works across processes: Unlike a lock, which is limited to thread synchronization within a single application, 
        a Mutex can synchronize threads across multiple processes. 

        This makes it suitable for situations where you want to ensure that
        only one instance of a program runs at a time, even across different processes.

        2. Slower than lock: Acquiring and releasing a Mutex is slower than a lock due to the overhead of inter-process synchronization. 
        A lock is extremely fast because it’s confined to a single process, 
        whereas a Mutex may involve OS-level coordination across processes.

        3. Thread-specific release: Just like a lock, a Mutex must be released by the thread that acquired it. 
        If you attempt to release a Mutex from a thread that did not acquire it, it will throw an exception (AbandonedMutexException).

        4. Abandoned Mutexes: If a thread acquires a Mutex and exits without releasing it (e.g., if it crashes), 
        the next thread that tries to acquire the Mutex will throw an exception. 
        This is called an abandoned mutex.


        --- How a Mutex Works

        A Mutex works by blocking access to a resource. 
        A thread that needs access to a resource first calls the WaitOne() method, 
        which blocks the thread until it can acquire the mutex (i.e., when no other thread or process holds the mutex). 
        
        Once the thread finishes using the resource, it releases the mutex using the ReleaseMutex() method.

        // Create a named Mutex that is available system-wide.
        // Use a unique name for your application (e.g., "Global\YourAppName").
        using var mutex = new Mutex(true, @"Global\MyUniqueMutexName");

        // Try to acquire the mutex for up to 3 seconds.
        if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
        {
            Console.WriteLine("Another instance of the app is running. Bye!");
            return;
        }

        try
        {
            // Run the main program logic here
            Console.WriteLine("Running.");
        }
        finally
        {
            // Always release the mutex when done
            mutex.ReleaseMutex();
        }

        -------------------------------------

        mutex.WaitOne(TimeSpan.FromSeconds(3), false) tries to acquire the mutex, waiting for up to 3 seconds. 
        If the mutex is already acquired by another instance of the program, it will not acquire it within the 3-second timeout, 
        and the application will display a message and exit.

        */

        /* Mutex and Thread Synchronization
        In contrast to lock statements, which only ensure synchronization within the same process,
        a Mutex allows synchronization between threads and processes running on the same machine.
        
        For example, if you have two different instances of a program running in two separate processes, 
        you can use a Mutex to ensure that only one instance runs at a time.
        
        ---
        For this reason, a common use of Mutexes is in preventing multiple instances of an application, such as:

        1. Single Instance Applications: 
        Ensuring that only one instance of an application runs at a time, 
        such as a desktop application that should not be opened multiple times.
        
        2. Global Resource Access: 
        Synchronizing access to resources that are shared by multiple processes, like shared files or databases.

        In cases where you don’t need cross-process synchronization, using a lock statement (which is faster) is usually preferred. 
        A lock works well when you’re working within a single process and 
        just need to synchronize access to shared data between threads.


        // Create a named Mutex that is available system-wide.
        // Use a unique name for your application (e.g., "Global\YourAppName").
        using var mutex = new Mutex(true, @"Global\MyUniqueMutexName");
        // Try to acquire the mutex for up to 3 seconds.
        if (!mutex.WaitOne(TimeSpan.FromSeconds(3)))
        {
            Console.WriteLine("Another instance of the app is running. Bye!");
            return;
        }

        try
        {
            // Run the main program logic here
            Console.WriteLine("Running.");
        }
        finally
        {
            // Always release the mutex when done
            mutex.ReleaseMutex();
        }


        */


        /* Nonexclusive Locking
        
        In multithreading and parallel programming, a semaphore is a synchronization mechanism that controls access to a shared resource by multiple threads. 
        It enforces a limit on how many threads can access the resource simultaneously. 
        To understand semaphores deeply, let's explore the concept from scratch with practical, real-world analogies and technical details.

        */

        /* What is a Semaphore?
        A semaphore can be thought of as a "counter" that tracks how many threads (or processes) can access a shared resource concurrently.
        
        Imagine a nightclub with a strict capacity limit. 
        The club can hold only a certain number of people at once.
        A bouncer at the door controls access:
        
            1. If there's space, the bouncer lets a person in.
            2. If the club is full, new arrivals must wait in line.
            3. When someone leaves the club, the bouncer lets the next person in.
        
        This is the essence of a semaphore:
        
            1. It has a capacity limit (e.g., the nightclub's maximum occupancy).
            2. It allows threads to wait when the limit is reached.
            3. It allows threads to proceed when space becomes available.
         
        --- How Does a Semaphore Work?
        
        A semaphore has two main operations:
        
            1. Wait (Acquire): Decrements the semaphore's counter, indicating that a thread is entering the resource. 
            If the counter is zero, the thread waits until another thread releases.
            
            2. Release: Increments the counter, signaling that a thread has exited the resource and space is now available.

        --- Types of Semaphores

        1. Binary Semaphore:
        A semaphore with a capacity of 1. 
        It acts like a mutex or lock because it allows only one thread to access the resource at a time.

        2. Counting Semaphore:
        A semaphore with a capacity greater than 1. 
        This is used when you want to allow multiple threads to access the resource simultaneously, up to a specific limit.



        --- Code Example

        public class Club
        {
            private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(5); // maximum capacity of 5
        
            public Club()
            {
                // Simulate 5 people trying to enter the club
                for (int i = 1; i <= 10; i++)
                {
                    int personId = i; // Capture the loop variable
                    new Thread(() => Enter(personId)).Start();
                }
            }
        
            private void Enter(int id)
            {
                Console.WriteLine($"Person {id} wants to enter.");
        
                // Wait for permission to enter (decrement the semaphore count)
                _semaphore.Wait();
        
                Console.WriteLine($"Person {id} is in!");
                Thread.Sleep(1000 * id); // Simulate time spent in the club
        
                Console.WriteLine($"Person {id} is leaving.");
        
                // Release the semaphore (increment the count)
                _semaphore.Release();
            }
        }
        -----------------------------------

        --- When to Use Semaphores
        1. Rate Limiting: Limiting the number of concurrent requests to a service or API.
        2. Throttling: Ensuring a system doesn't exceed its capacity in terms of processing threads.


                                    Semaphore vs SemaphoreSlim

        Feature	                        Semaphore	                        SemaphoreSlim
        Purpose	                        Works across processes	            Works within the same process
        Performance	                    Higher latency	                    Optimized for low latency
        Async Support	                No	                                Yes
        Cancellation Token	            No	                                Yes


        ----- Real-World Example: Web Server Request Limiting
        In a web server, there may be a limit to how many simultaneous requests the server can process. 
        Using SemaphoreSlim, you can enforce this limit as follows:

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
                string aggregatedResults = string.Join(", ", results);
                Console.WriteLine(aggregatedResults);
            }
        }

        Output:

        Request: 1 is waiting for a slot to process the request.
        Request: 2 is waiting for a slot to process the request.
        Request: 3 is waiting for a slot to process the request.
        Request: 4 is waiting for a slot to process the request.
        Request: 5 is waiting for a slot to process the request.
        Request: 6 is waiting for a slot to process the request.
        Request: 7 is waiting for a slot to process the request.
        Request: 8 is waiting for a slot to process the request.
        Request: 9 is waiting for a slot to process the request.
        Request: 10 is waiting for a slot to process the request.
        Request: 10 is processed.
        Request: 9 is processed.
        Request: 9 is leaving.
        Request: 11 is waiting for a slot to process the request.
        Request: 8 is processed.
        Request: 8 is leaving.
        Request: 12 is waiting for a slot to process the request.
        Request: 1 is processed.
        Request: 1 is leaving.
        Request: 13 is waiting for a slot to process the request.
        Request: 4 is processed.
        Request: 4 is leaving.
        Request: 14 is waiting for a slot to process the request.
        Request: 6 is processed.
        Request: 6 is leaving.
        Request: 7 is processed.
        Request: 15 is waiting for a slot to process the request.
        Request: 7 is leaving.
        Request: 3 is processed.
        Request: 16 is waiting for a slot to process the request.
        Request: 18 is waiting for a slot to process the request.
        Request: 10 is leaving.
        Request: 17 is waiting for a slot to process the request.
        Request: 3 is leaving.
        Request: 5 is processed.
        Request: 5 is leaving.
        Request: 2 is processed.
        Request: 19 is waiting for a slot to process the request.
        Request: 2 is leaving.
        Request: 20 is waiting for a slot to process the request.
        Request: 15 is processed.
        Request: 15 is leaving.
        Request: 12 is processed.
        Request: 12 is leaving.
        Request: 14 is processed.
        Request: 14 is leaving.
        Request: 11 is processed.
        Request: 11 is leaving.
        Request: 13 is processed.
        Request: 13 is leaving.
        Request: 16 is processed.
        Request: 16 is leaving.
        Request: 18 is processed.
        Request: 18 is leaving.
        Request: 17 is processed.
        Request: 17 is leaving.
        Request: 19 is processed.
        Request: 19 is leaving.
        Request: 20 is processed.
        Request: 20 is leaving.


        Output Explanation

        Up to 10 requests are processed simultaneously because the semaphore capacity is 10.
        The other requests wait until a slot becomes available.

        */

        /* Reader/Writer Locks
        
        Reader/Writer locks are a specialized synchronization mechanism designed for scenarios where 
        a shared resource is read frequently but updated only occasionally.
        
        These locks balance concurrency and safety by allowing multiple threads to read simultaneously 
        while ensuring exclusive access for writing operations. 
        
        The primary motivation behind such locks is to enhance performance in applications dominated by read operations, 
        such as caching in business servers, without compromising the integrity of data during updates.


        ----- The Need for Reader/Writer Locks

        In multi-threaded environments, concurrent read and write operations on shared data can lead to 
        race conditions, data corruption, or inconsistent states. 
        
        The simplest way to ensure safety is to use an exclusive lock (like Monitor.Enter/Exit) that serializes all access to the resource. 
        However, this approach can unnecessarily restrict concurrency, especially when most operations are reads. 
        
        For example, if 10 threads are reading from a shared cache and 1 thread occasionally writes to it, 
        an exclusive lock would block all readers whenever the writer accesses the resource, 
        leading to significant performance bottlenecks. 
        
        Reader/Writer locks address this issue by distinguishing between read and write access, optimizing concurrency for read-heavy workloads.


        ----- How Reader/Writer Locks Work

        Reader/Writer locks operate using two distinct types of locks:

        1. Read Lock:
        This lock is shared among multiple threads. If no thread is holding a write lock, 
        any number of threads can simultaneously acquire a read lock, allowing concurrent reads. 
        This maximizes parallelism and improves performance when read operations dominate.

        Write Lock:
        This lock is exclusive. When a thread acquires a write lock, it prevents all other threads - 
        whether attempting to read or write from accessing the resource. 
        This ensures that updates are atomic and data consistency is maintained.

        The fundamental rule is that a write lock blocks all other threads, while a read lock blocks only writers, not other readers.


        ----- ReaderWriterLockSlim in .NET
        The .NET framework provides the ReaderWriterLockSlim class, a modern implementation of Reader/Writer locks. 
        It replaces the older ReaderWriterLock class, which had design flaws and was significantly slower. 
        
        ReaderWriterLockSlim strikes a balance between simplicity, performance, and functionality, 
        making it ideal for scenarios where read operations are frequent and write operations are infrequent.

        --- Key Features:
        1. High Performance: Although it is slower than a simple lock (like Monitor), 
        it is significantly reduces contention in read-heavy scenarious

        2. Timeout Support: The TryEnterReadLock and TryEnterWriteLock methods allow threads to attempt acquiring locks with a timeout, 
        preventing indefinite blocking in highly contended situations.

        3. Monitoring and Debugging: Properties like IsReadLockHeld, IsWriteLockHeld and CurrentReadCount provide insights
        into the lock's state, aiding in debugging and performance tuning.


        ----- Example: Using ReaderWriterLockSlim

        using System.Security.Cryptography;

        namespace AdvancedThreading_ch21;
        public class ReaderWriterLockExample
        {
            private readonly ReaderWriterLockSlim _lockSlim = new ReaderWriterLockSlim();
            private readonly List<int> _sharedList = new List<int>();
            private readonly Random _random = new Random();
        
            public void StartThreads()
            {
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
        
                    Task.Delay(2 * 1000).Wait();
                }
            }
        }

        ReaderWriterLockExample readerWriterLockExample = new ReaderWriterLockExample();
        readerWriterLockExample.StartThreads();

        Lock Held	        New Read Lock Allowed?	        New Write Lock Allowed?
        ---------------------------------------------------------------------------
        Read Lock	        Yes	                            No
        Write Lock	        No	                            No



        --- Monitoring Lock State

        ReaderWriterLockSlim provides several properties for monitoring and debugging:
        
        IsReadLockHeld: Indicates whether the current thread holds a read lock.
        IsWriteLockHeld: Indicates whether the current thread holds a write lock.
        CurrentReadCount: Shows the number of threads currently holding read locks.
        WaitingReadCount and WaitingWriteCount: Indicate the number of threads waiting for read or write locks.
        -----
        -----
        bool readHeld = _lockSlim.IsReadLockHeld;
        bool writerHeld = _lockSlim.IsWriteLockHeld;
        
        int curReadCount = _lockSlim.CurrentReadCount;
        
        int waitingReadCount = _lockSlim.WaitingReadCount;
        int waitingWriteCount = _lockSlim.WaitingWriteCount;


        --- Conclusion
        
        Reader/Writer locks, particularly the ReaderWriterLockSlim class, 
        are a powerful tool for managing shared resources in multi-threaded environments. 
        
        By enabling concurrent reads while ensuring exclusive writes, 
        they optimize performance in scenarios dominated by read operations. 
        
        Proper use of these locks, along with careful exception handling and monitoring, 
        ensures both safety and efficiency in concurrent programming.

        */

        /* Upgradeable Locks
        
        Upgradeable locks are a specialized feature in ReaderWriterLockSlim designed to address scenarios 
        where you need to perform an operation that begins with read-only access but might escalate to require write access.
        Their main advantage is that they ensure thread safety while minimizing the time spent holding exclusive write locks. 

        --- The Need for Upgradeable Locks
        Consider the following common scenario in multithreaded programming:

        1. You have a shared resource (e.g., a list) that is frequently read but occasionally modified.
        2. You want to modify the resource only if certain conditions are met. 
           For example, add an item to a list only if it is not already present.
        3. While reading, you do not want to block other threads from accessing the resource unless absolutely necessary. 
           Write operations, however, must still be exclusive.

        The problem lies in ensuring atomicity (i.e., that no other thread modifies the resource between checking and modifying it). 
        Without upgradeable locks, this would require:

        - Releasing the read lock after verifying the condition.
        - Reacquiring a write lock to modify the resource.

        This introduces a race condition: another thread could modify the resource after you release the read lock 
        but before you acquire the write lock.
        
        Upgradeable locks solve this problem by allowing a read lock to be promoted to a write lock atomically 
        while ensuring no other threads can interfere during the upgrade process.


        ----- Behaviour of Upgradeable Locks
    
        1. Read Access with Intent to Write:
        An upgradeable lock behaves like a read lock, allowing concurrent access with other readers.
        However, only one upgradeable lock can exist at any time. 
        This ensures that two threads cannot simultaneously attempt to escalate to a write lock, which could lead to deadlock.

        2. Atomic Upgrade to Write Lock:
        The upgradeable lock can be promoted to a write lock via EnterWriteLock. 
        This operation is atomic, meaning no other thread can modify the resource during the promotion process.
    
        3. Compatibility Rules:
        Upgradeable locks are compatible with other read locks but not with write locks or additional upgradeable locks. 
        This prevents conflicts and ensures thread safety during upgrades.


        ----- How Upgradeable Locks Work

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

        --- Why This Design Is Important ?

        1. Minimizes Contention:
        By allowing multiple threads to acquire read locks concurrently and promoting to write locks only when necessary
        upgradeable locks reduce contention and improve performance.

        2. Ensures Thread Safety:
        The atomic promotion mechanism guarantees that no thread can interfere between the read and write operations, 
        maintaining the integrity of the shared resource.


        ----- Conclusion:
        
        Upgradeable locks provide a powerful mechanism for scenarios where read operations dominate but occasional writes are necessary. 
        By allowing atomic promotion to a write lock, they ensure thread safety while optimizing performance. 
        They are particularly useful in situations where write operations are rare but critical, 
        such as maintaining a unique list of elements.

        */


        /* Signaling with Event Wait Handles

        The simplest kind of signaling constructs are called event wait handles (unrelated to C# events). 
        Event wait handles come in three flavors: AutoResetEvent, ManualResetEvent(Slim), and CountdownEvent. 
        The former two are based on the common EventWaitHandle class from which they derive all their functionality.

        */

        /* AutoResetEvent
        
        An AutoResetEvent is one of the most foundational synchronization primitives in multithreaded programming, 
        enabling threads to coordinate by signaling events. 
        
        Think of it as a mechanism to implement a "turnstile" where only one thread is allowed to proceed at a time when it's signaled. 
        Its purpose is to provide a thread-safe and efficient way to allow threads to communicate, 
        especially when one thread must pause and wait for a signal from another before proceeding. 


        ----- Key Concepts of AutoResetEvent

        An AutoResetEvent is based on the EventWaitHandle class, which provides the core signaling capabilities in the .NET threading model. 
        The defining characteristic of an AutoResetEvent is that 
        it automatically resets itself to a non-signaled state as soon as one thread is allowed to pass. 

        This means that it’s designed to release exactly one thread per signal, 
        and after that, it reverts to its "closed" or "non-signaled" state.


        The signaling mechanism of AutoResetEvent is straightforward yet powerful:
        1. A thread signals an AutoResetEvent using the Set method.
        2. Any thread waiting on that event (using the WaitOne method) is unblocked and allowed to proceed.
        3. The event automatically resets after releasing the waiting thread, ensuring only one thread can proceed for each signal.


        --- How It Works
        When you create an AutoResetEvent, you specify its initial state—signaled (true) or non-signaled (false):

        - If initialized with false, threads calling WaitOne on this event will block until another thread calls Set.
        - If initialized with true, the first thread to call WaitOne will proceed immediately, and the event will reset automatically afterward.

        When a thread calls the WaitOne method, it’s essentially saying:
        --- "Wait here until I’m signaled to proceed."

        When another thread calls the Set method on the same AutoResetEvent, it signals one waiting thread to continue. 
        If no threads are waiting at the time of the signal, the AutoResetEvent remains signaled until the next WaitOne call.

        The automatic reset behavior ensures that extra signals are "wasted" rather than queued. 
        This prevents over-signaling from allowing multiple threads to proceed simultaneously.

        --- Example Explanation
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
                _waitHandle.WaitOne();
                Console.WriteLine("Notified");
            }
        }

        --- Behavior When no Thread are waiting?
        If a thread calls Set on an AutoResetEvent when no threads are currently blocked by WaitOne, 
        the event remains signaled until the next WaitOne is called. 
        This behavior is crucial to avoid race conditions where a thread signals too early, and another thread misses the signal entirely.

        However, calling Set repeatedly on a turnstile at which no one is waiting doesn’t allow an entire party through when they arrive: 
        only the next single person is let through, and the extra tickets are “wasted.”


        --- Timeouts with WaitOne

        The WaitOne method also supports a timeout, allowing a thread to stop waiting after a specified duration if the event hasn’t been signaled. 
        This is particularly useful when you want to implement a fallback behavior in case of delays or deadlocks.

        if (!_waitHandle.WaitOne(5 * 1000)) Console.WriteLine("Timeout: No signal received.");
        else Console.WriteLine("Signal received within timeout.");

        */

        /* Two-way signaling
         
        Suppose that we want the main thread to signal a worker thread three times in a row. 
        If the main thread simply calls Set on a wait handle several times in rapid succession, 
        the second or third signal can become lost because the worker might take time to process each signal.

        The solution is for the main thread to wait until the worker’s ready before signaling it. 
        We can do this by using another AutoResetEvent, as follows

        class TwoWaySignaling
        {
            static EventWaitHandle _ready = new AutoResetEvent(false);
            static EventWaitHandle _go = new AutoResetEvent(false);
            static readonly object _locker = new object();
            static string? _message;
        
            static void Main()
            {
                new Thread(Work).Start();
                _ready.WaitOne(); // First wait until worker is ready
                lock (_locker) _message = "ooo";
                _go.Set(); // Tell worker to go
                _ready.WaitOne();
                lock (_locker) _message = "ahhh"; // Give the worker another message
                _go.Set();
                _ready.WaitOne();
                lock (_locker) _message = null; // Signal the worker to exit
                _go.Set();
            }
            static void Work()
            {
                while (true)
                {
                    _ready.Set(); // Indicate that we're ready
                    _go.WaitOne(); // Wait to be kicked off...
                    lock (_locker)
                    {
                        if (_message == null) return; // Gracefully exit
                        Console.WriteLine(_message);
                    }
                }
            }
        }

        // Output:
        ooo
        ahhh

        */


        /* ManualResetEvent
        
        The ManualResetEvent is a powerful threading construct in .NET that acts like a gate controlling the flow of threads. 
        Unlike AutoResetEvent, which automatically resets after releasing one thread, 
        ManualResetEvent remains open once it is set, allowing multiple threads to pass through simultaneously. 
        To block threads again, it must be explicitly reset.

        This behavior makes ManualResetEvent suitable for scenarios where you need to allow multiple threads to proceed at once, 
        such as initializing shared resources before allowing worker threads to start. 


        --- Conceptual Overview
        
        Imagine a gate at the entrance to a park. The gate can be opened or closed.
        - When the gate is closed, people wait outside until it opens.
        - When the gate is opened, anyone waiting can walk through, and it stays open until someone explicitly closes it again.


        --- ManualResetEvent functions similarly:

        - Set(): Opens the gate, allowing all threads waiting on it to pass.
        - Reset(): Closes the gate, causing threads that subsequently call WaitOne to block until the gate opens again.

        NOTE: This behavior contrasts with AutoResetEvent, where the gate closes automatically after releasing one thread.


        --- Construction > You can create a ManualResetEvent in two ways:

        1. using its direct construct:
        var manual1 = new ManualResetEvent(false); // Initially closed

        2. using the EventWaitHandle class:
        var manual2 = new EventWaitHandle(false, EventResetMode.ManualReset);

        the false parameter indicates that the gate starts in a closed state initially.

        -----------------------------------

        ManualResetEventExample manualResetEvent = new ManualResetEventExample();
        manualResetEvent.Main();

        ---

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

        OUTPUT:
        
        Main thread: Performing initialization...
        Worker: 3: waiting for the gate
        Worker: 4: waiting for the gate
        Worker: 4: waiting for the gate
        Main thread: Initialization complete. Opening the gate!
        Worker: 3: entered the gate
        Worker: 4: entered the gate
        Worker: 4: entered the gate
        Main Thread: Closing the gate...


        --- Differences Between ManualResetEvent and ManualResetEventSlim

        # Performance:
        1. ManualResetEventSlim is optimized for scenarios with short waiting times.
        2. It uses spinning constructs (a thread keeps checking in a tight loop for a short time before blocking) to reduce overhead.
        3. Spinning avoids expensive OS-level context switching if the wait is brief.

        # Cancellation:
        1. ManualResetEventSlim allows waiting to be cancelled via a CancellationToken.

        # Compatibility:
        1. ManualResetEventSlim does not subclass WaitHandle, though it provides a WaitHandle property if needed.

        */

        /* CountdownEvent
        CountdownEvent is a synchronization object in .NET that allows a thread to wait until a specified number of signals have been received. 
        It is particularly useful when coordinating multiple threads that 
        must all complete some work before a main thread can continue.

        --- It works like a countdown:
        1. When created, it starts with an initial count value.
        2. Each call to Signal() decrements the count.
        3. Once the count reaches to zero, the Wait() method unblocks any waiting threads.


        --- Initialization
        A CountdownEvent is instantiated with an initial count, 
        which represents the number of signals that must occur before it is considered "complete."

        * var countdown = new CountdownEvent(3); // Initialize with a count of 3.
        
        The count of 3 means that the event will not signal (allow threads waiting on Wait() to proceed) 
        until three calls to Signal() are made.


        --- Signalling
        The Signal() method decrements the count by 1. 
        Each time a thread completes its task, it calls Signal() to notify the CountdownEvent.

        * countdown.Signal(); // Decrease the count by 1.
        When the count reaches zero, the event is signaled, and any threads waiting on Wait() are released.


        --- Waiting (Wait())
        The Wait() method blocks the calling thread until the count of the CountdownEvent reaches zero.

        * countdown.Wait(); // Wait until Signal() has been called the specified number of times.
        If the count is already zero when Wait() is called, the method returns immediately.


        --- code example:

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

        CountdownEventExample example = new CountdownEventExample();
        example.Main();


        --- Reincrementing the Count (AddCount and TryAddCount)
        1. AddCount(int count): Adds a specified value to the count, effectively incrementing it.
        _countdownEvent.AddCount(1);

        However, it cannot be called if the count has already reached zero. If you try, it throws an exception.
        System.InvalidOperationException: 'The event is already signaled and cannot be incremented.'

        2. TryAddCount(int count): Similar to AddCount, but safer. It returns false if the count is already zero, avoiding an exception.
        if (_countdownEvent.TryAddCount(1))
        {
            Console.WriteLine("Cannot add count because the countdown has reached zero.");
        }


        --- Resetting the Countdown (Reset)
        The Reset() method resets the countdown to its original value. 
        This is useful if you need to reuse the same CountdownEvent for multiple rounds of work.

        _countdownEvent.Reset(); // Resets to the original count.

        */

        /* Creating a Cross-Process EventWaitHandle
        The concept of a cross-process EventWaitHandle is powerful for scenarios where 
        you need inter-process communication (IPC). Here's a detailed explanation: 

        --- Key Points
        1. Purpose:

        An EventWaitHandle allows multiple threads to signal and wait on events.
        When given a name, it extends this capability across multiple processes.

        2. Named Handles:

        The name parameter uniquely identifies the EventWaitHandle.
        If the specified name already exists, the OS returns a reference to the existing handle. Otherwise, a new one is created.
        To avoid conflicts, use a unique and descriptive name (e.g., use a company/app-specific prefix).

        new Thread(() =>
        {
            EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\MyCompany");

            Console.WriteLine("Press Enter to signal the event...");
            Console.ReadLine();

            // Signal the event
            wh.Set();
            Console.WriteLine("Event has been signaled.");
        }).Start();

        new Thread(() =>
        {
            EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\MyCompany");
            Console.WriteLine("Waiting for the event to be signaled...");

            // Wait for signal
            wh.WaitOne();
            Console.WriteLine("Event was signaled! Proceeding with execution.");
        }).Start();

        */

        new Thread(() =>
        {
            EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\MyCompany");

            Console.WriteLine("Press Enter to signal the event...");
            Console.ReadLine();

            // Signal the event
            wh.Set();
            Console.WriteLine("Event has been signaled.");
        }).Start();

        new Thread(() =>
        {
            EventWaitHandle wh = new EventWaitHandle(false, EventResetMode.AutoReset, @"Global\MyCompany");
            Console.WriteLine("Waiting for the event to be signaled...");

            // Wait for signal
            wh.WaitOne();
            Console.WriteLine("Event was signaled! Proceeding with execution.");
        }).Start();

        // todo: from 'the barrier class' to 'timers'

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