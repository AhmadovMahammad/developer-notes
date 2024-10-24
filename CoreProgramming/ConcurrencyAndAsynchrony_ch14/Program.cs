using System.Buffers;
using System.Text;
using System.Threading;
using System.Threading.Channels;

namespace ConcurrencyAndAsynchrony_ch14
{
    internal class Program
    {
        static ManualResetEvent signal = new ManualResetEvent(false);
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

            1. If you forget to lock access to a shared field in just one place, 
            you can reintroduce thread safety issues.
            
            2. Locking comes with a performance cost, as threads must wait their turn to enter a locked section. 
            If many threads are contending for the same lock, this can cause slowdowns (known as contention).

            3. Locks can lead to deadlocks if two or more threads each hold a lock and are waiting on each other’s locks. 
            Deadlocks cause the program to freeze.

            */

            /* Passing Data to a Thread
             
            WAY #1

            Sometimes, you’ll want to pass arguments to the thread’s startup method. 
            The easiest way to do this is with a lambda expression that calls the method with the desired arguments:

            new Thread(() => Print("passing argument to thread")).Start();
            void Print(string message) => Console.WriteLine(message);

            With this approach, you can pass in any number of arguments to the method. 
            You can even wrap the entire implementation in a multistatement lambda:

            new Thread(() =>
            {
                Console.WriteLine("I'm running on another thread!");
                Console.WriteLine("This is so easy!");
            }).Start();


            WAY #2

            An alternative (and less flexible) technique is to pass an argument into Thread’s Start method:

            Thread t = new Thread (Print);
            t.Start ("Hello from t!");
            
            void Print (object messageObj)
            {
                string message = (string) messageObj; // We need to cast here
                Console.WriteLine (message);
            }

            This works because Thread’s constructor is overloaded to accept either of two delegates:

            1. public delegate void ThreadStart();
            2. public delegate void ParameterizedThreadStart (object obj);

            */

            /* What is Captured Values?
             
            Captured variables in C# are variables that are used inside 
            1. a lambda expression, 2. anonymous method, or 3. local function, 
            and they are "captured" from the outer scope where they are declared. 
           
            -----KEY CONCEPTS:

            Instead of capturing the current value of the variable when the lambda is created, 
            C# captures the reference to the variable. 
            This means that the lambda expression or anonymous method still points to the original memory location, 
            so any changes to that variable outside the lambda are reflected within the lambda itself.

            int x = 5;
            Action myAction = () => Console.WriteLine(x);
            x = 10;
            myAction();  // What will it print? it prints 10, not 5.

            -----Captured Variables in Loops

            This can lead to unexpected behavior, especially in loops. 
            Let’s revisit the common problem in multithreading with captured variables:

            for (int i = 0; i < 5; i++)
            {
                new Thread(() => Console.WriteLine(i)).Start();
            }

            You might expect the loop to print the numbers 0 through 4, 
            but the actual output is unpredictable because each thread captures the reference to the same variable i. 
            
            By the time the thread runs, i has likely changed, 
            so all threads may end up printing the same final value of i, like 5, or 
            a random mix of values depending on when each thread executes.

            ---Solution with Temporary Variable
            To fix this issue, you can introduce a temporary variable inside the loop to ensure 
            each thread gets its own copy of the value:

            for (int i = 0; i < 5; i++)
            {
                int temp = i;
                new Thread(() => Console.WriteLine(temp)).Start();
            }
            */

            /* Lambda expressions and captured variables

            1. Captured variables in Lambda Expressions

            In C#, when a lambda expression uses a variable from its surrounding scope, 
            the compiler captures that variable by reference. 
            
            This means that the lambda expression does not take a snapshot of the variable’s value at the time it was created; 
            rather, it refers to the same memory location as the original variable. 
            
            Any changes to the variable outside the lambda expression will also be reflected within the lambda.

            for (int i = 0; i < 10; i++)
            {
                new Thread(() => Console.Write(i)).Start();
            }

            You might expect the output to print the numbers 0 through 9, one from each thread. 
            However, the output will be non-deterministic, something like this: 0223457799

            The problem here is that the lambda expression inside each thread captures the variable i by reference, 
            and all threads share the same memory location for i. 
            
            As the loop progresses, i continues to change, so by the time each thread starts executing, 
            i might have a different value. This is why we see random numbers, not necessarily in sequence from 0 to 9.

            */

            /* Exception Handling
             
            Any try/catch/finally blocks when a thread is created do not have any impact
            on the thread when it starts executing. Consider the following program:

            try
            {
                new Thread(Go).Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("We'll never get here!");
                Console.WriteLine("Exception: {0}!", ex.Message);
            }

            void Go() { throw null; }

            The try/catch statement in this example is ineffective, 
            and the newly created thread will be encumbered with an unhandled NullReferenceException. 
            This behavior makes sense when you consider that each thread has an independent execution path.

            The remedy is to move the exception handler into the Go method:

            new Thread(Go).Start();
            void Go()
            {
                try
                {
                    throw null;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception message: {0}", ex.Message);
                }
            }

            */

