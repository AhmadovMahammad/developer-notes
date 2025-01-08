using System;
using System.Collections.Concurrent;

namespace ParallelProgramming_ch22;
internal class Program
{
    private static void Main(string[] args)
    {
        /* Introduction into PFX?
        
        The PFX (Parallel Framework Extensions) libraries in C# are designed to enable parallel programming, 
        which is essential in taking advantage of modern multicore processors. 
        
        Over the last decade and a half, CPU manufacturers have shifted from single-core processors to multicore processors, 
        dramatically increasing the number of threads that can execute in parallel. 
        
        However, this shift poses challenges for software developers, especially those working with traditional single-threaded applications. 
        The core issue is that simply running code on a multicore machine does not automatically make it faster 
        unless that code is explicitly written to leverage the additional cores. 
        This is where parallel programming and frameworks like PFX come into play. 


        --- The Problem with Single-Threaded Code on Multicore Processors

        With single-core processors, the performance of software applications is limited by the processing power of that single core. 
        But with multiple cores, the processor can theoretically handle multiple tasks at once. 
        The challenge arises in how to partition your existing single-threaded code to leverage these additional cores.

        For server applications, this is often easier to handle because 
        tasks can be parallelized by assigning each core a separate request. 

        However, desktop applications tend to have more complex computational tasks that 
        cannot always be split easily into independent units. 
        A typical computationally intensive task might require:

        1. Partitioning the task into smaller chunks that can be processed concurrently.
        2. Executing these chunks in parallel by using multithreading.
        3. Collecting the results from the different threads in a manner that ensures thread safety and high performance.

        */

        /* Multithreading vs Parallel Programming
         
        The classic multithreading model is often used for handling concurrency. 
        However, this approach is cumbersome when it comes to partitioning the work into smaller pieces 
        and handling the results in a thread-safe manner. 

        Furthermore, traditional multithreading usually involves locks to ensure thread safety, 
        and these locks can introduce significant performance bottlenecks due to 
        contention when multiple threads access shared data simultaneously.

        In contrast, parallel programming is a specific subset of multithreading focused on 
        leveraging multiple processors (or cores) to perform computations faster.

        While multithreading can handle both parallel and concurrent tasks, 
        parallel programming focuses specifically on splitting tasks to take advantage of multicore processors.


        --- Partitioning Work: Data Parallelism vs Task Parallelism
        There are two major approaches to partitioning work for parallel execution: 
        data parallelism and task parallelism. Each of these approaches has its strengths and weaknesses.

        --- 1. Data Parallelism
        Data parallelism is one of the more common strategies in parallel programming, 
        especially for applications that involve processing large datasets. 

        The fundamental idea is to partition the data into smaller subsets, with each subset processed by a different thread. 
        Each thread performs the same set of tasks on its respective subset of data. 
        This means that the operations across all the threads are identical, but the data on which they operate is different.

        The main advantage of data parallelism is that it often scales much better to systems with multiple cores because:

        1. The data is divided between threads, reducing contention for shared data.
        2. Each thread works independently, leading to fewer synchronization issues compared to task parallelism.
        3. It works well for problems where the same operation must be performed on multiple data values 
           (such as array operations, image processing, and mathematical computations).

        *** Example: Consider a situation where you want to compute the square of each number in an array.

        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] squares = new int[numbers.Length];

        Parallel.For(0, numbers.Length, (int index) =>
        {
            squares[index] = numbers[index] * numbers[index];
        });

        Console.WriteLine($"Squares: {string.Join(',', squares)}");
        -----------------------------------------------------------

        In this example, each number is squared in parallel across different threads. 
        The Parallel.For method is part of PFX and 
        is designed to handle the task of partitioning the data and executing the work on multiple threads.


        2. Task Parallelism
        On the other hand, task parallelism involves partitioning the tasks rather than the data. 
        Here, each thread performs a different task, and those tasks may or may not operate on the same data. 

        Task parallelism can be more difficult to manage because it often involves coordinating between threads
        that may be doing entirely different work, potentially accessing the same data or requiring synchronization.

        Example: If you had multiple independent tasks, such as downloading files, processing data, and saving results, 
        task parallelism would let you run these tasks concurrently.

        Task task1 = Task.Run(() => DownloadFiles());
        Task task2 = Task.Run(() => ProcessData());
        Task task3 = Task.Run(() => SaveResults());
        
        await Task.WhenAll(task1, task2, task3);
        ----------------------------------------

        --- Structured vs Unstructured Parallelism.
        
        One significant difference between data parallelism and task parallelism is the structure of the work. 
        Data parallelism tends to be structured because the tasks that are parallelized typically start and finish at the same time, 
        and they often follow a regular, predictable pattern. 
        This makes it easier to implement because the framework can manage the partitioning, execution, and collation of results.

        In contrast, task parallelism tends to be unstructured, 
        meaning that the parallel tasks may start and finish at different times, with varying complexities. 
        Unstructured parallelism can be more flexible but also harder to manage correctly and efficiently.

        */

        /* Why PFX?

        The Parallel Framework Extensions (PFX) libraries are specifically designed to address the challenges of parallel programming. 
        PFX provides abstractions and tools to simplify the partitioning of work when dealing with data parallelism. 
        
        The Parallel class in PFX (such as Parallel.For, Parallel.ForEach, and Parallel.Invoke) is ideal for 
        handling structured parallelism in scenarios where data can be partitioned easily and tasks can be executed concurrently.

        --- 1. Parallel.For
        
        This method is used for iterative tasks where you process a range of numbers in parallel. 
        It splits the iterations across threads automatically.

        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] squares = new int[numbers.Length];

        Parallel.For(0, numbers.Length, (int index) =>
        {
            squares[index] = numbers[index] * numbers[index];
        });

        Console.WriteLine($"Squares: {string.Join(',', squares)}");

        Here, the Parallel.For method divides the range [0, numbers.Length) among multiple threads. 
        Each thread computes the square of the number at its assigned index.

        --- 2. Parallel.ForEach

        This method works for collections like arrays, lists, or any enumerable structure. 
        It processes each item in the collection in parallel.

        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> squares = new List<int>();
        object lockObj = new object();

        Parallel.ForEach(numbers, (int number) =>
        {
            lock (lockObj)
            {
                squares.Add(number * number);
            }
        });

        Console.WriteLine($"Squares: {string.Join(',', squares)}");

        The Parallel.ForEach method splits the collection among threads, 
        and each thread processes a subset of items. 
        Note the use of lock to handle thread-safe access to the shared results list.


        --- 3. Parallel.Invoke

        This method is used to execute independent tasks concurrently. 
        It is typically for scenarios where you have several methods or actions to run in parallel.

        int result_factorial = 0;
        int result_square = 0;
        int result_cube = 0;

        Parallel.Invoke(
            () => result_factorial = ComputeFactorial(3), // Task 1
            () => result_cube = ComputeCube(3),           // Task 2
            () => result_square = ComputeSquare(3)        // Task 3
        );

        The Parallel.Invoke method runs each provided action in parallel. 
        Since these tasks are independent, there's no need for synchronization.

        ------------------------------------------------------------------------

        In summary, PFX allows developers to write high-performance parallel code without needing to deal 
        directly with low-level thread management, locks, and synchronization mechanisms. 
        
        By focusing on data parallelism, PFX simplifies the task of leveraging multicore processors, 
        thus enabling applications to run faster and scale more effectively.

        */

        /* PFX Components Overview

        The Parallel Framework Extensions (PFX) in C# is structured into two primary layers, 
        each offering different levels of abstraction for parallel programming:

        Higher Layer:

            1. PLINQ (Parallel LINQ): A declarative approach to parallelism.
            2. Parallel Class: An imperative approach for structured parallelism.
        
        Lower Layer:
        
            1. Task Parallelism: Explicit control for low-level tasks.
            2. Concurrent Collections and Spinning Primitives: Tools for managing concurrency efficiently, minimizing locking and contention.

        */

        /* Higher Layer

        1. PLINQ (Parallel LINQ)
        PLINQ is designed to provide the most automated and feature-rich experience for parallel programming. 
        It builds upon LINQ (Language Integrated Query) and allows you to process collections in parallel 
        without worrying about partitioning, threading, or result collation.

        Key Features:

        1. Declarative: You specify "what" needs to be done, and the runtime determines "how" to execute it in parallel.
        2. Automates partitioning, task execution, and result collation.

        var numbers = Enumerable.Range(1, 10);
        var results = numbers
            .AsParallel() // enable parallelism.
            .Where(m => m % 2 == 0)
            .Select(m => Math.Pow(m, 2))
            .ToArray();

        Console.WriteLine(string.Join(',', results));
        // Output: 36,4,64,100,16

        In this example:
        Partitioning: PLINQ automatically divides the numbers sequence into chunks.


        2. Parallel Class
        The Parallel class provides methods like Parallel.For, Parallel.ForEach, and Parallel.Invoke. 
        While it automates partitioning, it leaves the responsibility of collating results to the developer. 
        This approach is less abstracted compared to PLINQ and is suitable when you need finer control over how tasks are executed.

        int[] numbers = { 1, 2, 3, 4, 5 };
        int total = 0;
        object lockObj = new object(); // For thread-safe access to total

        Parallel.For(0, numbers.Length, i =>
        {
            int square = numbers[i] * numbers[i];
            lock (lockObj)
            {
                total += square;
            }
        });

        Here, the Parallel.For method automatically partitions the iterations, 
        but the result (total) is collated manually using a lock to ensure thread safety.

        */

        /* Lower Layer
        
        1. Task Parallelism
        At the lowest level, task parallelism gives developers explicit control over partitioning and result collation. 
        This approach involves using the Task class and related constructs to create and manage threads. 
        It provides maximum flexibility but requires more effort. 

        Example: Summing the squares of numbers with tasks:
        int[] numbers = { 1, 2, 3, 4, 5 };
        int total = 0;
        object lockObject = new object();

        var tasks = numbers.Select(m => Task.Run(() =>
        {
            lock (lockObject)
            {
                total += m * m;
            }
        })).ToArray();

        Task.WaitAll(tasks);


        2. Concurrent Collections and Spinning Primitives
        These are advanced tools optimized for highly concurrent access. 
        Unlike traditional collections, such as List<T> or Dictionary<K, V>, concurrent collections are designed to handle 
        concurrent operations without significant contention.

        Examples include:

        * ConcurrentQueue<T>
        * ConcurrentStack<T>
        * ConcurrentBag<T>

        --- Why Do We Need Concurrent Collections?
        When working with a shared collection (e.g., a List<T> or Dictionary<K, V>) in a multithreaded environment, 
        multiple threads might try to read or modify the collection simultaneously. 
        This can lead to race conditions, data corruption, or even crashes because these collections are not inherently thread-safe.

        --- The Problem with List<T> and Locking
        Take a simple List<int> as an example. Imagine you have multiple threads adding elements to the list. 
        Without proper synchronization, two threads might access the internal structure of the list at the same time, 
        causing unexpected behavior:

        To avoid this, you’d typically use a lock to serialize access:
        
        List<int> list = new List<int>();
        object lockObj = new object();
        
        Parallel.For(0, 100, i =>
        {
            lock (lockObj)
            {
                list.Add(i);
            }
        });

        --- This works, but locking comes at a cost:
        1. It blocks other threads, causing delays.
        2. It doesn’t scale well on systems with many cores, as threads spend more time waiting for locks to be released.

        --- How Concurrent Collections Help
        Concurrent collections, like ConcurrentBag<T>, ConcurrentQueue<T>, or ConcurrentDictionary<K, V>
        are specifically designed to handle concurrent operations without requiring external locking. 
        They solve the problem in the following ways:

        1. Optimized for Multi-Threading.
        Internally, they use techniques like fine-grained locking (locking only parts of the collection) or 
        lock-free algorithms (atomic operations to avoid locking entirely).
        
        This reduces contention and allows multiple threads to access the collection simultaneously, 
        significantly improving performance.

        2. Thread-Safe by Design.
        These collections ensure that operations like Add, Remove, or TryGetValue are atomic. 
        You can use them without needing additional synchronization (e.g., lock statements).
        
        3. Reduced Blocking.
        Blocking (threads waiting for access) is minimized or eliminated, 
        making these collections more efficient in highly concurrent scenarios.

        --- Examples
        
        -----------------------------------
        * Using a ConcurrentBag

        var bag = new ConcurrentBag<int>();
        //var list = new List<int>();

        Parallel.For(0, 100, (int i) =>
        {
            bag.Add(i); // thread-safe addition
        });

        int total = 0;
        //total = list.Sum();
        while (bag.TryTake(out int num))
        {
            total += num;
        }

        Console.WriteLine($"Total: {total}");
        -------------------------------------

        In this example:

        1. Multiple threads can safely call Add and TryTake concurrently.
        2. Internally, the ConcurrentBag ensures thread-safe access without needing explicit locks.
        -------------------------------------


        -----------------------------------
        * Using a ConcurrentDictionary

        var dictionary = new ConcurrentDictionary<int, int>();
        Parallel.For(0, 100, i =>
        {
            dictionary.TryAdd(i, i * i);
        });

        foreach (var kvp in dictionary)
        {
            Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        }

        * You don’t need to worry about locks when adding, updating, or retrieving items.
        * The dictionary handles all concurrency issues internally.

        */

        /* Fine-Grained Locking: Locking Only parts of the Collection.
        
        --- What It Means
        Instead of locking the entire collection when performing operations like adding, removing, or updating an element, 
        fine-grained locking involves locking only specific "parts" of the collection. 
        This approach reduces contention by allowing multiple threads to work on different parts of the collection simultaneously.


        --- Example in a Dictionary
        Let’s take a traditional Dictionary<K, V> as an example:

        If you lock the entire dictionary when adding an element,
        all threads trying to add or access any key must wait, even if they are working on different keys.

        With fine-grained locking, the dictionary might divide its internal storage (buckets) into multiple segments. 
        Each segment has its own lock, so threads can operate on separate segments in parallel.


        --- How Concurrent Collections Use Fine-Grained Locking

        The ConcurrentDictionary<K, V> in .NET uses fine-grained locking by dividing its underlying storage 
        into multiple independent segments, based on the hash code of the keys. Here's how it works:

        1. Hash Partitioning:
            * When you add or retrieve a key-value pair, the dictionary determines the segment based on the hash of the key.
            * Each segment operates independently with its own lock, 
            * so threads modifying different segments can proceed without blocking each other.

        2. Thread Safety with Multiple Locks:
            * When thread A adds a key to segment 1, and thread B adds a key to segment 2, they can proceed in parallel.
            * However, if thread A and thread B both try to modify segment 1, one thread will block until the lock is released.


        */

        /* Lock-Free Algorithms
        

        */

        #region code examples
        //var dictionary = new ConcurrentDictionary<int, int>();
        //Parallel.For(0, 100, i =>
        //{
        //    dictionary.TryAdd(i, i * i);
        //});

        //foreach (var kvp in dictionary)
        //{
        //    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
        //}


        //var bag = new ConcurrentBag<int>();
        //Parallel.For(0, 100, (int i) =>
        //{
        //    bag.Add(i); // thread-safe addition
        //});

        //int total = 0;
        //while (bag.TryTake(out int num))
        //{
        //    total += num;
        //}

        //Console.WriteLine($"Total: {total}");


        //int[] numbers = { 1, 2, 3, 4, 5 };
        //int total = 0;
        //object lockObject = new object();

        //var tasks = numbers.Select(m => Task.Run(() =>
        //{
        //    lock (lockObject)
        //    {
        //        total += m * m;
        //    }
        //})).ToArray();

        //Task.WaitAll(tasks);


        //var numbers = Enumerable.Range(1, 10);
        //var results = numbers
        //    .AsParallel() // enable parallelism.
        //    .Where(m => m % 2 == 0)
        //    .Select(m => Math.Pow(m, 2))
        //    .ToArray();


        //Console.WriteLine(string.Join(',', results));
        // Output: 36,4,64,100,16

        //int[] numbers = { 1, 2, 3, 4, 5 };
        //int total = 0;
        //object lockObj = new object(); // For thread-safe access to total

        //Parallel.For(0, numbers.Length, i =>
        //{
        //    int square = numbers[i] * numbers[i];
        //    lock (lockObj)
        //    {
        //        total += square;
        //    }
        //});


        //int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //int[] squares = new int[numbers.Length];

        //Parallel.For(0, numbers.Length, (int index) =>
        //{
        //    squares[index] = numbers[index] * numbers[index];
        //});

        //Console.WriteLine($"Squares: {string.Join(',', squares)}");


        //int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        //List<int> squares = new List<int>();
        //object lockObj = new object();

        //Parallel.ForEach(numbers, (int number) =>
        //{
        //    lock (lockObj)
        //    {
        //        squares.Add(number * number);
        //    }
        //});

        //Console.WriteLine($"Squares: {string.Join(',', squares)}");


        //int result_factorial = 0;
        //int result_square = 0;
        //int result_cube = 0;

        //Parallel.Invoke(
        //    () => result_factorial = ComputeFactorial(3), // Task 1
        //    () => result_cube = ComputeCube(3),           // Task 2
        //    () => result_square = ComputeSquare(3)        // Task 3
        //);

        #endregion
    }

    static int ComputeFactorial(int n) => n <= 1 ? 1 : n * ComputeFactorial(n - 1);
    static int ComputeCube(int n) => n * n * n;
    static int ComputeSquare(int n) => n * n;
}