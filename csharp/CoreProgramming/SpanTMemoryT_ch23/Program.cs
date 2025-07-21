using SpanTMemoryT_ch23;
using System;
using System.Buffers;
using System.Runtime.InteropServices;

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

        /* Spans and Slicing
        
        When working with arrays or contiguous memory, a common scenario involves processing only a portion of the data.
        Traditional approaches to handling this involve either copying the desired section into a new array or 
        adding parameters like offset and count to methods. 
        
        Both of these approaches have drawbacks: 
        1. copying is inefficient in terms of memory and CPU usage, while 
        2. adding parameters can clutter method signatures and make code harder to maintain.

        The introduction of Span<T> and ReadOnlySpan<T> eliminates these issues by allowing efficient, 
        non-copying access to subsections of arrays or other memory blocks. 
        
        A Span<T> is a lightweight representation of a contiguous region of memory,
        and it provides slicing capabilities, allowing you to work with subsections of the memory as if they were independent arrays. 
        This makes Span<T> highly suited for scenarios requiring high performance and low memory overhead.

        
        --- Advantages of Using Spans for Slicing

        1. Efficiency: Slicing a Span<T> or ReadOnlySpan<T> doesn’t create a new array or copy data. 
        Instead, it references the same memory, with an adjusted pointer and length.

        2. Simpler Method Signatures: By changing a method parameter from T[] to ReadOnlySpan<T>, 
        slicing becomes intrinsic to the method’s functionality without needing extra parameters like offset or count.
        
        3. Performance: The indexer for Span<T> and ReadOnlySpan<T> directly accesses the underlying data using ref readonly, 
        enabling high performance comparable to working directly with arrays.

        --------------------------------------------
        Example: Summing an Array or a Portion of It
        Let’s start with a simple method to sum the elements of an array. Conventionally, this would be written like so:

        int sum = Sum([1, 2, 3, 4, 5]);
        private int Sum(int[] nums)
        {
            int total = 0;
            foreach (var item in nums)
            {
                total += item;
            }

            return total;
        }

        This works well for summing an entire array. 
        However, if we want to sum only a portion of the array, we must either copy the relevant elements into a new array or 
        modify the method to accept additional parameters, like offset and count.
        Using Span<T> or ReadOnlySpan<T> solves this neatly. The method can be updated as follows:

        private int Sum(ReadOnlySpan<int> numbers)
        {
            int total = 0;
            foreach (int i in numbers)
                total += i;
            return total;
        }

        Here’s why this is better:

        1. Flexibility: You can pass a full array or a sliced portion without modifying the method.
        2. Performance: No unnecessary memory allocation or data copying occurs.


        --- Slicing with Spans
        var numbers = new int[1000];
        for (int i = 0; i < numbers.Length; i++)
            numbers[i] = i;
       
        1. Using slicing, you can sum a portion of the array without creating a copy. Here’s how:
        int total = Sum(numbers.AsSpan(250, 500));

        2. The AsSpan method provides a Span<T> over the array. 
           Similarly, if you already have a Span<T>, you can use the Slice method:

        Span<int> span = numbers.AsSpan();
        int total = Sum(span.Slice(250, 500));

        --- Using Indices and Ranges in C# 8

        Span<int> span = numbers.AsSpan();

        // access the last element
        Console.WriteLine(span[^1]);

        // sum the last 10 elements
        Console.WriteLine(Sum(span[..10]));

        // sum elements from the 100th index to the end
        Console.WriteLine(Sum(span[100..]));

        // sum the last 5 elements
        Console.WriteLine(Sum(span[^5..]));


        NOTE:
        We can call Sum with an array because there’s an implicit conversion from T[] to Span<T> and ReadOnlySpan<T>. 
        Another option is to use the AsSpan extension method


        --- CopyTo and TryCopyTo
        The CopyTo method copies elements from one span (or Memory<T>) to another. 
        In the following example, we copy all of the elements from span x into span y:

        Span<int> x = new int[] { 1, 2, 3, 4, 5 };
        Span<int> y = new int[5];

        x.CopyTo(y);

        for (int i = 0; i < y.Length; i++)
        {
            Console.WriteLine(y[i]);
        } // Output: 1 2 3 4 5


        Slicing makes this method much more useful. 
        In the next example, we copy the first half of span x into the second half of span y:

        Span<int> x = new int[] { 1, 2, 3, 4 };
        Span<int> y = new int[] { 10, 20, 30, 40 };

        x[..2].CopyTo(y[2..]);

        for (int i = 0; i < y.Length; i++)
        {
            Console.WriteLine(y[i]);
        } // Ouput: 10 20 1 2

        If there’s not enough space in the destination to complete the copy, CopyTo throws an exception, 
        whereas TryCopyTo returns false (without copying any elements).


        */

        /* Memory<T>
        
        Span<T> and ReadOnlySpan<T> are ref structs designed to optimize performance 
        by allowing efficient access to contiguous memory regions. 
        --
        Being ref structs provides several benefits, 
        such as enabling safe interaction with stack-allocated memory and reducing garbage collection overhead. 
        However, their nature as ref struct imposes several critical limitations, 
        which make them unsuitable for certain use cases.

        --- Understanding ref struct in C# and Its Significance

        The term ref struct in C# refers to a specialized kind of structure that 
        has specific constraints and optimizations related to memory management. 
        Ref struct is primarily used for types like Span<T> and ReadOnlySpan<T> 
        to ensure safe and efficient interaction with memory, particularly stack-allocated memory. 
        To elaborate, let’s break this down:

        What Is a ref struct?
        A ref struct is a structure in C# that is restricted to the stack. 
        This means instances of ref struct cannot be allocated on the heap.
        Instead, they are stored in the stack, 
        which is a short-lived memory area tied to the execution of methods and functions.

        --- Why ref struct Imposes Constraints

        1. Cannot Be Stored on the Heap:
        The stack is a temporary memory region, 
        and allowing a ref struct to escape to the heap could lead to undefined behavior 
        (e.g., dangling pointers or memory corruption). 
        
        Therefore, ref struct cannot be used as a field in a class or stored in heap-allocated objects.

        2. Cannot Be Used in Asynchronous Methods or Iterators:
        https://www.linkedin.com/pulse/exploring-async-await-state-machine-c-sarvaha-systems-hmy5f/

        Asynchronous methods and iterators are transformed by the compiler into state machines, 
        which are heap-allocated objects. 
        Any parameters or local variables inside these methods become fields in the state machine, 
        effectively placing them on the heap.
        
        Since ref struct cannot be allocated on the heap, 
        the compiler disallows their usage in such methods:

        ~ async Task Foo(Span<int> span) // Compile-time error!

        This restriction ensures that stack-bound memory referenced by Span<T> 
        cannot outlive the method’s execution.

        2.1. 

         // Allocate memory on the stack using 'stackalloc' and wrap it in a Span<int>
        Span<int> numbers = stackalloc int[5];

        // Initialize the values
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i * 2; // Fill with even numbers: 0, 2, 4, 6, 8
        }

        // Print values
        Console.WriteLine("Values in Span:");
        foreach (int num in numbers)
        {
            Console.WriteLine(num);
        }

        // Attempt to pass Span<int> to an asynchronous method (not allowed)
        // Uncommenting the following line will cause a compile-time error:
        // await AsyncMethod(numbers);

        -- 

        // Compile-time error: Span<int> cannot be used in an async method
        //static async Task AsyncMethod(Span<int> span)
        //{
        //    await Task.Delay(1000);
        //    Console.WriteLine(span[0]);
        //}


        When you write an asynchronous method or an iterator, 
        the compiler generates a state machine behind the scenes. 
        This state machine is implemented as a private class or struct, which contains:

        - Fields to hold the method's parameters and local variables.
        - Fields to track the current state of the method.
        - Code to manage the transitions between states during execution.


        2.2. 

        // Allocate memory on the stack using 'stackalloc' and wrap it in a Span<int>
        Span<int> numbers = stackalloc int[5];

        // Initialize the values
        for (int i = 0; i < numbers.Length; i++)
        {
            numbers[i] = i * 2; // Fill with even numbers: 0, 2, 4, 6, 8
        }

        // Attempt to use Span<int> in an iterator method
        // Uncommenting this will cause a compile-time error
        // foreach (var value in GetValues(numbers.AsSpan(1, 3)))
        // {
        //     Console.WriteLine(value);
        // }

        --

        // Iterator method - not allowed with Span<T>
        // static IEnumerable<int> GetValues(Span<int> span)
        // {
        //     foreach (var item in span)
        //     {
        //         yield return item; // Compile-time error
        //     }
        // }


        3. Cannot Be Captured by Closures:

        When a lambda expression or anonymous method captures a variable from its enclosing scope, 
        the compiler generates a closure, which is also heap-allocated. 
        Since ref struct cannot reside on the heap, it cannot be captured in such scenarios:

        Span<int> span = stackalloc int[10];
        Func<int> lambda = () => span[0]; // Compile-time error!

        -----------------------------------------------------------------------------

        The Memory<T> and ReadOnlyMemory<T> structs are designed to provide a way to work with memory in a flexible and efficient manner, 
        particularly for scenarios where Span<T> and ReadOnlySpan<T> cannot be used. 
        
        While Span<T> is optimized for stack-allocated and temporary memory operations, 
        Memory<T> overcomes limitations by allowing its use with heap-allocated memory. 
        
        It is not a ref struct like Span<T>, 
        making it suitable for use in fields, lambda expressions, asynchronous methods, and iterators.

        --- 1. Obtaining a Memory<T> or ReadOnlyMemory<T> Instance

        Memory<int> memory_1 = new int[] { 1, 2, 3, 4, 5 }; // Implicit conversion
        Memory<int> memory_2 = new int[] { 1, 2, 3, 4, 5 }.AsMemory(); // AsMemory() extension

        Here, Memory<T> wraps the array. 
        Unlike Span<T>, which is restricted to stack-allocated data, 
        Memory<T> can be stored in heap-allocated objects, making it more versatile.


        --- 2. Conversion to Span<T> or ReadOnlySpan<T>

        You can convert a Memory<T> or ReadOnlyMemory<T> to a Span<T> or ReadOnlySpan<T> using the Span property. 
        This conversion does not copy the underlying data. It simply provides a different view of the memory.

        private static void ProcessMemory(Memory<int> memory)
        {
            Span<int> span = memory.Span; // conversion to Span<T>
            for (int i = 0; i < span.Length; i++)
            {
                span[i] *= 2; // modify the memory
            }
        }

        - This allows you to interact with Memory<T> as if it were a span, enabling operations such as slicing and in-place modifications.

        You don't need to convert Memory<T> to Span<T> in every scenario; it depends on the specific operations you want to perform. 
        Memory<T> and ReadOnlyMemory<T> are abstractions for working with memory, 
        but they are not designed for all operations directly. 
        Converting Memory<T> to Span<T> is crucial for situations where you need:

        1. Direct Element Access and Modification
        
        Memory<T> itself is immutable in terms of its data access API, 
        so you cannot modify elements of the underlying data directly through Memory<T>. 
        However, Span<T> allows mutable, indexed access to the elements.

        Memory<int> memory = new int[] { 1, 2, 3 }.AsMemory();
        // memory[1] = 42; // Error! Memory<T> does not allow direct index-based assignment
        
        Span<int> span = memory.Span;
        span[1] = 42; // Allowed
        Console.WriteLine(span[1]); // Outputs 42

        Why Not Use Memory<T> Directly?
        Memory<T> has slicing, copying, and other useful methods, 
        but it is primarily designed to represent a memory block rather than provide efficient access to its elements. For example:
        
        1: Memory<T> doesn’t allow you to modify elements directly (memory[0] is invalid for assignment).
        2: Operations like sorting, summing, or in-place transformation require a mutable view, which Span<T> provides.

        
        --- 3. Slicing and Length Access
        
        Memory<T> and ReadOnlyMemory<T> support slicing using either the Slice() method or the C# range operator (..). 
        You can also access their length using the Length property.

        Memory<int> memory = new int[] { 10, 20, 30, 40, 50 }.AsMemory();
        Memory<int> slicedMemory = memory.Slice(1, 3); // Slices {20, 30, 40}

        Console.WriteLine($"Length: {slicedMemory.Length}");
        Span<int> span = slicedMemory.Span; // Use the slice as a span
        foreach (var value in span)
        {
            Console.WriteLine(value);
        }


        --- 4. Renting Memory with MemoryPool<T>
        
        Memory<T> can also be obtained from memory pools. 
        Using the System.Buffers.MemoryPool<T> class, you can rent reusable memory blocks to reduce the load on the garbage collector.

         MemoryPool<int> pool = MemoryPool<int>.Shared;
        using IMemoryOwner<int> rentedMemory = pool.Rent(); // rents a block of memory
        Memory<int> memory = rentedMemory.Memory[..20];

        Span<int> span = memory.Span;
        for (int i = 0; i < span.Length; i++)
        {
            span[i] = (int)Math.Pow(i, 2) + 5;
        }

        Console.WriteLine(string.Join(',', span.ToArray()));



        --- 5. Improved Split Implementation with ReadOnlyMemory<char>

        One of the limitations of Span<T> is the inability to create arrays of spans, 
        which makes certain operations, like implementing a Split method, impractical. 
        ReadOnlyMemory<char> overcomes this limitation by allowing you to return 
        slices of the original string instead of creating new strings for each word.

        static IEnumerable<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> input)
        {
            int wordStartIndex = 0;

            for (int i = 0; i <= input.Length; i++)
            {
                if (i == input.Length || char.IsWhiteSpace(input.Span[i]))
                {
                    yield return input[wordStartIndex..i];
                    wordStartIndex = i;
                }
            }
        }

        var input = "The quick brown fox jumps over the lazy dog".AsMemory();
        foreach (var word in Split(input))
        {
            Console.WriteLine($"Word: {word}");
        }

        Here, Split operates efficiently by yielding slices of the original string as ReadOnlyMemory<char>
        instead of creating new string objects. 
        This reduces allocations and improves performance, especially for large strings.


        --- 6. Recommendations for Method Parameters
        
        When designing methods, it’s better to accept Span<T> or ReadOnlySpan<T> where possible for performance and safety. 
        However, if you need to support scenarios involving heap allocation, fields, or asynchronous methods, 
        Memory<T> and ReadOnlyMemory<T> are the preferred choices.

        void ProcessSpan(Span<int> data) { // Span for fast, temporary processing  }
        void ProcessMemory(Memory<int> data) { // Memory for heap and async support }

        */

        /* Advanced CharSpanSplitter
        
        NOTE:
        In C#, foreach works with any type that satisfies the following:

        1. Has a public method named GetEnumerator that returns an enumerator.
        2. The enumerator must:
            2.1. Implement a MoveNext method that advances the enumerator and returns a bool indicating if there are more items.
            2.2. Expose a Current property that provides the current item.


        public ref struct CharSpanSplitter
        {
            private readonly ReadOnlySpan<char> _input;
        
            public CharSpanSplitter(ReadOnlySpan<char> input)
            {
                _input = input;
            }
        
            public readonly CharEnumerator GetEnumerator() => new CharEnumerator(_input);
        }
        
        public ref struct CharEnumerator
        {
            private readonly ReadOnlySpan<char> _input;
            private int _wordPosition;
        
            public ReadOnlySpan<char> Current { get; private set; }
        
            public CharEnumerator(ReadOnlySpan<char> input)
            {
                _input = input;
                _wordPosition = 0;
                Current = default;
            }
        
            public bool MoveNext()
            {
                for (int i = _wordPosition; i <= _input.Length; i++)
                {
                    if (i == _input.Length || char.IsWhiteSpace(_input[i]))
                    {
                        Current = _input[_wordPosition..i];
                        _wordPosition = i + 1;
        
                        return true;
                    }
                }
        
                return false;
            }
        
            public void Reset()
            {
                _wordPosition = 0;
            }
        }


        var span = "the quick brown fox".AsSpan();
        var splitter = new CharSpanSplitter(span);

        foreach (var line in splitter)
        {
            Console.WriteLine(line.ToString());
        }

        OUTPUT:
        the
        quick
        brown
        fox

        */

        /* Working with Stack-Allocated and Unmanaged Memory
        
        In high-performance applications, reducing heap allocations minimizes garbage collector (GC) overhead and improves responsiveness. 
        Traditionally, managing memory outside the heap required using pointers with stackalloc for stack-based memory or 
        functions like Marshal.AllocHGlobal for unmanaged memory. 
        These approaches, while efficient, were error-prone due to the manual memory management and lack of safety checks.

        The introduction of Span<T> and ReadOnlySpan<T> provides a safer, modern alternative that 
        retains the benefits of stack and unmanaged memory while adding versatility, 
        such as slicing and bounds-checking.


        --- 1. Stack-Allocated Memory with Span<T>
        
        The stack is a fixed-sized, short-lived memory area associated with the current method execution. 
        Allocating memory on the stack is efficient but must remain within scope. 

        stackalloc directly allocates memory on the stack, returning a pointer or 
        allowing a span to wrap the allocated memory. 
        This eliminates the need for raw pointer arithmetic when manipulating stack-allocated arrays.


        Span<int> span = stackalloc int[200];

        private static int Sum(ReadOnlySpan<int> span)
        {
            int total = 0;

            //for (int i = 0; i < span.Length; i++)
            foreach (int i in span)
                total += i;

            return total;
        }

        int result = Sum(numbers); // Pass stack memory to the method
        Console.WriteLine(result); 


        --- 2. Combining Span<T> and Unsafe Pointers

        In some cases, raw pointers are still required for advanced memory manipulation. 
        Spans can be constructed from pointers to enable safer access to manually allocated memory. 
        This is particularly useful when working with stackalloc or unmanaged memory.

        unsafe
        {
            int* ptr = stackalloc int[200];
            Span<int> numbers = new Span<int>(ptr, 200);

            for (int i = 0; i < numbers.Length; i++)
                numbers[i] = i;

            int total = Sum(numbers);
            Console.WriteLine(total);
        }


        --- 3. Unmanaged Memory with Span<T>

        Unmanaged memory resides outside the control of the GC. 
        Allocating unmanaged memory is useful for interoperability with native libraries or when working with large, long-lived datasets. 
        However, managing it safely requires attention to avoid memory leaks or access violations. 
        Spans make working with unmanaged memory safer by wrapping pointers.

        ReadOnlySpan<char> source = "The quick brown fox".AsSpan();
        IntPtr unmanagedPointer = Marshal.AllocHGlobal(source.Length * sizeof(char)); // Allocate unmanaged memory

        try
        {
            unsafe
            {
                Span<char> unmanagedSpan = new Span<char>((char*)unmanagedPointer, source.Length);
                source.CopyTo(unmanagedSpan);

                //foreach (var word in new CharSpanSplitter(unmanagedSpan))
                foreach (var word in unmanagedSpan.ToString().Split())
                {
                    Console.WriteLine(word.ToString());
                }
            }
        }
        finally
        {
            Marshal.FreeHGlobal(unmanagedPointer); // free unmanaged memory
        }

        */

        ReadOnlySpan<char> source = "The quick brown fox".AsSpan();
        IntPtr unmanagedPointer = Marshal.AllocHGlobal(source.Length * sizeof(char)); // Allocate unmanaged memory

        try
        {
            unsafe
            {
                Span<char> unmanagedSpan = new Span<char>((char*)unmanagedPointer, source.Length);
                source.CopyTo(unmanagedSpan);

                //foreach (var word in new CharSpanSplitter(unmanagedSpan))
                foreach (var word in unmanagedSpan.ToString().Split())
                {
                    Console.WriteLine(word.ToString());
                }
            }
        }
        finally
        {
            Marshal.FreeHGlobal(unmanagedPointer); // free unmanaged memory
        }
    }

    static IEnumerable<ReadOnlyMemory<char>> Split(ReadOnlyMemory<char> input)
    {
        int wordStartIndex = 0;

        for (int i = 0; i <= input.Length; i++)
        {
            if (i == input.Length || char.IsWhiteSpace(input.Span[i]))
            {
                yield return input[wordStartIndex..i];
                wordStartIndex = i;
            }
        }
    }

    private static void ProcessMemory(Memory<int> memory)
    {
        Span<int> span = memory.Span; // conversion to Span<T>
        for (int i = 0; i < span.Length; i++)
        {
            span[i] *= 2; // modify the memory
        }
    }

    // Compile-time error: Span<int> cannot be used in an async method
    //static async Task AsyncMethod(Span<int> span)
    //{
    //    await Task.Delay(1000);
    //    Console.WriteLine(span[0]);
    //}

    // Iterator method - not allowed with Span<T>
    // static IEnumerable<int> GetValues(Span<int> span)
    // {
    //     foreach (var item in span)
    //     {
    //         yield return item; // Compile-time error
    //     }
    // }

    private static int Sum(int[] nums)
    {
        int total = 0;
        foreach (var item in nums)
        {
            total += item;
        }
        return total;
    }

    private static int Sum(ReadOnlySpan<int> span)
    {
        int total = 0;

        //for (int i = 0; i < span.Length; i++)
        foreach (int i in span)
            total += i;

        return total;
    }
}