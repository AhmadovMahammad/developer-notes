﻿using System.Text;

namespace Chapter3;

internal class Program
{
    private static void Main(string[] args)
    {
        /* (De)constructor
        
        A deconstructor in C# is a method that does the opposite of a constructor. 
        While a constructor sets values to fields when an object is created, 
        a deconstructor allows you to take those fields out and assign them back to individual variables.

        1. Constructor and Fields

        In the Rectangle class, the constructor takes two parameters—width and height 
        and assigns them to the class fields Width and Height.
         
        public class Rectangle
        {
            public readonly float Width, Height;

            public Rectangle(float width, float height)
            {
                Width = width;
                Height = height;
            }
        }

        When you create an instance of Rectangle, you pass in these values:
        var rect = new Rectangle(3, 4);

        2. Now, the deconstructor (method named Deconstruct) does the reverse. 
        It takes the values stored in Width and Height and assigns them 
        to the variables passed to it as out parameters:

        public void Deconstruct(out float width, out float height)
        {
            width = Width;
            height = Height;
        }

        3. How Deconstruction Works

        When you write this line: (float width, float height) = rect;

        1. The special syntax tells the compiler: "I want to deconstruct this object".
        2. It looks for a method in Rectangle called Deconstruct that has the matching number of out parameters 
        as the variables on the left side of the assignment (width, height).
        3. If the method is found, it calls rect.Deconstruct(out width, out height), 
        so width and height get their values from the rectangle’s Width and Height.


        This is equivalent to manually calling:
        float width, height;
        rect.Deconstruct(out width, out height);

        */

        /* Init-only properties
         
        // From C# 9, you can declare a property accessor with init instead of set:

        public class Note
        {
            public int Pitch { get; set; }
            public decimal Duration { get; init; }
        }

        //These init-only properties act like read-only properties, 
        except that they can also be set via an object initializer:

        //var note = new Note { Duration = 210.5M };
        //note.Pitch = 20;
        //note.Duration = 100; // Compile time error

        The alternative to init-only properties is to have read-only properties that 
        you populate via a constructor:

        public class Note
        {
            public int Pitch { get; }
            public int Duration { get; }
            
            public Note (int pitch = 20, int duration = 100)
            {
                Pitch = pitch; 
                Duration = duration;
            }
        }
        */

        /* Implementing an indexer
         
        To write an indexer, define a property called this, specifying the arguments in square brackets:

        Here’s how we could use this indexer:
        Sentence s = new Sentence();

        Console.WriteLine(s[3]);
        s[3] = "kangaroo";
        Console.WriteLine(s[3]); // kangaroo

        A type can declare multiple indexers, each with parameters of different types. 
        An indexer can also take more than one parameter:

        */

        /* Finalizers
         
        Finalizers in C# are special methods that execute right before the garbage collector reclaims 
        the memory of an object that is no longer referenced. 

        Finalizers give the object a chance to clean up resources that are not managed by the CLR, 
        such as file handles, database connections, or unmanaged memory.
         
        ---Syntax of a Finalizer
        A finalizer looks like a method but is named with a tilde (~) followed by the class name. 
        Here's a basic example:

        public class FileHandler
        {
            // Finalizer to clean up unmanaged resource
            ~FileHandler()
            {
                Console.WriteLine("Finalizer called: File handle closed.");
            }
        }

        NOTE: This finalizer will run when the object is about to be destroyed by the garbage collector.

        ---When to Use a Finalizer
        Finalizers are generally used to clean up unmanaged resources. 
        Unmanaged resources are resources not managed by the CLR’s garbage collector, such as:

        1. File handles
        2. Network connections
        3. Database connections
        4. Unmanaged memory (memory allocated outside the .NET framework)


        Let's say you're working with a file, 
        and you want to make sure the file handle is properly closed when the object is destroyed. 
        Here's how you can use a finalizer to close the file:

        public class FileHandler
        {
            private readonly IntPtr fileHandle;  // Unmanaged resource (file handle)

            // Finalizer to clean up unmanaged resource
            ~FileHandler()
            {
                CloseFile(fileHandle);
                Console.WriteLine("Finalizer called: File handle closed.");
            }

            public FileHandler(string filePath)
            {
                fileHandle = OpenFile(filePath);
            }

            private IntPtr OpenFile(string path)
            {
                // Pretend to open a file and return a handle (IntPtr represents an unmanaged resource)
                Console.WriteLine("File opened.");
                return new IntPtr(123);  // Simulated handle
            }

            private void CloseFile(IntPtr handle)
            {
                // Pretend to close the file
                Console.WriteLine("File closed.");
            }
        }

        When an object is no longer referenced, it becomes eligible for garbage collection. 
        However, finalizers don't run immediately when the object becomes eligible; 
        they run before the object is reclaimed by the garbage collector. 
        Finalizers are non-deterministic, meaning you can't predict exactly when they will run.

        Important Notes:

        1. Do not rely on finalizers for managed resources: 
        Managed resources (like string, List<T>, or other CLR objects) are automatically handled by the garbage collector. 
        Use IDisposable and the using pattern to clean up managed resources instead.


        2. Performance considerations: 
        Finalizers can impact performance because the garbage collector has to run the finalizer before cleaning up the memory. 
        For this reason, finalizers should be used sparingly, and only when absolutely necessary (for unmanaged resources).

        //var fileHandler = new FileHandler(Path.Combine(
        //    Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
        //    "risk",
        //    "pandasettings.xml"
        //    ));
        //fileHandler.Dispose();
        */

        /* The as operator
        
        The as operator performs a downcast that evaluates to null 
        (rather than throwing an exception) if the downcast fails:

        Asset a = new Asset();
        Stock s = a as Stock; // s is null; no exception thrown
         
        Without such a test, a cast is advantageous, because if it fails, 
        a more helpful exception is thrown. We can illustrate by comparing the following two lines of code:

        int shares = ((Stock)a).SharesOwned; // Approach #1
        int shares = (a as Stock).SharesOwned; // Approach #2
        
        If a is not a Stock, the first line throws an InvalidCastExcep tion, 
        which is an accurate description of what went wrong.
        
        The second line throws a NullReferenceException, which is ambiguous. 
        Was a not a Stock, or was a null?

        */

        /* New versus override
         
        public class A { public int Counter = 1; }
        public class B : A { public int Counter = 2; }
        
        The Counter field in class B is said to hide the Counter field in class A. Usually, this
        happens by accident, when a member is added to the base type after an identical
        member was added to the subtype. For this reason, the compiler generates a warning.

        Occasionally, you want to hide a member deliberately, 
        in which case you can apply the new modifier to the member in the subclass.

        public class A { public int Counter = 1; }
        public class B : A { public new int Counter = 2; }

        Consider the following class hierarchy:
        public class BaseClass
        {
            public virtual void Foo() 
            { 
                Console.WriteLine ("BaseClass.Foo"); 
            }
        }
        public class Overrider : BaseClass
        {
            public override void Foo() 
            { 
                Console.WriteLine ("Overrider.Foo"); 
            }
        }
        public class Hider : BaseClass
        {
            public new void Foo()
            { 
                Console.WriteLine ("Hider.Foo"); 
            }
        }

        BaseClass base1 = new Overrider(); // Object is Overrider, reference is BaseClass
        BaseClass base2 = new Hider();     // Object is Hider, reference is BaseClass

        Overrider over = new Overrider();  // Object and reference both are Overrider
        Hider hider = new Hider();         // Object and reference both are Hider

        base1.Foo();  // Output: Overrider.Foo  -> `override` method is called
        base2.Foo();  // Output: BaseClass.Foo  -> `BaseClass.Foo` is called because `new` hides the method

        over.Foo();   // Output: Overrider.Foo  -> Calls `Overrider.Foo` directly
        hider.Foo();  // Output: Hider.Foo      -> Calls `Hider.Foo` directly

        KEY CONCEPTS:

        1. virtual: The method in BaseClass is marked virtual, 
        which means it can be overridden in a derived class.

        2. override: In Overrider, override means this method replaces the virtual method from BaseClass. 
        When you call Foo on an Overrider object, even if it's referenced as a BaseClass, 
        the Overrider.Foo method is called.

        3. new: In Hider, new hides the method from the base class rather than overriding it. 
        When calling Foo on a Hider object referenced as a BaseClass, 
        the original BaseClass.Foo method is called, not Hider.Foo.

        With new (in Hider), the new keyword hides the base method but does not override it. 
        
        When you call Foo on a BaseClass reference to a Hider object, 
        the method in the base class (BaseClass.Foo) is called. 
        This happens because new breaks the polymorphic behavior and 
        tells the compiler to treat the method in Hider as a separate, non-overriding method.

        */

        /* Sealing a (Property, Class, Method)

        ---Example 1. Sealing a Property

        public class Building
        {
            public virtual decimal Liability { get; set; }
        }

        public class House : Building
        {
            public decimal Mortgage { get; set; }
            public sealed override decimal Liability
            {
                get => Mortgage;
                set => Mortgage = value;
            }

            public House(decimal mortgage)
            {
                Mortgage = mortgage;
            }
        }

        public class Villa : House
        {
            public Villa(decimal mortgage) : base(mortgage) { }

            // The following line would cause an error:
            //public override decimal Liability { get => base.Liability; set => base.Liability = value; }
        } 

        In this example:

        The Building class defines a Liability property that is virtual, meaning it can be overridden.
        The House class overrides the Liability property and 
        uses the sealed keyword to prevent further overriding in subclasses.
        If you try to override Liability in the Villa class (which inherits from House), 
        you'll get a compilation error.


        ---Example 2. Sealing a Class
        public sealed class Apartment
        {
            public decimal Rent { get; set; }
        
            public Apartment(decimal rent)
            {
                Rent = rent;
            }
        
            public void ShowRent()
            {
                Console.WriteLine($"The rent is {Rent}.");
            }
        }
        
        // Attempting to inherit from Apartment will cause a compilation error
        // The following line would cause an error:
        // public class Penthouse : Apartment { }


        */

        /* The GetType Method and typeof Operator
         
        All types in C# are represented at runtime with an instance of System.Type. 
        There are two basic ways to get a System.Type object:

        1. Call GetType on the instance
        -The GetType() method is used to obtain the runtime type of an instance. 
        It is called on an object and provides details about the object’s type at runtime.
        -It is evaluated when the program runs, so the exact type is determined during execution.

        2. Use the typeof operator on a type name
        -The typeof operator is used to get the System. Type object for a type name at compile-time. 
        It doesn’t require an instance to be called and provides type information based on the type definition.
        -This is evaluated statically (during compilation), which means the type is known at compile time.

        Examples:

        var point = new Point();

        // Example 1: Using GetType on an instance
        Console.WriteLine(point.GetType().Name);                // Output: Point
        Console.WriteLine(point.GetType().FullName);            // Output: Namespace.Point (if namespace exists)

        // Example 2: Using typeof with the type name
        Console.WriteLine(typeof(Point).Name);                  // Output: Point
        Console.WriteLine(typeof(Point).FullName);              // Output: Namespace.Point (if namespace exists)

        // Example 3: Comparing GetType and typeof
        Console.WriteLine(point.GetType() == typeof(Point));    // True

        // Note: without name, it prints Fullname
        */

        /* Object member listing
         
        In C#, all types (classes, structs, etc.) automatically inherit from the Object class, 
        which is the root of the type hierarchy. 
        This means every type in C# has access to the members of the Object class. 
        Here is an explanation of the key methods of Object and how you can use them:
         
        1. GetType()
        2. Equals(object obj)
        3. Equals(object objA, object objB)
        4. ReferenceEquals(object objA, object objB)
        5. GetHashCode()
        6. ToString()
        7. Finalize()
        8. MemberwiseClone()

        1. GetType()
        Description: Returns the System.Type of the current instance. 
        It gives information about the type of the object at runtime.

        object obj = new object();
        Console.WriteLine(obj.GetType());  // Output: System.Object

        2. Equals(Object obj)
        Description: Determines whether the current instance is equal to another object. 
        The default implementation checks for reference equality (i.e., whether both objects are the same instance).

        Override: You can override Equals in your custom class to compare values or fields instead of reference equality.

        object obj1 = new object();
        object obj2 = obj1;
        Console.WriteLine(obj1.Equals(obj2));  // Output: True (same instance)

        3. Equals(object objA, object objB) (Static)
        Description: Compares two objects for equality. 
        By default, it checks reference equality unless overridden by one of the objects.

        object obj1 = new object();
        object obj2 = new object();
        Console.WriteLine(Object.Equals(obj1, obj2));  // Output: False (different instances)

        4. ReferenceEquals(object objA, object objB) (Static)
        Description: Determines whether two object references refer to the same instance in memory. 
        This method does not consider value equality or overridden Equals methods.

        5. GetHashCode()
        Description: Returns a hash code for the object. 
        It’s often used in hashing algorithms and data structures like hash tables. 
        The default GetHashCode() is based on the object’s reference.

        object obj = new object();
        Console.WriteLine(obj.GetHashCode());  // Outputs a unique hash code
     
        6. ToString()

        7. Finalize()

        8. MemberwiseClone()
        Description: Creates a shallow copy of the current object. 
        A shallow copy means it copies the values of the fields, 
        but if any of those fields are references to objects, 
        it only copies the reference and not the actual object (hence shallow).

        public class Person
        {
            public string Name { get; set; }
        }
        
        Person p1 = new Person { Name = "John" };
        Person p2 = (Person)p1.MemberwiseClone();  // p2 is a shallow copy of p1
        */

        /* Differences between Equals(), Object.Equals(), and ReferenceEquals() in C#
         
        1. Equals(object obj) (Instance Method)

        The Equals() method is meant to compare content or state of objects. 
        It can be overridden by classes to provide custom logic for comparing the values of objects.

        By default, if Equals() is not overridden, it behaves like ReferenceEquals() 
        and compares object references (memory locations). 
        But for many built-in types (e.g., string, int), Equals() is overridden to compare the values.

        public class Car
        {
            public string Model { get; set; } = string.Empty;
            public int Year { get; set; }

            public override bool Equals(object? obj)
            {
                if (obj is Car other)
                {
                    return Model == other.Model && Year == other.Year;
                }

                return false;
            }

            public override int GetHashCode() => base.GetHashCode();
        }

        Car car1 = new() { Model = "Tesla", Year = 2021 };
        Car car2 = new() { Model = "Tesla", Year = 2021 };
        Car car3 = new() { Model = "Ford", Year = 2020 };

        Console.WriteLine(car1.Equals(car2));  // True (same values)
        Console.WriteLine(car1.Equals(car3));  // False (different values)
        
    
        2. Object.Equals(object objA, object objB) (Static Method)
        This static method provides a null-safe comparison at first, calling Equals() on the first object (objA).


        ---If either objA or objB is null, it handles that properly.
        * Return false, if one is null but not the other.
        * Return true if both are null.

        ---Otherwise, it calls overriden Equals method

        Car car1 = new() { Model = "Tesla", Year = 2021 };
        Car car2 = new() { Model = "Tesla", Year = 2021 };
        Car car3 = new() { Model = "Ford", Year = 2020 };
        Car? car4 = null;

        Console.WriteLine(Object.Equals(car1, car2));  // True (same values)
        Console.WriteLine(Object.Equals(car1, car4));  // False (one is null)
        Console.WriteLine(Object.Equals(car4, car4));  // True (both are null)
         

        3. ReferenceEquals(object objA, object objB) (Static Method)
        ReferenceEquals() checks if two objects are the exact same instance in memory, regardless of their content. 
        It always returns true if the two references point to the same memory location, even if they have the different values.

        Car car1 = new() { Model = "Tesla", Year = 2021 };
        Car car2 = new() { Model = "Tesla", Year = 2021 };
        Car car3 = new() { Model = "Ford", Year = 2020 };
        Car car5 = car1;        // Assigning the same reference

        Console.WriteLine(ReferenceEquals(car1, car2));  // False (different instances)
        Console.WriteLine(ReferenceEquals(car1, car5));  // True (same reference)
        Console.WriteLine(ReferenceEquals(car1, car3));  // False (different instances)

        Even though car1 and car2 have the same values, ReferenceEquals() returns false because they are different instances. 
        car5 is assigned to the same reference as car1, so ReferenceEquals() returns true.

        KEY CONCEPTS:

        1. Equals() compares values (if overridden).
        2. Object.Equals() is a null-safe comparison.
        3. ReferenceEquals() checks if two variables point to the exact same memory.
        */

        /* Structs in C#: Full Explanation
        
        A struct in C# is a value type and is often used for lightweight objects 
        where memory efficiency and performance are crucial. 
        While similar to classes in many ways, structs differ significantly in their behavior and use cases.

        public struct Point
        {
            public int X;
            public int Y;

            public Point(int x, int y)
            {
                X = x;
                Y = y;
            }
        }

        ---Key Differences Between Structs and Classes

        1. Value Type vs Reference Type:
        A struct is a value type, meaning that it is stored on the stack (unless it's part of a reference type). 
        When you assign a struct to another variable, it copies the entire object, including its data.

        A class is a reference type. A class instance is stored on the heap, and when you assign it to another variable, 
        it passes the reference (or pointer) to the object, not a copy of the object.

        2. Inheritance
        Structs do not support inheritance, except for deriving implicitly from System.ValueType, which is itself derived from Object. 
        This means you cannot create a struct hierarchy or use polymorphism with structs.

        3. Default Constructors:
        Structs have an implicit, parameterless constructor that initializes all fields to their default values (e.g., integers to 0, booleans to false).
        Classes can define parameterless constructors with custom logic, but structs cannot.

        ---Why Use Structs?

        1. You want to represent small, lightweight objects.
        2. You want to avoid heap allocations for performance reasons.
        3. You want value semantics. For example, when copying a struct, you want a copy of the data, not a reference.

        Examples of good use cases for structs are points in 2D or 3D space, rectangles, complex numbers, and colors.

        ---Struct Construction Semantics

        1. Parameterless Constructor: Every struct has an implicit parameterless constructor that sets all fields to their default values (e.g., integers to 0).
        2. Custom Constructors: When you define a constructor for a struct, every field must be explicitly assigned. 
        You cannot leave a field unassigned, and you cannot use field initializers.

        ---Example of Struct vs Class

        StructPoint sp1 = new StructPoint { X = 1, Y = 2 };
        StructPoint sp2 = sp1;  // Copies the value (not a reference)
        sp2.X = 10;
        Console.WriteLine($"Struct: sp1.X = {sp1.X}, sp2.X = {sp2.X}");  // sp1.X remains 1

        ClassPoint cp1 = new ClassPoint { X = 1, Y = 2 };
        ClassPoint cp2 = cp1;  // Copies the reference
        cp2.X = 10;
        Console.WriteLine($"Class: cp1.X = {cp1.X}, cp2.X = {cp2.X}");  // cp1.X changes to 10

        */

        /* Explicit Interface Implementation
         
        Implementing multiple interfaces can sometimes result in a collision between member signatures. 
        You can resolve such collisions by explicitly implementing an interface member. 
        Consider the following example:

        Widget w = new Widget();
        w.Foo();            // Widget's implementation of I1.Foo
        ((i1)w).Foo();      // Widget's implementation of I1.Foo
        ((i2)w).Foo();      // Widget's implementation of I2.Foo
        if (w is i2 i2)
            i2.Foo();       // Widget's implementation of I2.Foo
        */

        /* Enum Conversions
         
        An enum is a special value type that lets you specify a group of named numeric constants.
        For example:
        public enum BorderSide { Left, Right, Top, Bottom }
         
        We can use this enum type as follows:
        BorderSide topSide = BorderSide.Top;
        bool isTop = (topSide == BorderSide.Top); // true

        Each enum member has an underlying integral value. These are by default:
        • Underlying values are of type int.
        • The constants 0, 1, 2... are automatically assigned, in the declaration order of the enum members.

        You can specify an alternative integral type, as follows:
        public enum BorderSide : byte { Left, Right, Top, Bottom }


        ---You can convert an enum instance to and from its underlying integral value with an explicit cast:

        byte i = (byte)BorderSide.Left;
        BorderSide side = (BorderSide)i;

        Console.WriteLine(i);
        Console.WriteLine(side);
        
        var enumValuesAsEnumerator = Enum.GetValues(typeof(BorderSide)).GetEnumerator();
        while (enumValuesAsEnumerator.MoveNext())
        {
            var cur = enumValuesAsEnumerator.Current;
            Console.WriteLine((byte)cur);
        }

        ---You can also explicitly cast one enum type to another. 
        Suppose that Horizontal Alignment is defined as follows:

        A translation between the enum types uses the underlying integral values

        //HorizontalAlignment h = (HorizontalAlignment)BorderSide.Right;
        // same as:
        //HorizontalAlignment h1 = (HorizontalAlignment)(BorderSide)2;
        // same as:
        //HorizontalAlignment h = (HorizontalAlignment)(int)BorderSide.Right;

        BorderSide side = (BorderSide)1234567890;
        bool sideIsDefinedFromBorder = Enum.IsDefined(typeof(BorderSide), side);
        Console.WriteLine($"{nameof(sideIsDefinedFromBorder)}: {sideIsDefinedFromBorder}");
        */

        /* Bitwise Operators
         
        Bitwise operators in C# work at the binary level on integers and are used to manipulate individual bits. 
        These operators are useful for tasks that involve binary data manipulation, 
        such as low-level hardware programming, encryption, and optimizing performance. 
        Here’s a breakdown of bitwise operators and how they work:

        AND (&), OR (|), XOR (^)
        NOT (~), Left Shift (<<), Right Shift (>>)

        1. AND (&) Operator
        The bitwise AND operator compares each bit of two numbers and 
        returns 1 only if both bits are 1; otherwise, it returns 0.

        A	B	A & B
        0	0	  0
        0	1	  0
        1	0	  0
        1	1	  1

        To calculate the bitwise AND (&) of two integers, 
        you compare their binary representations bit by bit. 
        For each pair of corresponding bits, the result is 1 only if both bits are 1; otherwise, it's 0.

         int a = 6;  // 6 in binary: 110
         int b = 3;  // 3 in binary: 011
         int result = a & b; // result: 2 (in binary: 010)
         Console.WriteLine(result); // Output: 2

        2. OR (|) Operator
        The bitwise OR operator compares each bit of two numbers and 
        returns 1 if at least one of the bits is 1; otherwise, it returns 0.

        int a = 6;  // 6 in binary: 110
        int b = 3;  // 3 in binary: 011
        int result = a | b; // result: 7 (in binary: 111)
        Console.WriteLine(result); // Output: 7

        3. XOR (^) Operator
        The bitwise XOR (exclusive OR) operator compares each bit of two numbers and 
        returns 1 if only one of the bits is 1; otherwise, it returns 0.

        int a = 6;  // 6 in binary: 110
        int b = 3;  // 3 in binary: 011
        int result = a ^ b; // result: 5 (in binary: 101)
        Console.WriteLine(result); // Output: 5

        4. Left Shift (<<) Operator
        The left shift operator shifts all bits of its first operand to the left 
        by the number of positions specified by its second operand. 
        It essentially multiplies the number by powers of two.

        int a = 6;  // 6 in binary: 0000 0110
        int b = 7;  // 7 in binary: 0000 0111

        int result = a << 1;  // result: 12 (in binary: 0000 1100, shifted one bit left)
        int result2 = b << 1; // result: 12 (in binary: 0001 1100, shifted two bits left)
        Console.WriteLine(result);  // Output: 12
        Console.WriteLine(result2); // Output: 28

        ---What Happens If We Go Past 8 Bits?
        When you're shifting bits left by more than the width of the system (e.g., 8 bits in an 8-bit system), 
        any bits that exceed the width get discarded, and it becomes 0 in that case.

        However, if you're in a larger system like a 32-bit or 64-bit system, 
        shifting left by 8 bits will still yield a valid number 
        because those extra bits are within the width of the system. Let's break it down:

        Example: Shifting 7 Left by 8 Bits
        Original 7 in binary (8-bit): 0000 0111
        When shifted left by 8 bits:

        1. 32-bit system: You get something like this:
        0000 0111 << 8  = 0000 0111 0000 0000 = 1792 in decimal

        2. 8-bit system: The result becomes 0 because all the bits are shifted out of the 8-bit range:
        0000 0111 << 8  = 0000 0000 (all bits discarded)

        5. Right Shift (>>) Operator
        The right shift operator shifts all bits of its first operand to the right 
        by the number of positions specified by its second operand. 
        It essentially divides the number by powers of two.

        int a = 6;  // 6 in binary: 0000 0110
        int result = a >> 1; // result: 3 (in binary: 0000 0011, shifted one bit right)
        Console.WriteLine(result); // Output: 3

        */

        /* MSB and LSB
         
        The terms "most significant bit" (MSB) and "least significant bit" (LSB) are used 
        to describe the importance or position of bits within a binary number.

        1. Most Significant Bit (MSB)
        * The MSB is the bit in the binary number with the highest place value.
        * In a binary number, it is the leftmost bit.
        * It plays a key role in determining the magnitude (value) of the number.

        2. Least Significant Bit (LSB)
        * The LSB is the bit with the lowest place value.
        * It is the rightmost bit in the binary number.
        * It contributes the smallest value to the overall number.
          For example, in an 8-bit binary number, the LSB represents 2^0 (which is 1).


        Example Breakdown:
        Let’s consider the 8-bit binary number 1101 1010:
        
        1. Most Significant Bit (MSB): 
        The leftmost 1. In an unsigned 8-bit number, this 1 represents 2^7 = 128.
        2. Least Significant Bit (LSB): 
        The rightmost 0. This 0 represents 2^0 = 0.
        */

        /*Bitwise Manupilation related questions
         */

        /*1. Question : Check if a Number is a Power of 2
        
        Concept : A number is a power of 2 if it has exactly one 1 bit in its binary representation. 
        For example:
        
        2 is 0010 in binary.
        4 is 0100 in binary.
        8 is 1000 in binary.

        In general, powers of 2 in binary are represented as a single 1 bit followed by 0s.
        Subtracting 1 from n will flip all the bits to the right of the 1 bit (including the 1 bit itself).
        Because, Right shift reduces number, Left shift increases number.

        If n is not a power of 2:
        * n has multiple 1 bits.
        * Subtracting 1 from n flips all bits to the right of the rightmost 1 bit, 
          but there will still be at least one 1 bit in the result.
        * So, performing n & (n - 1) will not be 0.
        
        Solution: 
        private static bool IsPowerOfTwo(int n)
        {
            return n > 0 && (n & (n - 1)) == 0;
        }

        Console.WriteLine(IsPowerOfTwo(6));     // False
        Console.WriteLine(IsPowerOfTwo(8));     // True
        Console.WriteLine(IsPowerOfTwo(7));     // False


        2. Question : 
        */

        /* What is n & (n - 1)?

        The expression n & (n - 1) is used to turn off the rightmost set bit
        (the rightmost 1) in a binary number.
        It changes the lowest set bit to 0 and leaves all higher-order bits unchanged. 
        This is a crucial operation when working with bit manipulation. 

        Example: 7 (n = 7)
        0111   (7 in binary)
      & 0110   (6 in binary)
      --------
        0110   (Result) = 6

        0110   (6 in binary)
      & 0101   (5 in binary)
     --------   
        0100   (Result) = 4

        */

        /* Flag Enums
        You can combine enum members. 
        
        enum UserRoles
        {
            None = 0,               // 0000
            Admin = 1,              // 0001
            Moderator = 1 << 1,     // 0010 (same as 2)
            Editor = 1 << 2,        // 0100 (same as 4)
            Contributor = 1 << 3,   // 1000 (same as 8)
            Viewer = 1 << 4         // 10000 (same as 16)
        }

        1. Combining Roles
        You can combine roles using the bitwise OR (|) operator, 
        which is equivalent to adding their respective powers of two.

        UserRoles user = UserRoles.Admin | UserRoles.Editor;
        Console.WriteLine(user); // Output: Admin, Editor

        Admin is 0001
        Editor is 0100
        When OR'd together: 0001 | 0100 = 0101 (which represents both Admin and Editor).

        2. Checking Roles
        To check if a user has a particular role, you can use the bitwise AND (&) operator:

        UserRoles user = UserRoles.Admin | UserRoles.Editor;

        if ((user & UserRoles.Admin) != 0)
            Console.WriteLine("User is an Admin");
        else if ((user & UserRoles.Editor) != 0)
            Console.WriteLine("User is Editor");
        else
            Console.WriteLine("User has to another role");

        user is 0101 (Admin and Editor).
        We AND it with 0001 (Admin).
        If the result is non-zero, the user has the Admin role.

        Adding or Removing Roles
        
        user |= UserRoles.Moderator;  // Adds the Moderator role
        it means -> user = user.current roles (admin | editor) | new role Moderator and combines them
        Console.WriteLine(user);      // Output: Admin, Editor, Moderator

        Bitwise NOT (~):
        The ~ (NOT) operator inverts all the bits. 
        So, if the Editor role is 0100 in binary, 
        the NOT operator will flip all bits, resulting in:
        ~UserRoles.Editor = 1011

        This gives us the negated value of Editor, 
        where the Editor bit is now "off" (0), and all other bits are "on" (1).

        current user roles are :
        0111  // Admin, Moderator, Editor (before removal)

        1011  // Result of ~Editor

        UserRoles user = UserRoles.Admin | UserRoles.Moderator | UserRoles.Editor;
        // 0001
        // 0010
        // 0100
        //-----
        // 0111 all the roles

        // removing editor
        // editor is 0100 and not operator will flip it into : 1011

        //0111
        //1011
        //----
        //0011 is equal to admin and moderator

        user &= ~UserRoles.Editor; // admin and moderator

        Console.WriteLine(user);
        */
    }

