using System.Collections.Specialized;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Chapter3
{
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

        }
    }

    //public class Point
    //{
    //    public int X, Y;
    //}

    //public class BaseClass
    //{
    //    public virtual void Foo()
    //    {
    //        Console.WriteLine("BaseClass.Foo");
    //    }
    //}
    //public class Overrider : BaseClass
    //{
    //    public override void Foo()
    //    {
    //        Console.WriteLine("Overrider.Foo");
    //    }
    //}
    //public class Hider : BaseClass
    //{
    //    public new void Foo()
    //    {
    //        Console.WriteLine("Hider.Foo");
    //    }
    //}

    //public class A { public int Counter = 1; }
    //public class B : A { public new int Counter = 2; }

    //public class FileHandler : IDisposable
    //{
    //    private readonly IntPtr fileHandle;
    //    private bool disposed = false;

    //    public FileHandler(string filePath)
    //    {
    //        fileHandle = OpenFile(filePath);
    //    }

    //    public void Dispose()
    //    {
    //        Console.WriteLine("dispose is called");
    //        Dispose(true);
    //        GC.SuppressFinalize(this);  // Prevent finalizer from running
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if (!disposed)
    //        {
    //            if (disposing)
    //            {
    //                Console.WriteLine("Clean up managed resources (if any)");
    //            }

    //            Console.WriteLine("Clean up unmanaged resources");
    //            CloseFile(fileHandle);
    //            disposed = true;
    //        }
    //    }

    //    ~FileHandler()
    //    {
    //        Console.WriteLine("finalizer");
    //        Dispose(false);  // Cleanup in case Dispose wasn't called
    //    }

    //    private IntPtr OpenFile(string path)
    //    {
    //        Console.WriteLine("File opened.");
    //        return new IntPtr(123);  // Simulated handle
    //    }

    //    private void CloseFile(IntPtr handle)
    //    {
    //        Console.WriteLine("File closed.");
    //    }
    //}

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

    //public class Sentence
    //{
    //    private readonly string[] _words = "Lorem Ipsum is simply dummy text of the printing and typesetting industry".Split();
    //    public string this[int index] // indexer
    //    {
    //        get => _words[index];
    //        set => _words[index] = value;
    //    }

    //    public string this[int arg1, int arg2]
    //    {
    //        get
    //        {
    //            var sb = new StringBuilder();
    //            sb.Append(_words[arg1]);
    //            sb.Append(_words[arg2]);

    //            return sb.ToString();
    //        }
    //    }
    //}

    //public class Note
    //{
    //    public int Pitch { get; set; }
    //    public decimal Duration { get; init; }
    //}

    //public class Rectangle
    //{
    //    public readonly float Width, Height;

    //    public Rectangle(float width, float height)
    //    {
    //        Width = width;
    //        Height = height;
    //    }

    //    public void Deconstruct(out float width, out float height)
    //    {
    //        width = Width;
    //        height = Height;
    //    }
    //}
}