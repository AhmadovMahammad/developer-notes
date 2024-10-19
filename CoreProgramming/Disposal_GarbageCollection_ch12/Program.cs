using System.IO;
using System.IO.Pipes;
using System.Net;

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
             
            */
        }
    }
}