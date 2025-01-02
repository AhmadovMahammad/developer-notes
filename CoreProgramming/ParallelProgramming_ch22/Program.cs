namespace ParallelProgramming_ch22;
internal class Program
{
    private static void Main(string[] args)
    {
        /* Why PFX?
        
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


        */

        int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        int[] squares = new int[numbers.Length];

        Parallel.For(0, numbers.Length, (int index) =>
        {
            squares[index] = numbers[index] * numbers[index];
        });

        Console.WriteLine($"Squares: {string.Join(',', squares)}");
    }
}