            /* Foreground Versus Background Threads
             
            By default, threads you create explicitly are foreground threads. 
            Foreground threads keep the application alive for as long as any one of them is running, 
            whereas background threads do not. 
            
            After all foreground threads finish, the application ends, 
            and any background threads still running abruptly terminate.

            NOTE: Thread's foreground/background status does not affect to its priority.
            (allocation of execution time)

            You can query or change a thread’s background status using its IsBackground property:

            Thread worker = new Thread(() => Console.ReadLine());
            if (args.Length > 0) worker.IsBackground = true;

            worker.Start();

            If this program is called with no arguments, the worker thread assumes foreground
            status and will wait on the ReadLine statement for the user to press Enter. 
            
            Meanwhile, the main thread exits, but the application keeps running because 
            a foreground thread is still alive. 
            
            On the other hand, if an argument is passed to Main(),
            the worker is assigned background status, and the program exits almost immediately
            as the main thread ends (terminating the ReadLine).

            -----QUESTION? What happes to Finally blocks?

            if a background thread is still running when the process terminates, 
            any cleanup code inside finally blocks of that background thread will not execute because 
            the thread is abruptly stopped.

            This behavior can be problematic if your background thread is, for instance, 
            writing to a log file, cleaning up resources, or performing any other important task that must complete. 
            The background thread will simply be cut off without finishing its work, 
            leading to potential data corruption, resource leaks, or other undesirable behavior.

            Thread backgroundThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("reading content from files");
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    Console.WriteLine("deleting files that we read content previously");
                }
            });

            backgroundThread.IsBackground = true;
            backgroundThread.Start();

            If this is a background thread, 
            the process will terminate abruptly once the main thread finishes execution. 
            The finally block containing cleanup code will never execute because 
            the thread is interrupted when the process exits. 

            ---How to Fix the Problem:

            1. Join the Background Thread:
            You can wait for the background thread to finish by calling its Join method before exiting the program. 
            This ensures the background thread completes its work, including running any finally blocks.

             Thread backgroundThread = new Thread(() =>
            {
                try
                {
                    Console.WriteLine("reading content from files");
                }
                catch (Exception)
                {

                    throw;
                }
                finally
                {
                    Console.WriteLine("deleting files that we read content previously");
                }
            });

            backgroundThread.IsBackground = true;
            backgroundThread.Start();
            backgroundThread.Join();

            */

            /* Thread Priority
             
            In multithreaded applications, the operating system determines how much CPU time each thread gets 
            based on a combination of factors, one of which is priority. 
            
            Thread priority allows you to influence how the OS schedules the execution of threads. 
            However, misusing thread priority can negatively impact the overall performance of your application.

            enum ThreadPriority { Lowest, BelowNormal, Normal, AboveNormal, Highest }

            By default, most threads are set to the Normal priority, which ensures that they receive an average share of CPU resources. 
            Higher-priority threads tend to execute sooner or more frequently than lower-priority ones.

            */

            /* Signalling
             
            Signaling in multithreading is a mechanism that allows threads to communicate with each other 
            and coordinate their activities. 
            
            A typical scenario is when one thread needs to wait for some task to complete 
            in another thread before continuing execution. 
            This can be achieved using signaling constructs like ManualResetEvent.

            -----What is ManualResetEvent?

            A ManualResetEvent is a tool used to make one thread wait until 
            another thread gives it the signal to continue. 
            
            Imagine it like a stoplight: one thread (the waiting thread) stays stopped until 
            the other thread (the signaling thread) changes the light to green, allowing it to go.

            IMPORTANT methods of ManualResetEvent:

            WaitOne(): This method blocks the current thread until the signal is set (opened).
            Set(): This opens the signal, allowing any thread that is waiting on WaitOne() to proceed.
            Reset(): This closes the signal again, putting it back in its "waiting" state.

            var signal = new ManualResetEvent(false);

            new Thread(() =>
            {
                Console.WriteLine("Waiting for signal...");
                signal.WaitOne();

                Console.WriteLine("Got signal!");
                signal.Dispose();
            }).Start();


            Thread.Sleep(5 * 1000);
            signal.Set();

            Since ManualResetEvent is an unmanaged resource (operating system handle), 
            calling Dispose() ensures that these resources are properly cleaned up once the event is done being used. 
            Without disposing, you could potentially run into resource leaks.

            */

