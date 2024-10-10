internal class Program
{
    private static void Main(string[] args)
    {
        /*CLR
            In simple terms, the Common Language Runtime (CLR) is like a manager for programs 
            written in certain languages (like C#, F#, Visual Basic, etc.). 
            It takes care of important tasks so that programmers don't have to handle everything themselves.

            Two key things the CLR does:

        1. Automatic Memory Management: The CLR keeps track of memory. 
            When a program needs memory, the CLR gives it. 
            When that memory is no longer needed, 
            the CLR automatically cleans it up, so the program doesn’t run out of memory 
            or crash because of memory issues.

        2. Exception Handling: If something goes wrong while a program is running 
            (like dividing by zero or accessing a file that doesn’t exist), 
            the CLR helps catch these errors, 
            so the program doesn’t completely crash and can handle the problem in a controlled way.

        The CLR does more than just automatic memory management and exception handling. 
            Here are some other important services it provides:

        3. Garbage Collection: This is part of memory management. 
            It automatically frees up memory that is no longer in use, preventing memory leaks.

        4. Type Safety: The CLR ensures that the data types used in a program are used correctly. 
            For example, if a program tries to put a string where a number should be, the CLR will catch that mistake.

        5. Security Management: The CLR enforces security policies, making sure that programs have the right permissions 
            to access system resources.

        6. Thread Management: The CLR handles multiple tasks or threads that can run in parallel in a program, 
            allowing efficient multitasking.

        7. Interoperability: It allows managed code (code that runs under CLR) to work 
            with unmanaged code (code that doesn't, like native C++ code).
        */

        /*JIT and AOT compilation
         
        C# is a managed language because when you write code in C#, 
        it gets turned into managed code. This managed code isn't the direct machine code that your computer understands.

        Instead, the C# code is first compiled into an intermediate form called Intermediate Language (IL). 
        IL is like a halfway step between the human-readable C# code and the machine code that the computer actually runs.

        Then, when you run the program, the Common Language Runtime (CLR) 
        takes that IL and converts it into machine code (like X64 or X86, depending on your computer) 
        right before it needs to be executed. 
        This process is called Just-In-Time (JIT) compilation, meaning the conversion happens at the last moment.

        Ahead-of-Time compilation is another option. Instead of waiting until the program runs to convert IL to machine code, 
        the conversion happens earlier. This is useful for things like:
        1. Making big programs start faster.
        2. Running programs on devices with limited resources.
        3. Following certain rules, like for iOS apps, which require code to be fully compiled ahead of time.

        So, JIT is the standard way the CLR runs C# code, 
        but Ahead-of-Time compilation is an alternative when it's needed for performance or platform requirements.

        How Ahead-of-Time (AOT) Compilation Works:
        
            Ahead-of-Time compilation means converting the code before running the program. 
            In this process, your C# code is compiled directly into machine code ahead of time 
            (before the program is run on your device).
            Instead of compiling into Intermediate Language (IL) and waiting until runtime to convert it to machine code, 
            the entire program is pre-compiled into machine code that matches the hardware platform (e.g., x86 or ARM).

            AOT is useful for:
                1. Faster startup: Since the machine code is already available, it doesn't need to wait for JIT compilation.
                2. Resource-constrained devices: Some devices, like mobile phones or embedded systems, have limited processing power or memory and benefit from having everything compiled upfront.
                3. Platform restrictions: Platforms like iOS don't allow JIT compilation, so apps need to be fully compiled before deployment.
        	
        How Just-In-Time (JIT) Compilation Works:
        
            Just-In-Time (JIT) compilation happens during program execution. 
            When you run a C# program, it is first compiled into Intermediate Language (IL), 
            which is platform-agnostic (i.e., not tied to any specific hardware like x86 or ARM).
            When the program starts, the CLR uses a JIT compiler to convert the IL into machine code 
            just before it’s needed for execution, piece by piece (function by function, or block by block). This happens at runtime.
            
            The process:
                The program runs, and the JIT compiler detects which parts of the program are needed at that moment.
                It then converts only those parts (e.g., functions) from IL to machine code.
                Once compiled, the machine code is stored in memory so that it doesn’t have to be recompiled again during the same program run.
        
        The container for managed code is called an assembly. 
        An assembly contains not only IL but also type information (metadata). 
        The presence of metadata allows assemblies to reference types in other assemblies without needing additional files.
         */

        /*Base Class Library
         
        A CLR always ships with a set of assemblies called a Base Class Library (BCL).

        A BCL provides core functionality to programmers, such as collections, input/output, text processing, 
        XML/JSON handling, networking, encryption, interop, concurrency, and parallel programming.

        using System;
        using System.Collections.Generic;
        using System.Linq;
        using System.Text;
        using System.Threading.Tasks; for example 

        CTRL ALT J to see object browser.
         */

        /* X86 (32-bit Architecture) && X64 (64-bit Architecture)
         
            1. Address Space: X86 architecture can address up to 4 GB of memory directly due to its 32-bit address space (2^32 = 4,294,967,296 bytes).
            This is a theoretical maximum due to the 32-bit address limitation, 
            which means that a 32-bit system can access up to 4 GB of RAM in total.
            
            2. Registers: X86 processors have 32-bit registers, which means that operations are performed using 32-bit chunks of data.
            3. Generally, 32-bit architectures are less efficient and can handle fewer simultaneous operations compared to 64-bit architectures.


            1. Address Space: X64 architecture can theoretically address up to 16 exabytes of memory (2^64 bytes), 
            though practical systems support up to a few terabytes. This allows for much larger memory addressing compared to X86.

            2. Registers: X64 processors have 64-bit registers, which can handle larger chunks of data and more registers than X86. 
            This can lead to better performance for applications that require handling large amounts of data or memory.
         */
    }
}