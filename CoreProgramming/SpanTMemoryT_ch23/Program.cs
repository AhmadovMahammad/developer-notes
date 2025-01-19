internal class Program
{
    private static void Main(string[] args)
    {
        /* Introduction

        The Span<T> and Memory<T> structs are pivotal features in modern .NET programming, 
        designed to provide a high-performance, low-allocation mechanism for working with contiguous memory blocks. 
        Their introduction addresses the growing demand for efficient memory management and performance optimization, 
        particularly in scenarios where frequent allocations and garbage collection can become a bottleneck. 
        These types act as lightweight abstractions over arrays, strings, or any contiguous block of memory, 
        including both managed and unmanaged memory. 

        The primary purpose of Span<T> and Memory<T> is to enable micro-optimization by reducing managed memory allocations. 
        This minimizes the load on the garbage collector, 
        which can be critical in performance-sensitive applications such as ASP.NET Core pipelines or data parsing engines like JSON parsers. 

        A fundamental feature of these types is their ability to support slicing. 
        Slicing refers to the ability to work with a portion of an array, string, or memory block without creating a copy of the data. 
        This is achieved by simply referencing the original memory while exposing a specific subset of it. 
        This capability is not only memory-efficient but also computationally inexpensive, 
        as it avoids the overhead of creating new data structures for partial operations.

        The Span<T> struct is designed to be extremely lightweight, consisting of just two fields: 
        a pointer to the start of the memory and its length. 
        
        This simplicity allows it to efficiently represent any contiguous block of memory. 
        However, the contiguous nature of memory blocks imposes certain limitations; 
        for example, Span<T> cannot handle noncontiguous memory. 
        For such scenarios, .NET provides the ReadOnlySequence<T> class, which functions as a linked list for handling disjoint memory blocks.

        Both Span<T> and Memory<T> have read-only counterparts, ReadOnlySpan<T> and ReadOnlyMemory<T>, respectively. 
        These read-only types ensure immutability, preventing accidental modifications to the underlying data.

        The design philosophy of Span<T> and Memory<T> reflects a commitment to balancing performance and safety. 
        While their use is optional in many applications, 
        they offer significant benefits when working in environments where memory management and execution speed are critical. 
        Developers who encounter APIs leveraging these types can confidently use them, 
        even if their performance advantages are not immediately relevant to the task at hand. 
        These types represent a step forward in providing developers with tools that 
        enhance performance without sacrificing the safety and productivity that are hallmarks of the .NET ecosystem.

        */
    }
}