            /* Threading in Rich Client Applications
             
            In WPF, UWP, and Windows Forms applications, 
            executing long-running operations on the main thread makes the application unresponsive.

            -----The Problem with Updating UI from a Worker Thread
            Every UI application typically has a dedicated main thread responsible for handling 
            all UI updates and user interactions (mouse clicks, keyboard input, etc.). 
            This thread is referred to as the UI thread.

            When a background task 
            (like downloading data, doing computations, or waiting for some external resource) 
            is run on a separate thread (worker thread), 
            that thread doesn’t have access to update the UI directly. 
            The UI thread is the only one that should modify UI elements, 
            ensuring that updates happen in a coordinated, safe way.

            If you try to update the UI directly from a worker thread, 
            it can lead to unpredictable behavior or exceptions being thrown, 
            as UI components are not designed to be accessed from multiple threads simultaneously.

            -----Marshaling Work to the UI Thread.

            To update the UI safely from a worker thread, 
            the update request needs to be marshaled or forwarded to the UI thread. 
            This can be done using a few methods, depending on the framework you're working with:

            1. WPF (Windows Presentation Foundation)

            n WPF, UI components are associated with a Dispatcher object. 
            This dispatcher ensures that any UI-related task is run on the main UI thread.

            To marshal work onto the UI thread, you can call 
            either Dispatcher.Invoke or Dispatcher.BeginInvoke methods.

            2. UWP (Universal Windows Platform)
            
            UWP apps also use a dispatcher to manage UI-related tasks. 
            You can use Dispatcher.RunAsync or Dispatcher.Invoke to marshal tasks from worker threads.

            3. Windows Forms

            In Windows Forms applications, 
            you would use Control.BeginInvoke or Control.Invoke to marshal UI updates to the main UI thread.


            -----Key Differences between Invoke, BeginInvoke, and RunAsync.

            1. BeginInvoke (or RunAsync in UWP)
            
            It queues the work to be performed on the UI thread and 
            immediately returns control to the calling thread.
            
            It does not block the calling thread. 
            The calling thread continues executing its code without waiting for the UI thread to process the task.

            Since the calling thread doesn't wait for the task to complete, 
            it cannot retrieve any return values from the method invoked.

            Dispatcher.BeginInvoke(() => {
                // Code to update the UI
                txtMessage.Text = "Task Complete";
            });

            2. Invoke

            It also queues the task to be performed on the UI thread, but unlike BeginInvoke, 
            the calling thread waits until the UI thread completes the task.

            It blocks the calling thread until the UI thread processes the task. 
            This makes Invoke synchronous, meaning that the calling thread must wait.

            Because Invoke waits for the task to complete, 
            it can also retrieve a return value from the invoked method.

            private void WorkerThreadMethod()
            {
                // Invoke a method on the main thread and get a return value
                int length = (int)Dispatcher.Invoke(() => UpdateMessage("Hello, World!"));
            
                Console.WriteLine($"The length of the message is: {length}");
            }
            
            private int UpdateMessage(string message)
            {
                txtMessage.Text = message;
                return message.Length;
            }

            3. RunAsync (UWP only)

            This method is specific to UWP apps and behaves similarly to BeginInvoke. 
            It queues the task on the UI thread and 
            returns control to the calling thread immediately without blocking.

            -----Understanding the Dispatcher and Message Queue
            
            The UI thread in most desktop applications is driven by an event loop or message queue. 
            This queue handles all incoming events (like user input) and UI updates. 
            
            Here's a rough idea of how this works:
            
            When the UI thread starts (e.g., when you call Application.Run), 
            it enters a loop, waiting for things to process from its message queue.
            The loop continuously pulls tasks like:

            User inputs (keyboard or mouse events).
            UI updates.
            Tasks forwarded from background threads (via Invoke or BeginInvoke).

            */

            /* The Thread Pool
             
            The Thread Pool is a fundamental component of modern concurrency and... 
            designed to optimize the creation and management of threads.

            When creating a new thread, there is significant overhead due to the system needing 
            to allocate resources and set up the execution environment for that thread. 
            Let's break down what happens under the hood:

            1. Memory Allocation for the Thread's Stack:
            Each thread has its own stack memory, used to store local variables, 
            method call information, and other execution data. When a new thread is created:

            1. A fresh stack is allocated.
            2. The stack size can vary, but it typically takes up several megabytes of memory.
            3. This allocation involves memory management overhead, becasue
            the operating system (OS) finds and reserves space in RAM for the thread's stack.

            ====

            While this overhead is negligible for long-running operations, 
            for short, frequent tasks, the startup time becomes a major inefficiency. 
            This is where the thread pool comes in.

            It provides a pool of pre-created, reusable threads that significantly reduce this overhead 
            by recycling threads instead of creating new ones for every operation.

            -----Key Characteristics and Limitations

            1. However, thread pools come with some trade-offs and limitations. For instance, 
            threads from the pool cannot have custom names assigned to them, 
            which can make debugging more challenging. 

            2. Additionally, thread pool threads are always background threads. 
            A background thread, as discussed earlier, does not keep the process alive 
            when all foreground threads have exited. 
            This is suitable for most tasks, but in scenarios where you need guaranteed completion of critical tasks, 
            using a thread pool might not be appropriate unless you ensure proper management of thread lifetimes.

            */

        }
    }
}