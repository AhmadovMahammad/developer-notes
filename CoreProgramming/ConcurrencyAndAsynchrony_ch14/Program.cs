using System.Text;

namespace ConcurrencyAndAsynchrony_ch14
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Threads and Processes
             
            A process is the operating system's isolated environment in which a program runs, 
            providing all necessary resources like memory, file handles, and security boundaries. 
            Every application you run on your computer (e.g., a browser, editor) is a process.

            Within a single-threaded application, there's only one execution path, and 
            that path has exclusive access to the process’s environment. 
            In such programs, everything happens sequentially—tasks are handled one after another. 
            
            So if your program needs to perform a time-consuming task, 
            like downloading a large file, everything else must wait until that task is complete.

            -----Multithreading

            In a multithreaded program, multiple threads run within a single process. 
            This means that different parts of your program can execute independently but 
            still have access to the same memory and resources. 
            
            For instance, one thread might be downloading data from the internet, 
            while another thread simultaneously processes and displays the already downloaded portions.

            Shared state refers to the data or memory that multiple threads can access simultaneously. 
            While this is what makes multithreading powerful, it also introduces complexity because 
            multiple threads accessing shared data concurrently can lead to issues like race conditions, 
            where the program's outcome depends on the order in which threads access and modify shared data.

            ---Challenges in Multithreading
            1. Race conditions: 
            When two threads try to access and modify shared data at the same time, 
            the final result can depend on the timing of each thread. This makes program behavior unpredictable.

            2. Deadlocks:
            If two or more threads get stuck waiting for each other to release resources they need, none of them can proceed.

            */

            /* Creating a Thread
             
            When a typical application (Console, WPF, UWP, Windows Forms, etc.) starts,
            it is given one thread by the operating system, known as the main thread. 
            This thread is where your program begins execution. 

            By default, it remains a single-threaded application, 
            meaning only one path of execution is running at any given time. 
            However, to make your program capable of handling multiple tasks at the same time, you can create more threads.

            Thread t = new Thread(WriteY);
            t.Start();

            for (int i = 0; i < 300; i++) Console.Write("x");
            void WriteY()
            {
                for (int i = 0; i < 300; i++) Console.Write("y");
            }

            you might see something like this:
            xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            xyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
            yyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyyy
            yyyyyyyyyyyyyyyyyyyyyyyyyyyyyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxyyyyyyxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx
            xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx

            The main thread creates a new thread t on which it runs a method that repeatedly prints the character y. 
            Simultaneously, the main thread repeatedly prints the character x

            -----Single-Core vs Multi-Core

            Single-Core: 
            On a single-core machine, threads don’t actually run at the same time. 
            Instead, the OS switches between the threads rapidly (usually in intervals of 20 milliseconds) 
            to create the illusion of parallelism. 
            This is called time-slicing or preemptive multitasking. 
            In this case, the OS "freezes" the execution of one thread and "resumes" another.

            Multi-Core: 
            On a multicore machine, each core can execute a thread in parallel. 
            So, one thread might be running on Core 1 while another runs on Core 2, 
            potentially leading to truly parallel execution.

            NOTE:

            After it’s started, a thread’s IsAlive property returns true, until the point at which the thread ends. 
            A thread ends when the delegate passed to the Thread’s constructor finishes executing. 
            After it’s ended, a thread cannot restart.

            Each thread has a Name property that you can set for the benefit of debugging. 
            This is particularly useful in Visual Studio because the thread’s name is displayed in the...
            Threads Window and Debug Location toolbar. 

            */

            /* Join and Sleep
             
            1. Thread.Join()

            The Join method allows you to pause the execution of the calling thread until the target thread has finished executing. 
            In other words, if thread A calls Join on thread B, 
            thread A will stop and wait for thread B to finish before continuing its execution.

            Thread t = new Thread(delegate ()
            {
                for (int i = 0; i < 100; i++)
                {
                    Console.Write("t");
                }
            });
            t.Start();
            t.Join();

            Console.WriteLine("Thread t has ended!");

            In the example above, the main thread starts a new thread t that prints "t" 100 times. 
            The main thread then calls t.Join(), so it waits for t to finish before continuing.
            Once t is done, it prints "Thread t has ended!".

            You can also call Join with a timeout, either in milliseconds or as a TimeSpan. 
            The method then returns true if the thread has finished within the timeout period and 
            false if the timeout expires and the thread is still running.

            2. Thread.Sleep()

            Thread.Sleep() pauses the current(main) thread for a specified period, allowing other threads to execute. 
            The thread resumes execution after the sleep time elapses.

            Thread t = new Thread(delegate ()
            {
                for (int i = 0; i < 1000; i++)
                {
                    Console.Write("t");
                }
            });
            t.Start();
            Thread.Sleep(TimeSpan.FromSeconds(10));

            for (int i = 0; i < 1000; i++)
            {
                Console.Write("_");
            }

            after 10 seconds, main thread starts to print _

            */

            /* I/O-bound versus compute-bound
             
            The distinction between I/O-bound and compute-bound operations is important for understanding 
            how to manage and optimize program performance, 
            especially when dealing with concurrency and asynchronous programming.

            1. I/O-bound Operations:
            
            An I/O-bound operation is one that spends most of its time waiting for external events
            rather than actively using the CPU. 
            
            These external events could include reading from or writing to a file, 
            making a network request, interacting with a database, or waiting for user input.

            * Downloading a web page.
            * Reading a file from disk.
            * Waiting for a response from a web API.
            * Waiting for user input (e.g., Console.ReadLine).
            * Using Thread.Sleep to pause execution for a certain time.

            In I/O-bound tasks, the program is largely idle while waiting for the operation to complete, 
            meaning that the CPU is not being used much. 
            The performance bottleneck in these cases is the waiting time 
            (e.g., the time it takes to get data from an external source).

            2. Compound bound Operations:

            A compute-bound operation is one that spends most of its time actively using the CPU 
            to perform calculations or other processing. 
            This means that the operation is doing CPU-intensive work without much (or any) waiting on external events.

            * Performing complex mathematical calculations.
            * Rendering a large 3D scene.
            * Running a machine learning model inference.
            * Sorting a large array of data.

            The performance bottleneck in compute-bound operations is the CPU's processing speed. 

            */

            /* Local Versus Shared State
             
            1. LOCAL STATE

            When a thread executes a method, the local variables defined in that method 
            (inside the method body) exist on that thread’s stack. 
            
            Each thread gets its own stack, so there is no chance of interference from other threads 
            when accessing these local variables.

            Local variables are thread-local, 
            meaning they are isolated per thread and cannot be shared between threads. 
            This makes them inherently thread-safe.

            Thread t1 = new Thread(DoWork);
            Thread t2 = new Thread(DoWork);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            void DoWork()
            {
                for (int i = 0; i < 10; i++)
                {
                    Console.WriteLine(i);
                }
            }

            NOTES:
            1. Each thread has its own local copy of the variable i (from the for-loop).
            2. No interference happens between threads, as they each work with their own stack.

            ==================================================================================

            2. SHARED STATE

            When threads share variables—such as fields, static fields, or captured variables 
            from outer scopes, they can both access and modify the same piece of memory. 
            This introduces race conditions and the potential for thread safety issues.

            In shared state scenarios, 
            if one thread reads or modifies the value while another thread is doing the same, 
            you can have unpredictable results. 
            This behavior is often referred to as a data race.

            bool _done = false;

            Thread t1 = new Thread(SetFlag);
            Thread t2 = new Thread(SetFlag);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            void SetFlag()
            {
                if (!_done)
                {
                    Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} - setting flag");
                    _done = true;
                }
            }

            Both threads may see that _done is false and attempt to set it to true. 
            This can cause unpredictable output where both threads print "Setting flag", 
            even though we only want it to print once.

            */

            /* Locking and Thread Safety

            When multiple threads access shared state simultaneously, it can lead to indeterminacy. 
            This means that the outcome of your program might change depending on the timing of thread execution, 
            leading to inconsistent results or "race conditions."

            C# provides the lock statement to ensure that only one thread at a time 
            can enter a specific block of code. 
            This prevents race conditions and makes the code thread-safe.

             bool _done = false;
            object _lock = new object();

            Thread t1 = new Thread(SetFlag);
            Thread t2 = new Thread(SetFlag);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            void SetFlag()
            {
                lock (_lock)
                {
                    if (!_done)
                    {
                        Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} - setting flag");
                        _done = true;
                    }
                }
            }

            _locker is an object used as a lock, allowing threads to coordinate access. 
            In this case, the lock ensures that only one thread can access the critical section of code at any given time.

            You can use any reference type as a lock object, 
            but it’s common to use a dedicated readonly object to avoid accidentally changing the object or 
            using it for other purposes.

            -----Locking Pitfalls

            */

            // code example for shared state
            bool _done = false;
            object _lock = new object();

            Thread t1 = new Thread(SetFlag);
            Thread t2 = new Thread(SetFlag);

            t1.Start();
            t2.Start();

            t1.Join();
            t2.Join();

            void SetFlag()
            {
                lock (_lock)
                {
                    if (!_done)
                    {
                        Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId} - setting flag");
                        _done = true;
                    }
                }
            }

            // code example for thread-safe code (local state)
            //Thread t1 = new Thread(DoWork);
            //Thread t2 = new Thread(DoWork);

            //t1.Start();
            //t2.Start();

            //t1.Join();
            //t2.Join();

            //void DoWork()
            //{
            //    for (int i = 0; i < 10; i++)
            //    {
            //        Console.WriteLine(i);
            //    }
            //}
        }
    }
}