    static T FindMax<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) > 0 ? a : b;
    }

    //Q1
    private static bool IsPowerOfTwo(int n)
    {
        return n > 0 && (n & (n - 1)) == 0;
    }

    //Q2
    private static int CountSetBits(int n)
    {
        int count = 0;

        while (n > 0)
        {
            count++;
            n &= (n - 1);
        }

        return count;
    }
}

class ZooCleaner
{
    public static void Wash<T>(T animal) where T : Animal
    {
        Console.WriteLine($"animal: {animal.GetType().Name} is washed");
    }
}

class Animal
{
    public string Name { get; set; } = string.Empty;
}

class Bear : Animal
{
    public int SoundDecibel { get; set; }
}

class Camel : Animal
{
    public int NumberOfMountains { get; set; }
}

public class CustomStack<T>
{
    private int position;
    private T[] data { get; } = Array.Empty<T>();

    public void Push(T obj) => data[position++] = obj;
    public T Pop() => data[--position];
}

[Flags]
enum UserRoles
{
    None = 0,               // 0000
    Admin = 1,              // 0001
    Moderator = 1 << 1,     // 0010 (same as 2)
    Editor = 1 << 2,        // 0100 (same as 4)
    Contributor = 1 << 3,   // 1000 (same as 8)
    Viewer = 1 << 4         // 10000 (same as 16)
}

