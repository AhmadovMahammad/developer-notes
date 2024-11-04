using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ConcurrencyAndAsynchrony_ch14
{
    internal class Program
    {
        static ManualResetEvent signal = new ManualResetEvent(false);
        private static async Task Main(string[] args)
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
            If two or more threads get stuck waiting for each other to release resources they need, 
            none of them can proceed.

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

            //========================================================================================

            /* Tasks - Introduction
             
            Threads are fundamental for running code concurrently but come with complexity, 
            particularly in handling data and exceptions. 
            A Task, by contrast, abstracts these details, giving us a simple way to manage concurrent work 
            without dealing directly with threads.

            Threads have limitations:

            1. Although it’s easy to pass data into a thread that you start, 
            there’s no easy way to get a “return value” back from a thread that you Join.

            Direct threads can be challenging when trying to return values. 
            For example, if a thread performs a calculation, you need shared fields to access the result, 
            which then requires careful synchronization to avoid race conditions..

            2. With a thread, if an exception occurs, handling it is more complex and 
            may require catching it at the thread's end or in a shared field. 
            
            ---What can i do with Tasks?

            1. Task Composition and Fine-Grained Concurrency

            Tasks are ideal for building asynchronous workflows by composing small operations into larger, complex tasks. 
            This ability to chain or continue tasks after one finishes is invaluable in parallel programming, 
            especially when each stage of a process depends on the outcome of the previous one.

            _simple example to this chain:

             Task<int> task1 = Task.Run(() => PerformCalculation(5, 10));

            Task<int> task2 = task1.ContinueWith(previousTask =>
            {
                int resultFromTask1 = previousTask.Result;
                Console.WriteLine($"Result from Task 1: {resultFromTask1}");
                return resultFromTask1 * 2;
            });

            int finalResult = await task2;
            Console.WriteLine($"Final Result from Task 2: {finalResult}");

            async Task<int> PerformCalculation(int a, int b)
            {
                await Task.Delay(3 * 1000);
                return a + b;
            }

            */

            /* Starting a Task
             
            Task.Run is a method that allows you to run a piece of code asynchronously using a task, 
            which is a higher-level abstraction over threads. 

            When you call Task.Run, you pass an Action delegate, which is a method that does not return a value. 
            Task.Run(() => Console.WriteLine("hello from separate thread"));

            Tasks use pooled threads by default, which are background threads. 
            This means that when the main thread ends, so do any tasks that you create.

            Calling Task.Run in this manner is similar to starting a thread as follows 
            (except for the thread pooling):

            new Thread(() => Console.WriteLine("hello from separate thread)).Start();

            When you call Task.Run, it returns a Task object, which you can use to monitor the task's status and progress. 
            For instance, you can check properties like Status to see if the task is running, completed, or faulted.

            ---Cold versus Hot Tasks

            Calling Task.Run starts the task immediately, meaning it's a "hot" task. 
            You don't need to call Start explicitly as you would with a Thread object.
            
            There are "cold" tasks, where you create a task but do not start it immediately. 
            This is less common and usually not needed in practical applications.

            */

            /* Waiting a Task
             
            1. Blocking Behavior:

            Calling Wait on a task causes the current thread to block until the task has completed its execution. 
            This is similar to calling Join on a thread, which also waits for the thread to finish its work 
            before allowing the program to continue executing subsequent code.

            ===
            Task task = Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);
                Console.WriteLine("Foo");
            });
            Console.WriteLine(task.IsCompleted);
            task.Wait();

            Console.WriteLine("all tasks are completed.");
            ===

            In this case, the main thread starts a task that sleeps for 2 seconds before printing "Foo". 
            The Console.WriteLine(task.IsCompleted) will output False because the task is still running at that moment. 
            After calling task.Wait(), the main thread will block until the task finishes, which happens after 2 seconds.

            2. Timeout and Cancellation:
            The Wait method can also accept optional parameters for a timeout and a cancellation token.

            a. Timeout: 
            You can specify a time limit for how long to wait for the task to complete. 
            If the task doesn’t finish within this timeframe, Wait will return without blocking indefinitely.

            Task task = Task.Run(() =>
            {
                Thread.Sleep(2 * 1000);
                Console.WriteLine("Foo");
            });

            bool completed = task.Wait(1 * 1000);
            if (!completed)
            {
                Console.WriteLine("Task did not complete in the allocated time. [1 seconds]");
            }

            b. Cancellation

            Using a cancellation token, you can signal that you want to stop waiting for the task to finish. 
            This is especially useful in scenarios where you might want to cancel long-running tasks or 
            gracefully handle user interruptions.

            simple cancellation example:

            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;

            Task task = Task.Run(() =>
            {
                Console.WriteLine("Task started. Press 'c' to cancel...");

                for (int i = 0; i < 10; i++)
                {
                    if (token.IsCancellationRequested)
                    {
                        Console.WriteLine("Task was cancelled!");
                        return;
                    }

                    Console.WriteLine("Working...{0}", i);
                    Thread.Sleep(500);
                }

            }, token);

            if (Console.ReadKey().KeyChar == 'c')
            {
                tokenSource.Cancel();
                Console.WriteLine();
            }

            task.Wait();
            Console.WriteLine("Main program ending.");

            */

            /* Long-running Tasks
             
            By default, the CLR runs tasks on pooled threads, which is ideal for short-running compute-bound work. 
            For longer-running and blocking operations, you can prevent use of a pooled thread as follows.

            Task task = Task.Factory.StartNew(() => { }, TaskCreationOptions.LongRunning);

            -----Why This Matters for Performance:

            The thread pool is optimized for short tasks, designed to quickly pick up new tasks after each one finishes. 
            When a long-running task monopolizes a pooled thread, 
            it limits the pool's capacity to handle new tasks efficiently. 
            
            If many tasks are long-running and occupy pooled threads, 
            it can lead to thread starvation—where other tasks have to wait unnecessarily.

            Creating a dedicated thread with LongRunning avoids this, 
            as it doesn’t rely on the limited pool of threads shared with other tasks.

            Task task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Starting long-running task...");
                Thread.Sleep(5 * 1000);
                Console.WriteLine("Long-running task completed.");
            }, TaskCreationOptions.LongRunning);

            task.Wait();
            Console.WriteLine("Main program ends.");
            
            */

            /* Returning Values
            
            The Task<TResult> class in .NET allows a task to produce a result value when it completes. 
            This makes it possible to run code concurrently, 
            then retrieve a calculated result without relying on shared variables. 
            Here’s how it works:

            -----Creating a Task with a Return Value

            To create a task that returns a result, use Task.Run with 
            1. a Func<TResult> delegate 
            2. (or lambda expression) that returns a value instead of an Action. 

            Task<int> task = Task.Run(() => 
            {
                Console.WriteLine("Calculating...");
                return 3;  // The task's result
            });

            -----Accessing the Task Result

            To access the result of a Task<TResult>, use the Result property. 
            If the task has completed, Result will immediately provide the value; 
            if the task is still running, Result blocks the current thread until the task finishes.

            int result = task.Result; // Blocks if the task is not yet complete
            Console.WriteLine(result); // Outputs 3 once available

            -----Why Use await Instead of .Result

            1. Non-Blocking Execution: 
            When you use await, it allows the main thread to continue running other code while 
            waiting for the task to complete. 
            
            This keeps the UI responsive in a WPF or WinForms application and
            prevents blocking in a web application.

            2. Avoiding Deadlocks:
            In some cases, accessing .Result on a Task that runs on the same context (like in UI threads) 
            can cause a deadlock, where both the task and the thread wait indefinitely on each other. 
            
            Using await prevents this by asynchronously freeing up the context for other operations.

            3. Better Error Handling:
            If a task throws an exception, using .Result will wrap it in an AggregateException, 
            which may need extra handling to access the original error. 
            
            Using await provides a more straightforward exception handling, 
            as exceptions bubble up naturally without wrapping.

            Task<int> backgroundTask = Task.Run(async () =>
            {
                Console.WriteLine("Background Task: Starting process...");
                await Task.Delay(5 * 1000);
                Console.WriteLine("Background Task: Process completed.");
                return 42;
            });

            Console.WriteLine("Main Thread: Task started, doing other work...");

            // Using .Result (Blocking the main thread)
            int resultBlocking = backgroundTask.Result;  // This blocks until the task finishes
            Console.WriteLine($"Main Thread: Blocking result is {resultBlocking}");

            // Example of non-blocking with await
            Console.WriteLine("Main Thread: Starting async process...");
            int resultAsync = await backgroundTask;  // Non-blocking; allows the main thread to continue
            Console.WriteLine($"Main Thread: Async result is {resultAsync}");

            */

            /* Exceptions
             
            Unlike with threads, tasks conveniently propagate exceptions.
            If a task throws an exception, that exception does not terminate the application. 
            Instead, it is stored and can be accessed later. 
            
            When you attempt to access the task's result or wait for its completion using methods like 
            Wait() or accessing the Result property, the exception is rethrown.

            When a task faults, the exception is wrapped in an AggregateException. 
            This allows for the possibility of multiple exceptions being thrown 
            when dealing with parallel tasks.

            Task task = Task.Run(() =>
            {
                throw new DivideByZeroException("you cannot divide a number to zero!!!");
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException aex)
            {
                foreach (var innerEx in aex.InnerExceptions)
                {
                    if (innerEx is DivideByZeroException divideByZeroException)
                    {
                        Console.WriteLine($"divide exception: {divideByZeroException.Message}");
                    }
                    else
                    {
                        Console.WriteLine("Caught an exception: " + innerEx.Message);
                    }
                }
            }

            Tasks provide properties such as IsFaulted and IsCanceled to check 
            the state of the task without having to throw exceptions.

            Console.WriteLine($"task is faulted: {task.IsFaulted}");

            */

            /* Continuations
             
            Continuations in .NET allow you to schedule an additional action or task to execute automatically 
            once an existing task completes. 
            This feature is useful for handling complex workflows 
            where multiple asynchronous operations must occur sequentially or in response to each other.

            When a task completes (whether successfully, with an exception, or due to cancellation), 
            you can specify code to execute right afterward. 
            There are two main ways to attach a continuation:

            1. Using GetAwaiter().OnCompleted()
            2. Using ContinueWith

            -----1) Attaching Continuations with GetAwaiter().OnCompleted()

            When you call GetAwaiter() on a task, you retrieve an awaiter object. 
            An awaiter represents an object that can "wait" for the task to complete and 
            can provide functionality to execute a continuation once the task finishes.

            Task<int> primeNumbersTask = Task.Factory.StartNew(() =>
            {
                return Enumerable.Range(2, 3_000_000).Count(m =>
                {
                    return Enumerable.Range(2, (int)Math.Sqrt(m) - 1).All(i => m % i > 0);
                });
            }, TaskCreationOptions.LongRunning);

            Console.WriteLine("calculating all the prime numbers withing [1-3million]");

            TaskAwaiter<int> awaiter = primeNumbersTask.GetAwaiter();
            awaiter.OnCompleted(() =>
            {
                try
                {
                    int result = awaiter.GetResult();
                    Console.WriteLine(result);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            });

            -----Threading and Synchronization Context with ConfigureAwait(false)

            When you create and start a task, it runs on a separate thread from the UI, 
            typically from the thread pool. 
            
            Once the task completes, any code written as a continuation will try to run 
            on the original thread if a synchronization context is available.

            In applications with a UI (like WPF or WinForms), 
            there is a synchronization context associated with the UI thread. 
            
            This synchronization context ensures that updates to the UI only happen on the UI thread, 
            which is necessary to keep the UI responsive and to avoid conflicts.

            When you await a task in a UI application, by default, 
            .NET captures synchronization context.

            ---What ConfigureAwait(false) Does?

            If you call ConfigureAwait(false) on a task, 
            you are instructing it not to capture the synchronization context. 
            
            Instead, the continuation can execute on any thread available, 
            typically on a thread pool thread, rather than the original context (like the UI thread).

            await Task.Run(() => { long-running work }).ConfigureAwait(false);

            By adding ConfigureAwait(false), you are essentially telling the program, 
            "I don’t need this continuation to run on the original thread (UI thread). 
            Run it wherever it’s convenient."
            
            ---Why This Matters
             
            1. With ConfigureAwait(true) (default): 
            The continuation will try to return to the UI thread if possible, 
            ensuring you’re able to interact with UI components directly in the continuation.

            2. With ConfigureAwait(false): 
            The continuation may run on a different thread (a background or thread pool thread). 
            This improves performance because the runtime doesn’t have to wait to access the UI thread, 
            which could be busy. However, 
            
            if you need to access UI elements afterward, you’ll need to switch back to the UI thread.

            -----2) Attaching Continuations with ContinueWith

            a. ContinueWith Syntax
            The ContinueWith method is another way to add a continuation to a task.
            It offers a simpler syntax and is useful in parallel programming and 
            scenarios where more control over task chaining is needed.

            Task<int> primeNumbersTask = Task.Factory.StartNew(() =>
            {
                return Enumerable.Range(2, 3_000_000).Count(m =>
                {
                    return Enumerable.Range(2, (int)Math.Sqrt(m) - 1).All(i => m % i > 0);
                });
            }, TaskCreationOptions.LongRunning);

            Console.WriteLine("calculating all the prime numbers withing [1-3million]");

            primeNumbersTask.ContinueWith(antecedent =>
            {
                if (antecedent.IsFaulted)
                {
                    Console.WriteLine("Task failed with an exception.");
                }
                else
                {
                    Console.WriteLine($"Number of Prime numbers: {antecedent.Result}");
                }
            });

            b. Using TaskContinuationOptions for Thread Control

            With ContinueWith, the task typically runs on the thread pool, 
            but you can specify TaskContinuationOptions.ExecuteSynchronously to attempt 
            synchronous execution on the same thread, reducing thread-switching overhead.

            primeNumberTask.ContinueWith(antecedent =>
            {
                Console.WriteLine($"Result: {antecedent.Result}");
            }, TaskContinuationOptions.ExecuteSynchronously);
            
            Alternatively, you can control the behavior further using options like 
            1. TaskContinuationOptions.OnlyOnFaulted or 
            2. TaskContinuationOptions.OnlyOnRanToCompletion to execute specific actions 
            based on the task's outcome.


            By default, Task.Run uses the thread pool, which is a set of background threads managed by .NET 
            to handle tasks without creating new threads unnecessarily.
            
            These threads are background threads by nature, 
            meaning they don’t keep the application alive if the main thread exits.

            In applications with a UI thread (like WinForms or WPF), 
            there’s a synchronization context in place that manages updates and interactions on that thread.
            Continuation tasks can automatically use this context, 
            ensuring that code following a task executes back on the UI thread.

            //Key Points for Each Approach
            
            1. GetAwaiter().OnCompleted():

            a) Automatically captures and returns to the original context (like the UI thread), 
            unless ConfigureAwait(false) is specified.
            b) Directly rethrows exceptions via GetResult(), providing clean exception handling.

            2. ContinueWith:

            a) More control over task chaining and continuation scheduling (through TaskContinuationOptions).
            b) Continuations return another Task, 
            enabling more chaining but requiring manual exception handling (AggregateException).

            */

            #region ContinueWith
            //Task<int> primeNumbersTask = Task.Factory.StartNew(() =>
            //{
            //    return Enumerable.Range(2, 3_000_000).Count(m =>
            //    {
            //        return Enumerable.Range(2, (int)Math.Sqrt(m) - 1).All(i => m % i > 0);
            //    });
            //}, TaskCreationOptions.LongRunning);

            //Console.WriteLine("Calculating all the prime numbers withing [1-3million]");

            //primeNumbersTask.ContinueWith(antecedent =>
            //{
            //    if (antecedent.IsFaulted)
            //    {
            //        Console.WriteLine("Task failed with an exception.");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Number of Prime numbers: {antecedent.Result}");
            //    }
            //}, TaskContinuationOptions.ExecuteSynchronously);
            //Console.ReadLine();
            #endregion

            #region GetAwaiter
            //Task<int> primeNumbersTask = Task.Factory.StartNew(() =>
            //{
            //    return Enumerable.Range(2, 3_000_000).Count(m =>
            //    {
            //        return Enumerable.Range(2, (int)Math.Sqrt(m) - 1).All(i => m % i > 0);
            //    });
            //}, TaskCreationOptions.LongRunning);

            //Console.WriteLine("calculating all the prime numbers withing [1-3million]");

            //TaskAwaiter<int> awaiter = primeNumbersTask.GetAwaiter();
            //awaiter.OnCompleted(() =>
            //{
            //    try
            //    {
            //        int result = awaiter.GetResult();
            //        Console.WriteLine(result);
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"An error occurred: {ex.Message}");
            //    }
            //});

            //Console.ReadLine();
            #endregion

            /* Difference between Task.Factory.StartNew and Task.Run
               especially when it comes to handling asynchronous code.
             
            1. Task.Run
            Task.Run was introduced with async-await in mind. 
            When you pass an async lambda (like async () => {...}) to Task.Run, 
            it properly recognizes it as an asynchronous method and 
            knows to wait for the whole asynchronous operation to complete.

            await Task.Run(async () =>
            {
                Console.WriteLine("Task with Task.Run started...");
                await Task.Delay(2000); // Simulate async work
                Console.WriteLine("Task with Task.Run completed.");
            });

            Here, the await will wait for the full 2-second delay, meaning "Task with Task.Run completed." will display after the delay.

            2. Task.Factory.StartNew
            Task.Factory.StartNew is a lower-level method intended primarily for creating tasks with more customization options. 
            It was originally created in .NET Framework 4.0, before Task.Run was introduced, and 
            is ideal for scenarios requiring more control over task execution options (like task scheduling, creation options, etc.).

            Task.Factory.StartNew does not handle async lambdas intuitively. 
            When you pass an async lambda to Task.Factory.StartNew,  it doesn’t know to wait for the entire asynchronous operation. 
            Instead, it sees the lambda as a regular delegate, which will complete as soon as 
            it encounters the first await keyword within that lambda.

            await Task.Factory.StartNew(async () =>
            {
                Console.WriteLine("Task with Task.Factory.StartNew started...");
                await Task.Delay(2000); // Simulate async work
                Console.WriteLine("Task with Task.Factory.StartNew completed.");
            }, TaskCreationOptions.LongRunning);

            In this code, Task.Factory.StartNew won’t wait for await Task.Delay(2000). 

            */

            /* TaskCompletionSource
             
            In .NET, TaskCompletionSource is a tool for creating and controlling Task objects manually.
            
            With Task.Run, you start a task that runs a piece of code, 
            and it automatically completes when that code finishes. But sometimes, 
            you want more control over how a task completes. 
            
            For example, you might want the task to represent something that doesn't happen immediately, 
            like waiting for user input, a file to download, or data from a network. 
            In these cases, TaskCompletionSource is very useful because it lets you:

            1. Start the task.
            2. Manually complete, cancel, or fail the task at the right time.

            -----Key Idea of TaskCompletionSource

            Think of TaskCompletionSource as a "promise" of a task. 
            Instead of running code immediately, it creates a task that you can control. 
            You decide when that task finishes and with what result. 

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            _ = Task.Run(async () =>
            {
                Console.WriteLine("Task starting...");
                await Task.Delay(5 * 1000); // simulate 5 seconds delay
                tcs.SetResult(1);
                Console.WriteLine("Task completed!");
            });

            Console.WriteLine("Waiting for task to complete");

            var result = await tcs.Task;
            Console.WriteLine($"Result from background thread: {result}");

            Console.ReadLine();

            -----What if Something Goes Wrong?

            If the background work fails for some reason, 
            we can signal that by calling SetException instead of SetResult.

            TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            _ = Task.Factory.StartNew(() =>
            {
                try
                {
                    Console.WriteLine("Background task: Starting work...");
                    Thread.Sleep(2 * 1000);

                    throw new InvalidOperationException("Something went wrong!");

                    //tcs.SetResult(42);
                    //Console.WriteLine("Background task: Work completed successfully.");
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                    Console.WriteLine("Background task: Exception occurred.");
                }
            });

            Console.WriteLine("Main thread: Waiting for background task to complete...");

            try
            {
                int result = await tcs.Task;
                Console.WriteLine($"Main thread: Result from background task is {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Main thread: Caught exception - {ex.Message}");
            }

            -----
            To use TaskCompletionSource, you simply instantiate the class. 
            
            It exposes a Task property that returns a task upon which you can wait and attach continuations 
            just as with any other task. 
            
            The task, however, is controlled entirely by the TaskCompletionSource object via the following methods:
            
            public class TaskCompletionSource<TResult>
            {
                public void SetResult (TResult result);
                public void SetException (Exception exception);
                public void SetCanceled();
                public bool TrySetResult (TResult result);
                public bool TrySetException (Exception exception);
                public bool TrySetCanceled();
                public bool TrySetCanceled (CancellationToken cancellationToken);
                ...
            }

            Calling any of these methods signals the task, putting it into a completed, faulted, or canceled state.

            */

            /* Codes
            

            //TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();

            //_ = Task.Factory.StartNew(() =>
            //{
            //    try
            //    {
            //        Console.WriteLine("Background task: Starting work...");
            //        Thread.Sleep(2 * 1000);

            //        throw new InvalidOperationException("Something went wrong!");

            //        //tcs.SetResult(42);
            //        //Console.WriteLine("Background task: Work completed successfully.");
            //    }
            //    catch (Exception ex)
            //    {
            //        tcs.SetException(ex);
            //        Console.WriteLine("Background task: Exception occurred.");
            //    }
            //});

            //Console.WriteLine("Main thread: Waiting for background task to complete...");

            //try
            //{
            //    int result = await tcs.Task;
            //    Console.WriteLine($"Main thread: Result from background task is {result}");
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Main thread: Caught exception - {ex.Message}");
            //}

            //Console.Read();

            #region code example
            //Task<int> primeNumbersTask = Task.Factory.StartNew(() =>
            //{
            //    return Enumerable.Range(2, 3_000_000).Count(m =>
            //    {
            //        return Enumerable.Range(2, (int)Math.Sqrt(m) - 1).All(i => m % i > 0);
            //    });
            //}, TaskCreationOptions.LongRunning);

            //bool isCompleted;
            //do
            //{
            //    isCompleted = primeNumbersTask.Wait(TimeSpan.FromSeconds(1));
            //}
            //while (!isCompleted);

            //int primeCounts = await primeNumbersTask;
            //Console.WriteLine("number of primes from 1 to 3_000_000 : {0}", primeCounts);
            #endregion
            */

            // complete previous subjects, until page 645]

            //========================================================================================
            // Asynchronous Patterns

            /* Cancellation
             
            When running tasks concurrently, it's common to want to stop a task in response to 
            1. a user action, 2. timeout, or 3. other condition. 
            
            Instead of forcibly stopping a thread, .NET provides a pattern with CancellationToken and CancellationTokenSource 
            to allow tasks to cooperatively stop when requested.

            a) CancellationTokenSource: 
            This class is used to signal a cancellation. It has methods to initiate cancellation, like Cancel and CancelAfter.

            b) CancellationToken: 
            This struct represents a token that’s passed to tasks or methods, 
            allowing them to periodically check for cancellation requests and stop in a controlled way.

            -To get a cancellation token, we first instantiate a CancellationTokenSource:
            var cancelSource = new CancellationTokenSource();

            This exposes a Token property, which returns a CancellationToken:
            var token = cancelSource.Token;

            c) OperationCanceledException: 
            When cancellation is requested, tasks often throw this exception to indicate they’ve been cancelled. 
            Awaiting code will see this exception and handle it.

            
            -----Real-World Example: File Processing with Cancellation Support

            Let’s say we have a system where multiple files are being processed, 
            and we want to allow cancellation at any point, such as if a user cancels the operation or 
            if the processing exceeds a certain time limit.

            public class FileProcessor
            {
                public async Task ProcessFilesAsync(IEnumerable<string> filePaths, CancellationToken cancellationToken)
                {
                    Console.WriteLine("Beginning file processing...");
        
                    foreach (var filePath in filePaths)
                    {
                        try
                        {
                            await ProcessFileAsync(filePath, cancellationToken);
                        }
                        catch (OperationCanceledException)
                        {
                            Console.WriteLine($"\nProcessing cancelled for {filePath}.");
                            break;
                        }
                    }
        
                    Console.WriteLine("File processing operation completed.");
                }
        
                private async Task ProcessFileAsync(string filePath, CancellationToken cancellationToken)
                {
                    Console.WriteLine($"Starting processing of {filePath}");
        
                    for (int i = 0; i < 5; i++)
                    {
                        cancellationToken.ThrowIfCancellationRequested();
        
                        Console.WriteLine($"Processing stage {i} for {filePath}...");
                        await Task.Delay(500, cancellationToken); // each stage takes half second
                    }
        
                    Console.WriteLine($"Completed processing of {filePath}.");
                }
            }

            IEnumerable<string> filePaths = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            FileProcessor fileProcessor = new FileProcessor();

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(10 * 1000); // autmoatically cancel after 10 seconds.

            CancellationToken cancellationToken = cancellationTokenSource.Token;

            Task processingTask = fileProcessor.ProcessFilesAsync(filePaths, cancellationTokenSource.Token);

            // Allow user to manually cancel
            Console.WriteLine("Press 'c' to cancel within 10 seconds.");
            _ = Task.Run(() =>
            {
                if (Console.ReadKey().Key == ConsoleKey.C)
                {
                    cancellationTokenSource.Cancel();
                }
            });

            try
            {
                await processingTask;
            }
            catch (Exception exception)
            {
                if (exception is OperationCanceledException)
                {
                    Console.WriteLine("File processing was cancelled.");
                }
                else
                {
                    Console.WriteLine("Exception: {0}", exception.Message);
                }
            }

            Console.Read();


            //How does it work?

            ProcessFileAsync is an async method that processes a file in 5 stages.
            Each stage checks if cancellation has been requested via token.ThrowIfCancellationRequested().
            If the cancellation token’s IsCancellationRequested property is true, 
            ThrowIfCancellationRequested throws an OperationCanceledException, stopping the task.

            We create a CancellationTokenSource in Main with CancelAfter(10_000), 
            which will automatically request cancellation after 10 seconds if the operation isn’t complete.
            
            Simultaneously, we start a task listening for user input (c) to cancel manually.
            If cancellation is requested, either by user input or timeout, 
            the program stops processing further files and prints a cancellation message.

            ---Advanced: Using CancellationToken.Register for Notifications

            If you want to execute a specific action as soon as a cancellation request is made 
            (e.g., logging or cleaning up resources), you can use the CancellationToken.Register method.

            CancellationToken token = cancellationTokenSource.Token;

            // Register a callback to execute when cancellation is requested
            token.Register(() => Console.WriteLine("Cancellation has been requested."));

            */

            /* Progress Reporting
             
            When you’re running a long asynchronous task, you might want to report progress to the user, 
            especially in UI applications. 
            
            Progress reporting allows the task to periodically notify the calling code of its current status 
            without blocking or waiting for the task to complete. 
            
            The example shows two approaches to implementing progress reporting: 
            1. one using an Action delegate and 
            2. the other using IProgress<T>. 
            
            Here’s a detailed explanation with examples of each approach and why IProgress<T> is often better in UI scenarios.

            Approach 1: Using an Action Delegate

            Action<int> progress = (int i) =>
            {
                Console.WriteLine($"Progress: {i}%");
            };
            await Foo(progress);

            static Task Foo(Action<int> onProgressPercentChanged)
            {
                return Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (i % 10 == 0)
                        {
                            onProgressPercentChanged(i / 10);
                        }
                    }
                });
            }

            ---Limitation in UI Applications:

            1. Since Task.Run runs on a background thread, 
            reporting progress directly through an Action might cause issues in a UI application.
            
            2. Directly updating the UI from a background thread can lead to threading issues because 
            UI elements can only be safely accessed from the main UI thread.


            Approach 2: Using IProgress<T> and Progress<T>

            The CLR provides a pair of types to solve this problem: 
            an interface called IProgress<T> and a class that implements this interface called Progress<T>.

            Their purpose is to “wrap” a delegate so that 
            UI applications can report progress safely through the synchronization context.

            public interface IProgress<in T>
            {
                void Report (T value);
            }

            IProgress<int> progress = new Progress<int>((int progressPercentage) => Console.WriteLine($"{progressPercentage}%"));
            await Foo(progress);

            static Task Foo(IProgress<int> progress)
            {
                return Task.Run(() =>
                {
                    for (int i = 0; i < 1000; i++)
                    {
                        if (i % 10 == 0)
                        {
                            progress.Report(i / 10);
                        }
                    }
                });
            }

            The issue you’re seeing, where progress percentages are printed out of order, 
            is likely due to the fact that Console.WriteLine is not thread-safe and 
            can produce output in a non-sequential order when multiple writes happen quickly. 
            
            Each call to progress.Report is queued for execution in the captured synchronization context 
            (typically the main UI thread or a context managed by Progress<T>). 
            This context then processes each Report call, 
            but rapid reporting can lead to unpredictable output in the console.

            In a UI application, this wouldn’t be a problem because Progress<T> ensures that 
            each update is processed sequentially on the main UI thread


            -----Why IProgress<T> is Better in UI Applications

            In rich-client applications (like Windows Forms, WPF, or UWP), 
            updates to UI elements need to happen on the main thread. 
            
            The Progress<T> class ensures that progress reporting is synchronized with the main UI thread, 
            preventing potential crashes and issues.

            -----Real-World Example: File Download with Progress Reporting

            public class FileInstaller
            {
                public async Task InstallFileAsync(string url, string destinationPath, IProgress<int> progress, CancellationToken cancellationToken)
                {
                    Console.WriteLine($"Installing file from [{url}] to [{destinationPath}]");
                    int bytesDownloaded = 0;
                    int totalBytes = 1000;

                    while (bytesDownloaded < totalBytes)
                    {
                        cancellationToken.ThrowIfCancellationRequested();

                        bytesDownloaded += 100;
                        await Task.Delay(500); // each 100 bytes take half second to install for example

                        int percentComplete = (bytesDownloaded * 100) / totalBytes;
                        progress.Report(percentComplete);
                    }

                    // simulate saving the file
                    await Task.Delay(500);
                }
            }

            ---

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(20 * 1000); // cancel installing manually after 20 seconds

            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationToken.Register(() => Console.WriteLine("Installation was cancelled by user."));

            try
            {
                string url = "http://example.com/file";

                FileInstaller fileInstaller = new FileInstaller();
                IProgress<int> progress = new Progress<int>((int percentage) =>
                {
                    Console.WriteLine($"\nTotal installation percentage: {percentage}/100%");
                });

                Console.WriteLine("Press 'c' to cancel installation within 10 seconds.");
                _ = Task.Run(() =>
                {
                    if (Console.ReadKey().Key == ConsoleKey.C)
                    {
                        cancellationTokenSource.Cancel();
                    }
                });

                await fileInstaller.InstallFileAsync(url, "file.txt", progress, cancellationToken);
                Console.WriteLine("\nInstall completed!");
            }
            catch (OperationCanceledException)
            {

            }

            */

            using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfter(20 * 1000); // cancel installing manually after 20 seconds

            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationToken.Register(() => Console.WriteLine("Installation was cancelled by user."));

            try
            {
                string url = "http://example.com/file";

                FileInstaller fileInstaller = new FileInstaller();
                IProgress<int> progress = new Progress<int>((int percentage) =>
                {
                    Console.WriteLine($"\nTotal installation percentage: {percentage}/100%");
                });

                Console.WriteLine("Press 'c' to cancel installation within 10 seconds.");
                _ = Task.Run(() =>
                {
                    if (Console.ReadKey().Key == ConsoleKey.C)
                    {
                        cancellationTokenSource.Cancel();
                    }
                });

                await fileInstaller.InstallFileAsync(url, "file.txt", progress, cancellationToken);
                Console.WriteLine("\nInstall completed!");
            }
            catch (OperationCanceledException)
            {

            }

            //IProgress<int> progress = new Progress<int>((int progressPercentage) => Console.WriteLine($"{progressPercentage}%"));
            //await Foo(progress);

            Console.ReadLine();

            //Action<int> progress = (int i) =>
            //{
            //    Console.WriteLine($"Progress: {i}%");
            //};
            //await Foo(progress);

            #region file processor
            //IEnumerable<string> filePaths = Directory.EnumerateFiles(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments));
            //FileProcessor fileProcessor = new FileProcessor();

            //using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            //cancellationTokenSource.CancelAfter(10 * 1000); // autmoatically cancel after 10 seconds.

            //CancellationToken cancellationToken = cancellationTokenSource.Token;

            //Task processingTask = fileProcessor.ProcessFilesAsync(filePaths, cancellationTokenSource.Token);

            //// Allow user to manually cancel
            //Console.WriteLine("Press 'c' to cancel within 10 seconds.");
            //_ = Task.Run(() =>
            //{
            //    if (Console.ReadKey().Key == ConsoleKey.C)
            //    {
            //        cancellationTokenSource.Cancel();
            //    }
            //});

            //try
            //{
            //    await processingTask;
            //}
            //catch (Exception exception)
            //{
            //    if (exception is OperationCanceledException)
            //    {
            //        Console.WriteLine("File processing was cancelled.");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Exception: {0}", exception.Message);
            //    }
            //}

            //Console.Read();
            #endregion
        }

        static Task Foo(IProgress<int> progress)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (i % 10 == 0)
                    {
                        progress.Report(i / 10);
                    }
                }
            });
        }

        static Task Foo(Action<int> onProgressPercentChanged)
        {
            return Task.Run(() =>
            {
                for (int i = 0; i < 1000; i++)
                {
                    if (i % 10 == 0)
                    {
                        onProgressPercentChanged(i / 10);
                    }
                }
            });
        }
    }
}