using System.Collections.Concurrent;
using System.Security.Cryptography;

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
        In lock-free algorithms, operations are designed to avoid locking entirely. 
        Instead of locking and blocking threads, they use atomic operations (hardware-level instructions) to manage shared resources.

        --- How It Works
        Lock-free algorithms rely on compare-and-swap (CAS) operations, which ensure atomicity without requiring locks. For example:

        * When adding an element, the algorithm checks the current state of the collection.
        * If the state hasn’t changed, it proceeds with the update.
        * If the state has changed (e.g., another thread modified the collection), the operation retries.

        -----
        In computer science, compare-and-swap (CAS) is an atomic instruction used in multithreading to achieve synchronization. 
        It compares the contents of a memory location with a given value and, 
        only if they are the same, modifies the contents of that memory location to a new given value. 

        This is done as a single atomic operation. 
        The atomicity guarantees that the new value is calculated based on up-to-date information; 
        if the value had been updated by another thread in the meantime, the write would fail. 
        The result of the operation must indicate whether it performed the substitution; 

        this can be done either with a simple boolean response (this variant is often called compare-and-set), 
        or by returning the value read from the memory location (not the value written to it), 
        thus "swapping" the read and written values. 

        --- Putting It Together: Fine-Grained Locking vs. Lock-Free Algorithms
        Feature	            Fine-Grained Locking                            Lock-Free Algorithms
        ---------------------------------------------------------------------------------------------------------------------------------
        Mechanism	        Locks specific parts of the collection.	        Avoids locks entirely, using atomic operations.
        Use Case	        Collections where some level of locking         Scenarios requiring maximum concurrency (e.g., ConcurrentBag).
                            is acceptable (e.g., ConcurrentDictionary).	
        Complexity	        Easier to implement compared                    More complex to implement, but highly efficient. 
                            to lock-free algorithms.	



        */

        /* PLINQ
        PLINQ (Parallel LINQ) is a specialized LINQ extension designed to simplify parallel execution of LINQ queries, 
        leveraging multicore processors for enhanced performance. 
        It automates two challenging aspects of parallel programming: 

        1. partitioning work across multiple threads and 
        2. collating results efficiently.
        
        --- PLINQ Basics and Execution
        PLINQ introduces parallelism by using the AsParallel() method on an enumerable sequence. 
        This converts the sequence into a ParallelQuery<T>, 
        enabling parallel execution of subsequent query operators. For instance:

        public class ParallelQuery : IEnumerable
        {
        
        }

        IEnumerable<int> numbers = Enumerable.Range(3, 1_000_000);
        var parallelQuery = numbers
            .AsParallel()
            .Where(num => Enumerable.Range(2, (int)Math.Sqrt(num)).All(nth => num % nth > 0));
        
        int[] primes = parallelQuery.ToArray();
        Console.WriteLine($"count: {primes.Length}");

        
        --- How It Works:

        1. Partitioning: The sequence is divided into chunks, with each chunk processed on a separate thread.
        2. Execution: Each thread executes the query logic (e.g., filtering, mapping) on its assigned partition.
        3. Collation: Results from all threads are merged into a single output sequence, ensuring order and correctness where needed.


        --- Key Concepts in PLINQ

        1. AsParallel() and Query Operators
        The AsParallel() method is the entry point into PLINQ. It transforms a standard LINQ query into a parallel query.
        After calling AsParallel(), LINQ operators (e.g., Where, Select, GroupBy) are replaced with their parallelized counterparts, 
        defined in the System.Linq.ParallelEnumerable namespace.

        2. AsSequential() for Sequential Execution
        If part of the query must run sequentially (e.g., due to thread-safety issues or side effects), 
        the AsSequential() method can revert the query back to sequential mode.

        var mixedQuery = numbers.AsParallel()
           .Where(n => n % 2 == 0)
           .AsSequential() // Revert to sequential processing
           .Select(n => SomeThreadUnsafeOperation(n));


        --- Query Composition
        1. Once a query is parallelized using AsParallel(), subsequent operators produce ParallelQuery<T> outputs, maintaining parallelism.
        2. Avoid reapplying AsParallel() within the same query, as it introduces unnecessary overhead (merging and repartitioning).

        For query operators that accept two input sequences (Join, GroupJoin, Concat, Union, Intersect, Except, and Zip), 
        you must apply AsParallel() to both input sequences (otherwise, an exception is thrown). 
        
        You don’t, however, need to keep applying AsParallel to a query as it progresses, 
        because PLINQ’s query operators output another ParallelQuery sequence. 
        In fact, calling AsParallel again introduces inefficiency in that it forces merging and repartitioning of the query:

        mySequence.AsParallel()     // Wraps sequence in ParallelQuery<int>
            .Where (n => n > 100)   // Outputs another ParallelQuery<int>
            .AsParallel()           // Unnecessary - and inefficient!
            .Select (n => n * n)

        Notes:
        PLINQ is only for local collections: it doesn’t work with Entity Framework, for instance, 
        because in those cases the LINQ translates into SQL, which then executes on a database server. 
        However, you can use PLINQ to perform additional local querying on the result sets obtained from database queries.

        */

        /* Parallel Execution Ballistics
        
        Parallel Execution Ballistics in PLINQ refers to how queries execute and manage their results when operating in parallel. 
        Although PLINQ maintains lazy evaluation like ordinary LINQ, 
        there are significant differences in execution behavior due to the nature of parallelism.

        --- Lazy Evaluation in PLINQ
        
        In both LINQ and PLINQ, queries are lazily evaluated. 
        This means that execution doesn't start immediately when you define the query. 
        Instead, it begins only when you consume the results—such as with a foreach loop, calling ToArray, 
        or accessing a terminal operator like First or Count.

        --- Sequential Query Behavior
        In an ordinary sequential LINQ query:

        1. Execution happens entirely in a "pull-based" manner.
        2. Each element is fetched and processed just in time, only when needed by the consumer.
        3. For example, if you iterate over a query with a foreach loop, 
           elements are processed one by one, sequentially, as the loop iterates.

        --- PLINQ Query Behavior
        In a PLINQ query:

        1. Execution is still lazy, but it proceeds in parallel.
        2. Multiple threads process the query ahead of time by fetching elements from the input sequence
           slightly before the consumer requests them. This is similar to a teleprompter showing text to a newsreader before it's spoken.

        3. The results of the processing are temporarily stored in a buffer so they are ready for the consumer when needed.


        --- Buffering in PLINQ
        The buffer acts as an intermediary storage:

        1. It holds processed results in small chunks so that the consumer can retrieve them 
           without waiting for parallel processing to finish completely.

        2. If the consumer pauses or stops consuming the results (e.g., by breaking out of a foreach loop early), 
           PLINQ halts further execution to avoid wasting CPU or memory.

        --- Tweaking Buffering Behavior
        You can control how PLINQ buffers results by using the WithMergeOptions method after AsParallel. 
        The merge options affect when and how results are made available to the consumer:

        1. AutoBuffered (Default):

            - A small buffer is maintained to store results before they are passed to the consumer.
            - This is a good balance between:
                Minimizing delays (consumer gets results quickly).
                Reducing memory and CPU overhead (processing isn’t done all at once unless needed).
        
        When to use it: Default behavior works for most scenarios, such as general data analysis.
        
        2. NotBuffered:

            - No buffering is used. Results are sent to the consumer immediately as they are processed, without waiting to fill a buffer.
            - This ensures the fastest possible visibility of results, but may slow down overall performance as threads work on-demand.
        
        When to use it: Real-time or streaming scenarios where you need immediate results (e.g., processing sensor data or live updates).
        
        3. FullyBuffered:

            - All parallel work is completed and stored in memory before any results are presented to the consumer.
            - This ensures all results are available at once, 
              which is necessary for operations that depend on the full dataset (e.g., sorting with OrderBy).
        
        When to use it: Scenarios that require the complete dataset to be processed together, such as sorting or grouping.

        
        --- Concrete Example of PLINQ’s Benefits

        1. Sequential LINQ Example
        var numbers = Enumerable.Range(1, 100000);
        var squares = numbers.Where(n => n % 2 == 0).Select(n => n * n).ToList();

        This query processes numbers one at a time, using only a single core.
        For a large dataset (e.g., 100,000 numbers), this can be slow because:
            - It doesn’t utilize all CPU cores.
            - Each element is fetched and processed only when required.

        2. PLINQ Example with Default Buffering
        var numbers = Enumerable.Range(1, 1000000);
        var squares = numbers.AsParallel()
            .Where(n => n % 2 == 0)
            .Select(n => n * n)
            .ToList();

        What happens here:

        - AsParallel() tells PLINQ to split the data into chunks (partitions) and process them on multiple threads.
        - Results are buffered as they are processed, so the consumer (ToList) doesn’t wait for all numbers to be processed sequentially.
        - The work is distributed across all CPU cores, maximizing performance.

        3. Using NotBuffered:
        Imagine you're analyzing live temperature readings.
        Using NotBuffered ensures that as soon as a temperature reading is processed, you can see it immediately—ideal for time-sensitive applications.

        var sensorData = Enumerable.Range(1, 1000000);
        var liveReadings = sensorData.AsParallel()
            .WithMergeOptions(ParallelMergeOptions.NotBuffered)
            .Where(temp => temp > 500_000)
            .ToList();

        4. Using FullyBuffered:
        Imagine you’re sorting a massive dataset of names alphabetically.
        The entire dataset must be processed before sorting is meaningful, so you use FullyBuffered.

        var sortedNames = names.AsParallel()
           .WithMergeOptions(ParallelMergeOptions.FullyBuffered)
           .OrderBy(name => name)
           .ToList();

        */

        /* PLINQ and Ordering

        A side effect of parallelizing the query operators is that when the results are collated, it’s not necessarily in the same order that 
        they were submitted. In other words, LINQ’s normal order-preservation guarantee for sequences no longer holds.

        If you need order preservation, you can force it by calling AsOrdered() after AsParallel():
        myCollection.AsParallel().AsOrdered()...

        Calling AsOrdered incurs a performance hit with large numbers of elements because PLINQ must keep track of each element’s original position.

        You can negate the effect of AsOrdered later in a query by calling AsUnordered: this introduces a “random shuffle point,” 
        which allows the query to execute more efficiently from that point on. 
        So, if you wanted to preserve input-sequence ordering for just the first two query operators, you’d do this:

        inputSequence.AsParallel().AsOrdered()
            .QueryOperator1()
            .QueryOperator2()
            .AsUnordered() // From here on, ordering doesn’t matter
            .QueryOperator3();

        AsOrdered is not the default because for most queries, the original input ordering doesn’t matter. 
        In other words, if AsOrdered were the default, you’d need to apply AsUnordered to the majority of your parallel queries to get the best performance,
        which would be burdensome.

        */

        /* Cancellation
        Canceling a PLINQ query whose results you’re consuming in a foreach loop is easy: 
        simply break out of the foreach, and the query will be automatically canceled as the enumerator is implicitly disposed. 

        For a query that terminates with a conversion, element, or aggregation operator, 
        you can cancel it from another thread via a cancellation token. To insert a token, 
        call WithCancellation after calling AsParallel, passing in the Token property of a CancellationTokenSource object.
        Another thread can then call Cancel on the token source, which throws an OperationCanceledException on the query’s consumer:
        
        IEnumerable<int> numbers = Enumerable.Range(3, 1000000);
        var cts = new CancellationTokenSource();

        var parallelQuery = numbers
           .AsParallel().WithCancellation(cts.Token)
           .Where(num => Enumerable.Range(2, (int)Math.Sqrt(num)).All(nth => num % nth > 0));

        try
        {
            foreach (int num in parallelQuery)
            {
                if (num / 1000 >= 1)
                {
                    cts.Cancel();
                }

                Console.WriteLine(num);
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("Operation cancelled by user.");
        }

        */

        /* The Parallel Class
        
        ***** Parallel.Invoke is a method provided to execute multiple independent tasks (or delegates) in parallel. 
        It simplifies parallel programming by handling task management, thread allocation, and load balancing for you. 
        Let’s dive deeper into its functionality, behavior, and practical use cases.

        --- Core Mechanism of Parallel.Invoke

        1. Execution Model:

        - It accepts an array of Action delegates, which represent the code to execute in parallel.
        - All the provided delegates are executed concurrently, utilizing multiple threads.
        - Once all the delegates have completed, the method returns.

        2. Thread and Task Management:

        - It doesn't create one thread or Task per delegate; instead, 
          it intelligently batches large numbers of actions into smaller groups and 
          distributes them among a limited number of threads or Task instances.
        
        - This avoids the overhead of creating too many threads or tasks when processing a massive array of delegates.

        IEnumerable<int> numbers = Enumerable.Range(1, 1_000_000);
        foreach (var item in numbers)
        {
            Task.Run(() =>
            {

            });
        }

        - What happens here?

            - For every iteration of the loop, a new Task is created using Task.Run.
            - Each Task potentially requires its own thread. 
              Since there are 1,000,000 iterations, this could mean creating up to 1,000,000 threads 
              (depending on the thread pool's capacity and task scheduling).
            - Each thread consumes memory for its stack and incurs significant overhead for context switching and scheduling.
            - The system can quickly run out of resources (threads and memory) and degrade performance or even crash.
        
        - Why is this inefficient?
        
            - Threads are expensive in terms of memory and processing.
            - Most tasks will spend time waiting for CPU cycles, leading to wasted resources.
            - Managing such a large number of threads introduces excessive overhead and inefficiencies.

        Parallel.ForEach(numbers, x =>
        {

        });

        - What happens here?

            - Parallel.ForEach does not create a separate thread for each item in the collection.
            - Instead, it partitions the collection into chunks (or batches) of numbers and assigns those chunks to a limited number of threads.
            - The threads are drawn from the thread pool, and 
              the partitioning ensures that the number of active threads is proportional to the number of available CPU cores.
            - For example, on a 4-core machine, it might create 4 threads and process the collection in 4 parallel chunks. 
              Once a thread finishes its assigned chunk, it picks up another chunk until all items are processed.
        
        How is this efficient?
        
            - By limiting the number of threads to match the available CPU cores, Parallel.ForEach reduces thread creation overhead.
            - Tasks share the workload efficiently, leading to better CPU utilization and faster execution.
            - Partitioning ensures that threads spend less time waiting for CPU cycles and more time doing actual work.


        --- When to Use Parallel.Invoke
        
        1. When you have multiple independent tasks that can run simultaneously.
        2. When those tasks are compute-bound (CPU-intensive) rather than I/O-bound (e.g., waiting for network or file operations).


        --- Using ParallelOptions
        The overload of Parallel.Invoke that accepts a ParallelOptions object allows fine-tuning of parallel execution:

        1. Cancellation: Use a CancellationToken to cancel unstarted tasks. Tasks already running will complete:
        
        CancellationTokenSource cts = new CancellationTokenSource();
        ParallelOptions options = new ParallelOptions { CancellationToken = cts.Token };

        Parallel.Invoke(
            options,
            () => ComputeCube(3),
            () => ComputeCube(4),
            () => ComputeCube(5));

        cts.Cancel();

        2. Concurrency Control: Limit the number of threads using the MaxDegreeOfParallelism property:

        ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = 2 };
        Parallel.Invoke(
            options,
            () => ComputeHeavyTask(1),
            () => ComputeHeavyTask(2),
            () => ComputeHeavyTask(3)
        );

        MaxDegreeOfParallelism controls how many tasks or threads can run concurrently in a parallel operation, 
        such as Parallel.ForEach or PLINQ.

        Here’s a simplified explanation of how it works:
        
            1. Without MaxDegreeOfParallelism:
                - By default, the system chooses the number of threads to use based on the available CPU cores.
                - For example, if you have 8 CPU cores, it will typically use 8 threads for maximum efficiency.
        
            2. With MaxDegreeOfParallelism:
                - You can limit the number of threads explicitly by setting the MaxDegreeOfParallelism property in ParallelOptions or PLINQ.
                - For example, setting MaxDegreeOfParallelism = 4 ensures that 
                  only 4 threads will execute at any given time, even if there are more CPU cores.



        ***** Parallel.For and Parallel.ForEach
        
        1. Parallel.For

        A sequential for loop:
        for (int i = 0; i < 100; i++)
            Foo(i);

        Can be parallelised as:
        Parallel.For(0, 100, i => Foo(i));

        2. Parallel.ForEach

        A sequential foreach loop:
        foreach (char c in "Hello, world")
            Foo(c);

        Can be parallelized as:
        Parallel.ForEach("Hello, world", c => Foo(c));

        var keyPairs = new string[6];
        Parallel.For(0, keyPairs.Length, (int index) =>
        {
            keyPairs[index] = RSA.Create().ToXmlString(true);
        });


        --- Indexed Parallel.ForEach
        Parallel.ForEach can also provide the index of each iteration, similar to a for loop. For example:

        Parallel.ForEach("Hello, world", (c, state, index) =>
        {
            Console.WriteLine($"{c} at index {index}");
        });

        */

        /* Concurrent collections in NET

        The System.Collections.Concurrent namespace provides thread-safe collections optimized for high-concurrency scenarios. 
        These collections allow multiple threads to safely interact with the collection without explicit locking, 
        making them particularly useful in parallel or multi-threaded programming.
        
        Available Concurrent Collections
        ---------------------------------------------------------------------
           Concurrent Collection	                 Nonconcurrent Equivalent

        1. ConcurrentStack<T>	                     Stack<T>
        2. ConcurrentQueue<T>	                     Queue<T>
        3. ConcurrentBag<T>	                         (No direct equivalent)
        4. ConcurrentDictionary<TKey, TValue>	     Dictionary<TKey, TValue>

        --- Key Characteristics

        1. Thread Safety:
        Concurrent collections are inherently thread-safe, eliminating the need to use locks for access. 
        However, this does not make all code using them automatically thread-safe.

        2. Atomic Operations:
        Concurrent collections expose specialized methods (like TryAdd, TryRemove, or TryPop) that perform atomic test-and-act operations. 
        These methods ensure correctness without requiring explicit locks.


        In other words, these collections are not merely shortcuts for using an ordinary collection with a lock. 
        To demonstrate, if we execute the following code on a single thread:

            var d = new ConcurrentDictionary<int,int>();
            for (int i = 0; i < 1000000; i++) d[i] = 123;
        
        it runs three times more slowly than this:
        
            var d = new Dictionary<int,int>();
            for (int i = 0; i < 1000000; i++) lock (d) d[i] = 123;
        
        (Reading from a ConcurrentDictionary, however, is fast because reads are lock-free.)


        -----------------------------------------
        var queue = new ConcurrentQueue<int>();

        // Producer
        Task.Run(() =>
        {
            for (int i = 0; i < 10; i++)
            {
                queue.Enqueue(i);
                Console.WriteLine($"Produced: {i}");
            }
        });

        // Consumer
        Task.Run(() =>
        {
            while (true)
            {
                if (queue.TryDequeue(out int item))
                {
                    Console.WriteLine($"Consumed: {item}");
                }
            }
        });

        Console.ReadLine();


        */

        /* IProducerConsumerCollection<T>
        
        The IProducerConsumerCollection<T> interface in .NET represents a thread-safe collection optimized for the producer-consumer pattern. 
        This pattern involves one or more threads (producers) adding items to a collection, 
        while one or more threads (consumers) remove and process those items. 
        The interface and its implementations provide methods to perform these operations safely and efficiently in concurrent scenarios.

        --- Key Concepts of IProducerConsumerCollection<T>
        - Primary Operations

        1. Adding an Element (TryAdd):
        
        ~ Safely adds an item to the collection.
        ~ Returns true if the operation succeeds.
        ~ Can have restrictions, such as not allowing duplicates (in custom implementations).

        2. Removing an Element (TryTake):

        ~ Atomically retrieves and removes an item from the collection.
        ~ Returns true if the operation succeeds, with the item output via an out parameter.
        ~ The exact element removed depends on the underlying collection type:
        ~     Stack: Removes the most recently added item (LIFO).
        ~     Queue: Removes the earliest added item (FIFO).
        ~     Bag: Removes an arbitrary item efficiently.

        3. Utility Methods:

        ~ CopyTo(T[] array, int index): Copies the elements to an array starting at a specified index.
        ~ ToArray(): Returns a new array containing all elements of the collection.

        Implementations of IProducerConsumerCollection<T>

        1. ConcurrentStack<T>:
            A thread-safe stack (LIFO behavior).
            Use case: When you need to process items in reverse order of their addition.

        2. ConcurrentQueue<T>:
            A thread-safe queue (FIFO behavior).
            Use case: Tasks or data items are processed in the order they are added.

        3. ConcurrentBag<T>:
            A thread-safe, unordered collection.
            Use case: Scenarios where the order of processing doesn't matter.

        -------------------------------------------------------------
        ProducerConsumerDemo consumerDemo = new ProducerConsumerDemo();

        public class ProducerConsumerDemo
        {
            private readonly CancellationTokenSource _cts;
            private readonly IProducerConsumerCollection<int> _queue;
            private readonly Random _random;
        
            public ProducerConsumerDemo()
            {
                _cts = new CancellationTokenSource();
                _queue = new ConcurrentQueue<int>();
                _random = new Random();
        
                // Start multiple producers
                var producerTasks = new Task[3];
                for (int i = 0; i < producerTasks.Length; i++)
                {
                    int producerId = i + 1;
                    producerTasks[i] = Task.Run(() => Producer(_queue, _cts.Token, producerId));
                }
        
                // Start multiple consumers
                var consumerTasks = new Task[2];
                for (int i = 0; i < consumerTasks.Length; i++)
                {
                    int consumerId = i + 1;
                    consumerTasks[i] = Task.Run(() => Consumer(_queue, _cts.Token, consumerId));
                }
        
                // Let the producers and consumers run for 5 seconds
                Task.Delay(5000).Wait();
                _cts.Cancel();
        
                // Wait for all tasks to complete
                Task.WhenAll(producerTasks).Wait();
                Task.WhenAll(consumerTasks).Wait();
        
                Console.WriteLine("Processing complete.");
            }
        
            private void Producer(IProducerConsumerCollection<int> queue, CancellationToken token, int producerId)
            {
                while (!token.IsCancellationRequested)
                {
                    int item = _random.Next(1, 100);
                    if (_queue.TryAdd(item))
                    {
                        Console.WriteLine($"Producer {producerId} added: {item}");
                        Task.Delay(100).Wait();
                    }
                }
            }
        
            private void Consumer(IProducerConsumerCollection<int> queue, CancellationToken token, int consumerId)
            {
                while (_queue.TryTake(out int item) && !token.IsCancellationRequested)
                {
                    Console.WriteLine($"Consumer {consumerId} processed: {item}");
                    Task.Delay(100).Wait();
                }
            }
        }

        */

        ProducerConsumerDemo consumerDemo = new ProducerConsumerDemo();

        #region code examples
        //Parallel.ForEach("Hello, world", (c, state, index) =>
        //{
        //    Console.WriteLine($"{c} at index {index}");
        //});


        //CancellationTokenSource cts = new CancellationTokenSource();
        //ParallelOptions options = new ParallelOptions { CancellationToken = cts.Token, MaxDegreeOfParallelism = 2 };

        //Parallel.Invoke(
        //    options,
        //    () => ComputeCube(3),
        //    () => ComputeCube(4),
        //    () => ComputeCube(5));

        //cts.Cancel();


        //IEnumerable<int> numbers = Enumerable.Range(3, 1000000);
        //var cts = new CancellationTokenSource();

        //var parallelQuery = numbers
        //   .AsParallel().WithCancellation(cts.Token)
        //   .Where(num => Enumerable.Range(2, (int)Math.Sqrt(num)).All(nth => num % nth > 0));

        //try
        //{
        //    foreach (int num in parallelQuery)
        //    {
        //        if (num / 1000 >= 1)
        //        {
        //            cts.Cancel();
        //        }

        //        Console.WriteLine(num);
        //    }
        //}
        //catch (OperationCanceledException)
        //{
        //    Console.WriteLine("Operation cancelled by user.");
        //}

        //var numbers = Enumerable.Range(1, 100000);
        //var squares = numbers.Where(n => n % 2 == 0).Select(n => n * n).ToList();

        //var numbers = Enumerable.Range(1, 1000000);
        //var squares = numbers.AsParallel()
        //    .Where(n => n % 2 == 0)
        //    .Select(n => n * n)
        //    .ToList();

        //var sensorData = Enumerable.Range(1, 1000000);
        //var liveReadings = sensorData.AsParallel()
        //    .WithMergeOptions(ParallelMergeOptions.NotBuffered)
        //    .Where(temp => temp > 500_000)
        //    .ToList();


        // --------------------
        //var nums = Enumerable.Range(1, 1_000)
        //    .AsParallel()
        //    .Where(n => n % 2 != 0)
        //    .AsSequential()
        //    .Select(n => n);

        //foreach (var item in nums)
        //{
        //    Console.WriteLine(item);
        //    Task.Delay(200).Wait();
        //}

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