[Flags]
public enum Border
{
    None,
    Left,
    Right,
    Top,
    Bottom,
}

public enum HorizontalAlignment
{
    Left = BorderSide.Left,
    Right = BorderSide.Right,
    Center
}

public enum BorderSide
{
    Left,
    Right,
    Top,
    Bottom,
}

public interface i1 { void Foo(); }
public interface i2 { int Foo(); }

public class Widget : i1, i2
{
    public void Foo()
    {
        Console.WriteLine("Widget's implementation of I1.Foo");
    }

    int i2.Foo()
    {
        Console.WriteLine("Widget's implementation of I2.Foo");
        return 0;
    }
}

public interface IEnumerator
{
    bool MoveNext();
    object Current { get; }
    void Reset();
}

internal class Countdown : IEnumerator
{
    private int _count = 1;

    public Countdown(int startPoint = 0)
    {
        _count = startPoint;
    }

    public bool MoveNext() => _count-- > 0;
    public object Current => _count;
    public void Reset() => _count = 0;
}

public struct StructPoint
{
    public int X, Y;
}

public class ClassPoint
{
    public int X, Y;
}

public struct Point
{
    public int X, Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class Car
{
    public string Model { get; set; } = string.Empty;
    public int Year { get; set; }

    public override bool Equals(object? obj)
    {
        if (obj is Car other)
        {
            return Model == other.Model && Year == other.Year;
        }

        return false;
    }

    public override int GetHashCode() => base.GetHashCode();
}

//public class Point
//{
//    public int X, Y;
//}

public class BaseClass
{
    public virtual void Foo()
    {
        Console.WriteLine("BaseClass.Foo");
    }
}
public class Overrider : BaseClass
{
    public override void Foo()
    {
        Console.WriteLine("Overrider.Foo");
    }
}
public class Hider : BaseClass
{
    public new void Foo()
    {
        Console.WriteLine("Hider.Foo");
    }
}

public class A { public int Counter = 1; }
public class B : A { public new int Counter = 2; }

public class FileHandler : IDisposable
{
    private readonly IntPtr fileHandle;
    private bool disposed = false;

