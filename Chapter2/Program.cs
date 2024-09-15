extern alias WidgetLib1;
extern alias WidgetLib2;

namespace Chapter2
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /* Identifiers and Keywords

            If you really want to use an identifier that clashes with a reserved keyword,
            you can do so by qualifying it with the @ prefix. For instance:

             int using = 123; // Illegal
             int @using = 123; // Legal

            The @ symbol doesn’t form part of the identifier itself. 
            So, @myVariable is the same as myVariable.

            string myVar = "";
            string @myVar = "";

            compile time error */

            /* Predefined Type Examples
             Predefined types are types that are specially supported by the compiler. 
             The int type is a predefined type for representing the set of integers that fit into 32 bits of
             memory, from −231 to 231−1, and is the default type for numeric literals within this range.

             *Here’s a list of the common predefined types in C# along with their sizes in bytes:

             byte	            1 byte (8 bits)
             sbyte	            1 byte (8 bits)
             short	            2 bytes (16 bits)
             ushort	            2 bytes (16 bits)
             int	            4 bytes (32 bits)
             uint	            4 bytes (32 bits)
             long	            8 bytes (64 bits)
             ulong	            8 bytes (64 bits)
             float	            4 bytes (32 bits)
             double	            8 bytes (64 bits)
             decimal	        16 bytes (128 bits)
             char	            2 bytes (16 bits)
             bool	            1 byte (8 bits) (varies)
             object	            4 or 8 bytes (depends on system)
             string	            Variable (depends on string length)

             */

            /* Static and Instance Members

            1. Instance Members

            Instance members belong to a specific instance (object) of a class. 
            You need to create an object of the class to access them.

            Example: A person’s age is an instance member 
            because each person (instance) has their own age.

            2. Static Members

            Static members belong to the class itself rather than to any specific instance. 
            You access them through the class name, and they are shared across all instances of the class.

            Example: A counter that counts how many people have been created could be a static member, 
            because it is shared and updated for all instances.
             */

            /* Memory Layout of a Class

            1. Fields and Their Sizes:

            int: Each int field in C# is 4 bytes (32 bits) in size. 
            So, three int fields would total: 3 ints×4 bytes/int=12 bytes

            2. Object Overhead:

            Class Instances: When you create an instance of a class, it is stored on the heap.
            This overhead includes metadata for managing the object (such as type information and garbage collection data).

            Reference Size: In a 64-bit environment, the reference to the object itself (the pointer) is typically 8 bytes, 
            though this doesn’t affect the size of the actual data fields.

            Stack vs. Heap:

            1. Stack: When a reference to the Person object is created (e.g., Person person = new Person();), 
            the reference (pointer) to the object is stored on the stack.
            2. Heap: The actual object (with its fields: Age, Height, Weight) is stored on the heap.

            Value types (e.g., int, double, float) are typically stored on the stack 
            when they are used directly (e.g., as local variables or method parameters).
            int a = 10;  // Stored on the stack

            Reference types (e.g., class) are always stored on the heap, 
            and this includes any fields inside the class, 
            even if those fields are value types.
            So when value types like int are part of a class, 
            they are stored on the heap along with the class object, 
            because the entire class instance resides on the heap.

            public class Person
            {
                public int Age;
                public int Height;
                public int Weight;
            }

            64-bit System (8-byte reference pointer)

            Memory Size:

            Stack:
            On a 64-bit system, the reference (pointer) takes up 8 bytes.

            Heap:
            The Person object on the heap is the same size as in a 32-bit system 
            since the data fields (int) are still 4 bytes each. 
            However, the reference to the object is now larger, as it's 8 bytes instead of 4.

            Total = 20 bytes (8 + 4 + 4 + 4)
            */

            /* Alignment and Padding

            1. Alignment:
            Alignment refers to how data is arranged in memory to match 
            the architecture's word size (usually 4 or 8 bytes on modern systems). 

            For example:
            On a 32-bit system, an int (4 bytes) must be stored at a memory address divisible by 4.
            On a 64-bit system, a double (8 bytes) must be stored at an address divisible by 8.

            2. Padding:
            Padding is extra space inserted between fields (data members) to ensure that 
            each field starts at a properly aligned memory address, according to the alignment rules. 
            This helps in achieving faster access to the data at the cost of extra memory.

            Example N1:

            Consider a class or struct with an int (4 bytes) and a byte (1 byte).
            public class Example
            {
                public int a;   // 4 bytes
                public byte b;  // 1 byte
            }

            Without any padding, this structure would be 5 bytes in size. 
            However, alignment rules dictate that the int field must be aligned to a 4-byte boundary.

            Also, the structure as a whole must be aligned to its largest member’s alignment (in this case, 4 bytes).

            To satisfy these rules, the compiler adds padding after the byte field (b) 
            so that the size of the structure is a multiple of 4 bytes. 
            The extra 3 bytes of padding are added after b.

            Memory Layout:
            +-------+-------+-------+-------+
            |   a   |  b    |  padding       | (12 bytes in total: 4 bytes for int, 1 for byte, 3 bytes padding)
            +-------+-------+---------------+

            a starts at byte 0 and takes 4 bytes.
            b starts at byte 4 and takes 1 byte.
            3 bytes of padding are added after b to ensure the structure size is a multiple of 4 bytes.

            Example N2:

            public struct ComplexExample
            {
                public int x;      // 4 bytes
                public byte y;     // 1 byte
                public double z;   // 8 bytes
            }

            x (int): 4 bytes, starts at byte 0, aligned to 4 bytes.
            y (byte): 1 byte, starts at byte 4 (no padding yet).
            To align z (double) to an 8-byte boundary, 3 bytes of padding are added after y to make it start at byte 8.
            z (double): 8 bytes, starts at byte 8, aligned to 8 bytes.

            +-------+-------+-------+-------+-------+-------+-------+-------+
            |   x   |  y    |  padding       |    z (double, 8 bytes)       |
            +-------+-------+-------+-------+-------+-------+-------+-------+
            Total size: 16 bytes (4 bytes for x, 1 byte for y, 3 bytes padding, 8 bytes for z).

            Why Do We Need Alignment?

            Alignment improves performance. Modern CPUs are optimized for reading and writing data at aligned memory addresses. 
            Accessing unaligned data can result in slower performance due to additional memory operations.
            Padding is necessary to maintain the correct alignment for fields within a structure or class.
            */

            /* Types and Conversions

            A conversion always creates a new value from an existing one. 
            Conversions can be either implicit or explicit:

            implicit conversions happen automatically, 
            and explicit conversions require a cast

            In the following example, we implicitly convert an int to a long type 
            (which has twice the bit capacity of an int) 
            and explicitly cast an int to a short type 
            (which has half the bit capacity of an int):

            int x = 12345; // int is a 32-bit integer
            long y = x; // Implicit conversion to 64-bit integer
            short z = (short)x; // Explicit conversion to 16-bit integer

            Implicit conversions are allowed when both of the following are true:
            1. The compiler can guarantee that they will always succeed.
            2. No information is lost in conversion.

            Conversely, explicit conversions are required when one of the following is true:
            1. The compiler cannot guarantee that they will always succeed.
            2. Information might be lost during conversion.
            */

            /* Value types
            The content of a value type variable is simply a value. 
            For example, the content of the int, is 32 bits of data.

            You can define a custom value type with the struct keyword.
            public struct Point 
            { 
                public int X; 
                public int Y; 
            }

            ---The assignment of a value-type instance always copies the instance. 
            For example:

            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1; // Assignment causes copy

            Console.WriteLine (p1.X); // 7
            Console.WriteLine (p2.X); // 7

            p1.X = 9; // Change p1.X

            Console.WriteLine (p1.X); // 9
            Console.WriteLine (p2.X); // 7

               Stack
            +---------+
            |  p1     |
            |  X = 9  |
            |         |
            |  p2     |
            |  X = 7  |
            +---------+
             */

            /* Reference types
             A reference type is more complex than a value type, having two parts: 
             an object and the reference to that object.

            public class Point 
            { 
                public int X; 
                public int Y; 
            }

            Point class
            Reference -------------------------------> Object
                                                        X
                                                        Y
            ---Assigning a reference-type variable copies the reference, not the object instance. 
            This allows multiple variables to refer to the same object

            Point p1 = new Point();
            p1.X = 7;
            Point p2 = p1; // Copies p1 reference

            Console.WriteLine (p1.X); // 7
            Console.WriteLine (p2.X); // 7

            p1.X = 9; // Change p1.X

            Console.WriteLine (p1.X); // 9
            Console.WriteLine (p2.X); // 9

            p1 
            reference -------------------object meta data
                                  |------
            p2             |-------
            reference ------
            */

            /* 8 and 16 Bit Integral Types

            The 8- and 16-bit integral types are byte, sbyte, short, and ushort. 
            These types lack their own arithmetic operators.

            This can cause a compile-time error when trying to assign the result 
            back to a small integral type:

            short a = 1, b = 2;
            short c = a + b; // compile time error

            In this case, x and y are implicitly converted to int so 
            that the addition can be performed.
            This means that the result is also an int, which cannot be implicitly cast
            back to a short (because it could cause loss of data). To make this compile, 
            you must add an explicit cast:

            short z = (short) (x + y); // OK
            */

            /* Stack

            The stack is a block of memory for storing local variables and parameters. 
            The stack logically grows and shrinks as a method or function is entered and exited. 
            Consider the following method:

            static int Factorial (int x)
            {
                if (x == 0) return 1;
                return x * Factorial (x-1);
            }

            This method is recursive, meaning that it calls itself. 
            Each time the method is entered, a new int is allocated on the stack, 
            and each time the method exits, the int is deallocated.

            */

            /* Heap

            Whenever a new object is created, it is allocated on the heap, 
            and a reference to that object is returned.

            During a program’s execution, the heap begins filling up as new objects are created. 
            The runtime has a garbage collector that periodically deallocates objects from the heap, 
            so your program does not run out of memory.

            An object is eligible for deallocation as soon as 
            it’s not referenced by anything that’s itself “alive.”

            1. In the following example, we begin by creating a StringBuilder object referenced
            by the variable ref1 and then write out its content. 
            That StringBuilder object is then immediately eligible for garbage collection,
            because nothing subsequently uses it.

            using System;
            using System.Text;

            StringBuilder ref1 = new StringBuilder ("object1");
            Console.WriteLine (ref1);
            // The StringBuilder referenced by ref1 is now eligible for GC.

            Then, we create another StringBuilder referenced by variable ref2 and copy that
            reference to ref3. Even though ref2 is not used after that point, ref3 keeps the
            same StringBuilder object alive—ensuring that 
            it doesn’t become eligible for collection until we’ve finished using ref3:

            StringBuilder ref2 = new StringBuilder ("object2");
            StringBuilder ref3 = ref2;
            // The StringBuilder referenced by ref2 is NOT yet eligible for GC.
            Console.WriteLine (ref3); // object2

            NOTE: The heap also stores static fields. Unlike objects allocated on the heap 
            (which can be garbage-collected), these live until the process ends.
            */

            /* The Ref modifier

            To pass by reference, C# provides the ref parameter modifier. 
            In the following example, p and x refer to the same memory locations:

            int x = 8;
            Foo (ref x); // Ask Foo to deal directly with x
            Console.WriteLine (x); // x is now 9

            static void Foo (ref int p)
            {
                p = p + 1; // Increment p by 1
                Console.WriteLine (p);
            }
            */

            /* The Out modifier

            An out argument is like a ref argument except for the following:
            • It need not be assigned before going into the function.
            • It must be assigned before it comes out of the function.

            The out modifier is most commonly used to get multiple return values 
            back from a method.

            string a, b;
            Split ("Stevie Ray Vaughn", out a, out b);

            Console.WriteLine (a); // Stevie Ray
            Console.WriteLine (b); // Vaughn

            void Split (string name, out string firstNames, out string lastName)
            {
                int i = name.LastIndexOf (' ');
                firstNames = name.Substring (0, i);
                lastName = name.Substring (i + 1);
            }
            */

            /* The In modifier

            The in modifier in C# is used for passing arguments by reference, like the ref keyword, 
            but with one important difference: the value cannot be modified inside the method. 

            This helps improve performance when you're passing large value types (like big structs) 
            by avoiding the overhead of copying the value.

            Why Use the in Modifier?

            1. Without in: When passing a large value type (e.g., a struct), 
            C# normally creates a copy of the value to pass into the method, 
            which can be slow for large data.

            2. With in: Instead of copying, the method reads directly from the original value. 
            However, modification is not allowed—the method can only read the value.

            struct SomeBigStruct
            {
                public int Value1;
                public int Value2;
            }

            void Foo(SomeBigStruct a)
            {
                // Here, a copy of 'a' is made before the method uses it.
                Console.WriteLine(a.Value1);
            }

            void Foo(in SomeBigStruct a)
            {
                // 'a' is passed by reference, no copy is made.
                Console.WriteLine(a.Value1);
                // a.Value1 = 10; // Not allowed! It would cause a compile-time error.
            }
            */

            /* Ref Locals

            A somewhat esoteric feature of C# is that you can define a local variable that references
            an element in an array or field in an object (from C# 7):

            int[] numbers = { 0, 1, 2, 3, 4 };
            ref int numRef = ref numbers[2];

            In this example, numRef is a reference to numbers[2]. 
            When we modify numRef, we modify the array element:

            numRef *= 10;
            Console.WriteLine(numRef);          // 20
            Console.WriteLine(numbers[2]);      // 20
            */

            /* Ref Returns

            You can return a ref local from a method. This is called a ref return:

            public class Test
            {
                static string x = "Old Value";
                static ref string GetX() => ref x; // This method returns a ref

                public static void TestMain()
                {
                    ref string xRef = ref GetX(); // Assign result to a ref local
                    Console.WriteLine($"x: {x}");

                    xRef = "New Value";
                    Console.WriteLine($"x: {x}"); // New Value
                }
            }
            */

            /* Name hiding
            If the same type name appears in both an inner and an outer namespace, 
            the inner name wins. To refer to the type in the outer namespace, you must qualify its name:

            namespace Outer
            {
                class Foo { }

                namespace Inner
                {
                    class Foo { }
                    class Test
                    {
                        Foo f1; // = Outer.Inner.Foo
                        Outer.Foo f2; // = Outer.Foo
                    }
                }
            }

            All type names are converted to fully qualified names at compile time. 
            Intermediate Language (IL) code contains no unqualified or partially qualified names.
            */

            /* Advanced Namespace Features

            ---Extern

            The extern alias feature in C# helps when you need to reference two different assemblies (libraries) 
            that contain types with the same name and namespace. 
            This is a rare situation, but it can happen if you're using two versions of the same library 
            or different libraries that happen to define the same types.

            You have two libraries:
            
            Widget1.dll contains - 
            namespace Widgets
            {
                public class Widget { }
            }

            Widget2.dll contains - 
            namespace Widgets
            {
                public class Widget { }
            }

            Both libraries have a class called Widget in the same namespace Widgets. 
            If you try to reference both libraries in the same project, 
            the compiler won't know which Widget class you're referring to, 
            because the namespace and class names are identical.

            Solution: extern alias
            
            You can solve this by using extern alias to give each library a different alias. 
            This way, you can tell the compiler exactly which Widget class you're referring to.

            <ItemGroup>
	        	<ProjectReference Include="..\Widget1\Widget1.csproj">
	        		<Aliases>WidgetLib1</Aliases>
	        	</ProjectReference>
                
	        	<ProjectReference Include="..\Widget2\Widget2.csproj">
	        		<Aliases>WidgetLib2</Aliases>
	        	</ProjectReference>
	        </ItemGroup>

            in your C# code, you can use the extern keyword to specify which alias you want to use for each library.
            extern alias WidgetLib1;
            extern alias WidgetLib2;

            // Use the Widget class from Widgets1.dll
            WidgetLib1::Widgets.Widget widget1 = new WidgetLib1::Widgets.Widget();

            // Use the Widget class from Widgets2.dll
            WidgetLib2::Widgets.Widget widget2 = new WidgetLib2::Widgets.Widget();
            */
        }
    }
}
