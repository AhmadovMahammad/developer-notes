using System.Buffers;
using System.IO;
using System.IO.Pipes;
using System.Net;
using System.Runtime;
using System.Text;

namespace Disposal_GarbageCollection_ch12
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Introduction to Disposal and Garbage Collection

            In .NET, there are two essential processes involved in managing resources: 
            1. disposal and 2. garbage collection. 
            These concepts play a vital role in ensuring that your applications run efficiently and 
            do not waste system resources, especially when dealing with unmanaged resources like 
            ...files, network connections, or system handles.

            */

            /* Disposal

            Disposal refers to the process of releasing or "cleaning up" resources like 
            file handles, database connections, and unmanaged objects, 
            which are not automatically managed by the .NET runtime. 

            This is particularly important when working with resources 
            that are expensive to maintain or are limited in number.

            * IDisposable Interface: 
            The primary mechanism in .NET for handling disposal is through the IDisposable interface. 
            This interface provides a Dispose() method that can be implemented by classes that manage such resources. 

            Example: Imagine a file stream that keeps a file open until it’s explicitly closed.
            If you don't release a file handle after using it (by not calling Dispose() or closing it properly), 
            the file remains locked by the operating system. 
            This can prevent other parts of your code—or even other applications—from accessing or modifying the file.

            // Open a file and forget to close it.
            FileStream fs = new FileStream("example.txt", FileMode.OpenOrCreate);
            using StreamWriter sw = new StreamWriter(fs);
            sw.WriteLine("some text");

            // Later in the code, try to open the same file again.
            try
            {
                using FileStream fs_ = new FileStream("example.txt", FileMode.OpenOrCreate);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"error: {exception.Message}");
                // error: The process cannot access the file 'C:\...\CoreProgramming\Disposal_GarbageCollection_ch12\...\example.txt'
                // because it is being used by another process.
            }

            */

            /* Garbage Collection

            Garbage collection (GC) is the automated process that the CLR (Common Language Runtime) 
            uses to reclaim the memory used by objects that are no longer needed. 
            Unlike disposal, you do not need to directly manage memory cleanup. 
            This is handled automatically by the CLR.

            Managed Objects: 
            In .NET, objects created in the managed heap (like classes and structs) are automatically tracked. 
            The CLR periodically checks for objects that are no longer reachable or in use, 
            and when it finds them, it releases their memory.

            Automatic Process: 
            GC is automatic, meaning the programmer doesn’t control when it happens, 
            and it is designed to be as efficient as possible without affecting application performance. 
            However, GC primarily deals with managed memory. 

            For unmanaged resources (like file handles, database connections), 
            it relies on the programmer to implement disposal mechanisms.
            */

            /* Finalizers

            Finalizers (also known as destructors in C#) are special methods that 
            the garbage collector calls just before an object is destroyed. 

            They provide a way to clean up unmanaged resources (like file handles, database connections, etc.) 
            if they haven’t already been released through explicit disposal.

            1. A finalizer runs automatically when the garbage collector (GC) 
            determines that an object is no longer accessible.
            2. Finalizers are non-deterministic, meaning you cannot predict exactly 
            when they will run because it depends on when the GC collects the object.
            3. Finalizers can delay garbage collection since objects with finalizers are collected in a two-step process: 
                first, they are marked for finalization, and then on a second pass, the GC reclaims their memory.

            */

            /* IDisposable and Finalizer Pattern

            While finalizers help as a backup, 
            a more deterministic and preferred way to clean up resources is using the IDisposable interface.
            The combination of IDisposable and a finalizer ensures that resources are freed deterministically and 
            also provides a safety net in case the developer forgets to dispose of the object manually.

            public class FileManager : IDisposable
            {
                private readonly FileStream _fileStream; // Unmanaged resource (file handle)
                private bool _disposed;

                public FileManager()
                {
                    _fileStream = new FileStream("example.txt", FileMode.OpenOrCreate);
                    Console.WriteLine("File Opened.");
                }

                public void Dispose()
                {
                    Dispose(true);
                }

                protected virtual void Dispose(bool disposing)
                {
                    if (!_disposed)
                    {
                        if (disposing) // it is called by Dispose method and you should clean up managed resources
                        {
                            Console.WriteLine("Releasing managed resources (if any).");
                            // For example, closing a database connection or clearing a cache would go here.
                        }

                        // Release unmanaged resources (FileStream here)
                        if (_fileStream is not null)
                        {
                            _fileStream.Close();
                            Console.WriteLine("File stream closed (unmanaged resource).");
                        }
                    }

                    _disposed = true;
                }

                ~FileManager()
                {
                    Dispose(false);
                    // Finalizer calls Dispose to clean up unmanaged resources
                    // Because it handled managed resources itself by CLR
                }

                public void WriteToFile(string content)
                {
                    if (_disposed)
                    {
                        throw new ObjectDisposedException(GetType().Name);
                    }

                    byte[] bytes = Encoding.UTF8.GetBytes(content);
                    _fileStream.Write(bytes, 0, bytes.Length);
                    Console.WriteLine("Content written to file.");
                }
            }

            Explanation: 
            1. Dispose() Method: Allows the user to manually release both managed and unmanaged resources. 
            When Dispose() is called, it suppresses finalization using GC.SuppressFinalize() to prevent 
            the finalizer from running since the cleanup has already happened.

            2. Finalizer (Destructor): If the user forgets to call Dispose(), 
            the finalizer ensures that unmanaged resources are still released, acting as a fallback.
            */

            /* Clearing Fields in Disposal
             
            In disposal, clearing fields isn't always necessary,  but there are specific scenarios where doing so adds value, 
            such as unsubscribing from events and clearing sensitive data:

            1. Unsubscribing from Events
            Why it's important: When an object subscribes to events, 
            those event subscriptions create references that may prevent the object from being garbage-collected, 
            even if the object is no longer needed. 
            This can cause managed memory leaks by keeping the object alive in memory unnecessarily.

            You should unsubscribe from any events that the object has subscribed to during its lifetime. 
            This ensures that the object is no longer referenced, 
            allowing the garbage collector (GC) to reclaim its memory when it is no longer needed.

            2. Clearing Sensitive Data

            In scenarios where an object holds sensitive data (e.g., encryption keys or passwords), 
            clearing that data during disposal is crucial for security. 
            Even though the garbage collector will eventually release the object’s memory, 
            the data could be left in memory long enough to be potentially accessed by malicious processes. 
            By manually clearing the sensitive fields, you mitigate this risk.
            */

            /* Finalizers (elaborated)
             
            (Although similar in declaration to a constructor, finalizers cannot be declared as public or static, 
            cannot have parameters, and cannot call the base class.)

            Garbage Collection Phases

            1. Identification of Unused Objects:
            The garbage collector (GC) periodically identifies objects in memory that are no longer in use or 
            accessible by your application. These objects are considered "ripe for deletion."

            2. Immediate Deletion vs. Finalizers:

            1. Objects Without Finalizers: 
            If an object does not have a finalizer, it can be deleted immediately from memory 
            during this initial phase of garbage collection.
            
            2. Objects With Finalizers: 
            If an object has a finalizer (a method defined to clean up resources), it is not deleted immediately. 
            Instead, it is placed in a special queue for further processing. 
            This means the object is kept alive temporarily to allow its finalizer to run.

            3. Execution of Finalizers:

            After the main garbage collection phase is complete and your application resumes, 
            a separate finalizer thread starts processing the objects in the queue. 
            This thread runs in parallel to your program.
            
            Each object's finalizer is executed, 
            which allows you to perform any necessary cleanup tasks (like releasing unmanaged resources).


            ---Implications of Using Finalizers

            1. Performance Overhead:
            Finalizers slow down memory allocation and collection. 
            The GC has to keep track of which objects have finalizers and ensure they are processed, 
            adding overhead to the garbage collection process.

            2. Extended Object Lifetimes:
            Objects with finalizers remain in memory longer than necessary. 
            They must wait for both the finalization process and the next garbage collection cycle, 
            delaying the release of resources.

            */

            /* What is Resurrection?
             
            Resurrection refers to a scenario in .NET garbage collection where an object that was considered 
            "dead" or eligible for garbage collection is brought back to life because some reference to it is restored. 
            
            ---How Resurrection Happens
            1. The .NET Garbage Collector (GC) identifies an object as no longer reachable and 
            prepares it for garbage collection.

            2. The GC checks if the object has a finalizer (destructor). 
            If so, the object is not immediately collected but instead queued for finalization.

            3. The finalizer runs, allowing you to execute some code before the object is collected.

            4. During finalization, if some other live object starts referencing the object marked for finalization, 
            the object evades garbage collection. This phenomenon is known as resurrection.

            Example:
            Let’s consider a class that manages a temporary file. 
            You might want to ensure that when the class is finalized, the temporary file is deleted.

            However, there are cases where deleting the file may fail 
            (e.g., due to a lack of permissions or the file being locked by another process). 
            In this case, you might want to log the failure without crashing the application.

            public class TempFileManager
            {
                private static readonly ConcurrentQueue<TempFileManager> FailedDeletions = new ConcurrentQueue<TempFileManager>();
                public readonly string FilePath = string.Empty;
                public Exception? DeletionError { get; private set; }

                public TempFileManager(string filePath)
                {
                    FilePath = filePath;
                    Console.WriteLine($"Temporary file created: {FilePath}");
                }

                ~TempFileManager()
                {
                    try
                    {
                        File.Delete(FilePath);
                        Console.WriteLine($"Temporary file deleted: {FilePath}");
                    }
                    catch (Exception exception)
                    {
                        DeletionError = exception;
                        FailedDeletions.Enqueue(this);
                        Console.WriteLine($"Failed to delete file: {FilePath}. Error: {exception.Message}");
                    }
                }
            }

            TempFileManager? tempFileManager = new TempFileManager("temporary.txt");
            tempFileManager = null; // Make the object eligible for garbage collection

            // Force a garbage collection (for demonstration purposes)
            GC.Collect();
            GC.WaitForPendingFinalizers();

            while (TempFileManager.FailedDeletions.TryDequeue(out TempFileManager? failedTempFile))
            {
                Console.WriteLine($"Handling failed deletion for: {failedTempFile.FilePath}");
                // Perform any additional actions, like retrying deletion or logging
            }

            */

            /* Phases of the GC (Elaborated explanation)
             
            When you create new objects in your program using the new keyword, they are allocated memory on the heap. 
            Over time, as more objects are created, the heap fills up, or the system detects that memory usage has reached a certain threshold. 
            At this point, the garbage collector wakes up and initiates its process.

            -----Root References and Object Graph

            When the garbage collector (GC) starts its work, 
            it needs to figure out which objects in memory are still being used by the application and which ones are no longer needed. 
            To do this, it begins by identifying what are called root references.

            These are references that point to objects, 
            and they serve as starting points for the GC’s marking process. Common root references include:

            1. Static fields/variables in your code.
            2. Local variables that are currently in use on the call stack.
            3. Active threads in the application that are executing code.

            NOTE: 
            
            Once the GC identifies these root references, it starts tracing the object graph. 
            Think of an object graph as a network of objects in memory, 
            where each object can reference other objects. 
            
            For example, a list object might contain references to several string objects, 
            which in turn might reference other objects.

            -----Marking Process

            1. Starting from the roots:
            
            The GC looks at each root reference. 
            For each root, it follows the reference to the object that the root is pointing to in the heap. 
            These objects are immediately marked as reachable.

            Once an object is marked as reachable, the GC continues to explore it to see if it has references to other objects 
            (for instance, fields or properties that reference other objects). 
            If it does, those referenced objects are also marked as reachable.

            2. Marking objects as reachable:

            When we say the GC marks an object, we mean it flags that object internally in memory. 
            It essentially says, "This object is still in use; don't collect it."
            The marking step doesn't involve physically moving or altering objects in memory but 
            simply sets a flag on each object to indicate it’s still being used.

            This process continues recursively: the GC starts from the root, 
            marks an object, checks for any references that object holds to other objects, 
            and marks those as well, until it can’t find any more references.

            -----What Happens to Unmarked Objects?

            Once the marking phase is complete, all the objects that are not marked as reachable are considered garbage. 
            These unmarked objects are no longer accessible by the application 
            (because there are no references to them from the root or any reachable objects). 
            Since the application can no longer access these objects, 
            they are considered safe to clean up and free their memory.

            -----Marking and Compacting

            Once all reachable objects are identified, the GC performs a compaction step.
            The purpose of this is to deal with memory fragmentation, 
            which can occur as objects are created and destroyed, leaving small gaps in memory.

            The compaction phase shifts all live objects to the start of the heap, 
            thus freeing up a continuous block of memory for future allocations

            This also simplifies the allocation process for new objects, 
            as the GC can now simply allocate memory at the end of the heap, 
            instead of trying to find small fragmented spaces to fit new objects. 
            This keeps the memory allocation process fast.

            -----Generations in the GC

            The .NET GC uses generations to optimize its performance. Objects are grouped into three generations:

            Generation 0: 
            This is for short-lived objects (like temporary variables). When a garbage collection is triggered, 
            Generation 0 objects are the first to be checked and collected if they're no longer needed.

            Generation 1: 
            This is for medium-lived objects, usually those that survived a previous GC cycle in Generation 0.

            Generation 2: 
            This holds long-lived objects, like static data or objects that persist for the entire duration of the application. 
            Collecting objects from Generation 2 happens less frequently because it is more expensive in terms of performance.

            Gen0 and Gen1 are known as ephemeral (short-lived) generations.
            The CLR keeps the Gen0 section relatively small (with a typical size of a few hundred KB to a few MB).

            */

            /* Memory Allocation and OutOfMemoryException
             
            When the GC finishes compacting and clearing memory, it frees up space for future objects.
            However, if there is insufficient space on the heap after a garbage collection cycle,
            the .NET runtime will attempt to request more memory from the operating system.

            The operating system grants memory in blocks called pages, which are usually 4 KB in size on most systems. 
            When the heap needs more memory, the runtime will ask the OS for additional memory pages. 
            The OS checks whether it has available memory in the system's virtual memory space to allocate more pages to the process. 
            If the OS has enough free memory, it will allocate these additional pages, and the heap grows accordingly.

            However, if the system is low on memory or if there’s a limit on how much memory the process can use (due to system constraints), 
            the OS may deny the request for more memory. 
            In this case, the runtime throws an OutOfMemoryException because it cannot fulfill the memory allocation request. 

            */

            /* Garbage Collection and Application Freezing
             
            One consequence of garbage collection is that, during the collection process, 
            all threads in the application can be temporarily frozen.

            This happens because the GC needs to ensure that no references change while it's tracing the object graph. 
            This pause is often referred to as a STOP-THE-WORLD EVENT, and

            while modern garbage collectors try to minimize the duration of this pause, 
            it can still cause noticeable slowdowns in certain applications, 
            especially in real-time systems or applications with strict performance requirements.

            */

            /* The Large Object Heap (LOH)
             
            The (LOH) is designed to handle objects that are larger than a certain threshold (currently around 85,000 bytes), 
            which includes large arrays, buffers, or large object graphs. 
            
            Since managing large objects efficiently is challenging, 
            the LOH has some unique characteristics compared to the Small Object Heap (SOH) used for smaller objects.

            -----Key Characteristics of the LOH:

            1. Non-generational: 
            Unlike the SOH, the LOH doesn't follow the generational model of garbage collection (Gen0, Gen1, Gen2). 
            All objects in the LOH are treated as Gen2 objects, meaning they aren't collected as frequently as younger objects in the SOH.

            2. No Default Compaction: 
            By default, the LOH does not compact memory. 
            This is because moving large blocks of memory around during garbage collection would be expensive in terms of time and performance.

            -----Fragmentation in the LOH:

            Because the LOH doesn’t compact memory, fragmentation can occur. 
            When large objects are allocated and later collected, the memory space they occupied isn’t immediately compacted. 
            This can leave gaps or holes in memory. 
            These gaps may be too small to be used by future allocations, 
            leading to a fragmented heap where free space is available but unusable because it's too small for any new large object.

            For example:
            If a large object of 100,000 bytes is allocated and later freed, there’s now a 100,000-byte hole in the LOH.
            If the next large object needs 150,000 bytes, this gap can't be used, so the GC must find or allocate new space elsewhere, leaving the hole unused.

            */

            /* Forcing Garbage Collection
             
            You can manually force a garbage collection at any time by calling GC.Collect. 
            Calling GC.Collect without an argument instigates a full collection. 
            If you pass in an integer value, only generations to that value are collected, 
            so GC.Collect(0) performs only a fast Gen0 collection.

            NOTE: In general, you get the best performance by allowing the GC to decide when to collect.
            
            */

            /* Array Pooling
             
            Array Pooling is an optimization technique designed to reduce the overhead of frequent array allocations 
            by reusing arrays from a pool, introduced in .NET Core 3. 
            
            The idea is to "rent" an array when needed and "return" it when done, 
            which minimizes the work done by the garbage collector (GC) and helps prevent memory fragmentation.

            Here's how array pooling works:

            1. Renting an Array:
            Instead of allocating a new array every time, you can "rent" an array from a shared pool using the ArrayPool<T> class.

            int[] pooledArray = ArrayPool<int>.Shared.Rent(100);  // Rent an array of at least 100 elements.

            The pool manager might give you an array that's larger than what you requested, 
            typically rounding up to powers of two for efficiency. 
            This allows faster allocation and makes it easier to manage arrays of different sizes.

            2. Returning an Array:
            Once you're done with the array, instead of leaving it to the GC to clean up, 
            you explicitly "return" it to the pool:

            ArrayPool<int>.Shared.Return(pooledArray);

            3. Clearing Arrays:
            By default, the data in the array is not cleared when you return it, which saves performance. 
            However, if you need to clear the array's contents (for security or correctness), you can pass a true flag:

            ArrayPool<int>.Shared.Return(pooledArray, clearArray: true);

            Array pooling is especially beneficial in applications like ASP.NET Core or game development, 
            where arrays (such as buffers) are frequently allocated and discarded. 
            For example, in network or I/O operations, large arrays are often needed for reading or writing data. 
            By using array pooling, the application can reuse the same memory buffers.

            */
        }
    }
}