    public FileHandler(string filePath)
    {
        fileHandle = OpenFile(filePath);
    }

    public void Dispose()
    {
        Console.WriteLine("dispose is called");
        Dispose(true);
        GC.SuppressFinalize(this);  // Prevent finalizer from running
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposed)
        {
            if (disposing)
            {
                Console.WriteLine("Clean up managed resources (if any)");
            }

            Console.WriteLine("Clean up unmanaged resources");
            CloseFile(fileHandle);
            disposed = true;
        }
    }

    ~FileHandler()
    {
        Console.WriteLine("finalizer");
        Dispose(false);  // Cleanup in case Dispose wasn't called
    }

    private IntPtr OpenFile(string path)
    {
        Console.WriteLine("File opened.");
        return new IntPtr(123);  // Simulated handle
    }

    private void CloseFile(IntPtr handle)
    {
        Console.WriteLine("File closed.");
    }
}

//public class FileHandler
//{
//    private readonly IntPtr fileHandle;  // Unmanaged resource (file handle)

//    // Finalizer to clean up unmanaged resource
//    ~FileHandler()
//    {
//        CloseFile(fileHandle);
//        Console.WriteLine("Finalizer called: File handle closed.");
//    }

//    public FileHandler(string filePath)
//    {
//        fileHandle = OpenFile(filePath);
//    }

//    private IntPtr OpenFile(string path)
//    {
//        // Pretend to open a file and return a handle (IntPtr represents an unmanaged resource)
//        Console.WriteLine("File opened.");
//        return new IntPtr(123);  // Simulated handle
//    }

//    private void CloseFile(IntPtr handle)
//    {
//        // Pretend to close the file
//        Console.WriteLine("File closed.");
//    }
//}

public class Sentence
{
    private readonly string[] _words = "Lorem Ipsum is simply dummy text of the printing and typesetting industry".Split();
    public string this[int index] // indexer
    {
        get => _words[index];
        set => _words[index] = value;
    }

    public string this[int arg1, int arg2]
    {
        get
        {
            var sb = new StringBuilder();
            sb.Append(_words[arg1]);
            sb.Append(_words[arg2]);

            return sb.ToString();
        }
    }
}

public class Note
{
    public int Pitch { get; set; }
    public decimal Duration { get; init; }
}

public class Rectangle
{
    public readonly float Width, Height;

    public Rectangle(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public void Deconstruct(out float width, out float height)
    {
        width = Width;
        height = Height;
    }
}