using System.Runtime.CompilerServices;

namespace Chapter4;


//delegate int Transformer(int x);

//class Test
//{
//    public static int Square(int x) => x * x;
//}

//class Test
//{
//    public int Square(int x) => x * x;
//}

// ---

//public class MyReporter
//{
//    public string Prefix { get; set; } = string.Empty;

//    public void ReportProgress(int progress)
//    {
//        Console.WriteLine($"{Prefix}: {progress}%");
//    }
//}

//delegate void Report(int progress);

// ---

//public class FileDownloader
//{
//    public static void DownloadFiles(FileDownloaderDelegate fileDownloader)
//    {
//        for (int i = 0; i < 100; i++)
//        {
//            Thread.Sleep(500);
//            fileDownloader(i);
//        }
//    }
//}

//public class ProgressLogger
//{
//    public void LogToConsole(int percentComplete)
//    {
//        Console.WriteLine($"Progress: {percentComplete}%");
//    }

//    public void LogToFile(int percentComplete)
//    {
//        Console.WriteLine($"File: {percentComplete}%{Environment.NewLine}");
//    }
//}

//public delegate void FileDownloaderDelegate(int i);

// ---
public class PriceChangedEventArgs : EventArgs
{
    //public readonly decimal OldPrice;
    //public readonly decimal NewPrice;

    public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
    {
        OldPrice = oldPrice;
        NewPrice = newPrice;
    }

    public decimal OldPrice { get; }
    public decimal NewPrice { get; }
}
public class AdvancedStock : Stock
{
    public AdvancedStock(string symbol, decimal price, string stockName) : base(symbol)
    {
        StockName = stockName;
        Price = price;
        base.OnPriceChanged(new PriceChangedEventArgs(0, price));
    }
}
public class Stock
{
    private string _symbol = string.Empty;
    private decimal _price;

    public Stock(string symbol)
    {
        _symbol = symbol;
    }

    public event EventHandler<PriceChangedEventArgs>? PriceChanged;
    protected virtual void OnPriceChanged(PriceChangedEventArgs e)
    {
        PriceChanged?.Invoke(this, e);
    }

    public decimal Price
    {
        get => _price;
        set
        {
            if (_price == value) return;
            else
            {
                decimal oldPrice = _price;
                _price = value;
                OnPriceChanged(new PriceChangedEventArgs(oldPrice, value));
            }
        }
    }

    public string StockName { get; set; } = string.Empty;
}
record Point
{
    public Point(double x, double y) => (X, Y) = (x, y);
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }
}
public class GeneratedPoint : IEquatable<GeneratedPoint>
{
    public double X { get; init; }
    public double Y { get; init; }
    public double Z { get; init; }

    public GeneratedPoint(double x, double y)
    {
        X = x;
        Y = y;
    }

    // Generated copy constructor for nondestructive mutation
    protected GeneratedPoint(GeneratedPoint original)
    {
        X = original.X;
        Y = original.Y;
        Z = original.Z;
    }

    // Overridden Equals for structural equality
    public bool Equals(GeneratedPoint? obj) =>
        obj is GeneratedPoint point &&
        X == point.X &&
        Y == point.Y &&
        Z == point.Z;

    // Overridden GetHashCode for structural equality
    public override int GetHashCode() => HashCode.Combine(X, Y, Z);

    // Overridden ToString for better readability
    public override string ToString() => $"{nameof(GeneratedPoint)} {{ X = {X}, Y = {Y}, Z = {Z} }}";

    // == and != operators
    public static bool operator ==(GeneratedPoint left, GeneratedPoint right) => Equals(left, right);

    public static bool operator !=(GeneratedPoint left, GeneratedPoint right) => !Equals(left, right);
}
public record Student(string ID, string LastName, string GivenName)
{
    public string ID { get; } = ID;
}
public record PointValidation
{
    public PointValidation(double x, double y) => (X, Y) = (x, y);

    private double _x;
    public double X
    {
        get => _x;
        init
        {
            if (double.IsNaN(value))
                throw new Exception("X Cannot be NaN");
            _x = value;
        }
    }
    public double Y { get; init; }
}
struct Geometry { public int x, y; }

internal class Program
{
    static unsafe void Main(string[] args)
    {
        /* Delegates

        A delegate is an object that knows how to call a method.
        Specifically, it defines the method’s return type and its parameter types.

        delegate int Transformer (int x);

        Transformer is compatible with any method with an int return type and a single int parameter :

        int Square (int x) => return x * x;

        Assigning a method to a delegate variable creates a delegate instance :
        Transformer t = Square;

        You can invoke a delegate instance in the same way as a method :
        int answer = t(3); or t.Invoke(3);      // answer = 9;


        Here’s a complete example:
        Transformer t = Square; // Create delegate instance
        int result = t(3); // Invoke delegate
        Console.WriteLine (result); // 9

        Transformer t = (int a) => a; 
        ...is equal to 
        Transformer t2 = new Transformer((int a) => a);

        NOTE:
        Technically, we are specifying a method group when we refer to Square without brackets or arguments. 
        If the method is overloaded, C# will pick the correct overload based on 
        the signature of the delegate to which it’s being assigned.

        // Overloaded methods
        static int Square(int x) => x * x; // Single int parameter
        static double Square(double x) => x * x; // Single double parameter

        // Delegate that expects a method with the signature: int (int)
        delegate int Transformer(int x);

        Transformer t = Square; // This resolves to: int Square(int x)
        */

        /* Writing Plug-in Methods with Delegates

        Delegates are a powerful feature in C# that allow you to treat methods as first-class objects. 
        This means you can assign methods to variables, pass them as parameters, 
        and even return them from other methods. 
        Delegates are essential for implementing callback methods, event handling, and 
        creating flexible and extensible code architectures like plug-in systems.

        int[] ints = { 1, 2, 3, 4, 5, 6, 7, 8 };

        // first way to use
        Transformer t = Square;
        Transform(ints, t);
        foreach (int x in ints) Console.WriteLine(x);

        // or you can send method directly as paramether
        Transform(ints, Square);
        foreach (int x in ints) Console.WriteLine(x);
        */

        /* Instance and Static Method Targets
        A delegate’s target method can be a local, static, or instance method.

        1. Static Method Target

        delegate int Transformer(int x);

        private class Test
        {
            public static int Square(int x) => x * x;
        }

         // Assign static method to delegate
        Transformer t = Test.Square;
        Console.WriteLine(t.Invoke(10));

        2. Instance Method Target

        var test = new Test();

        // Assign instance method to delegate
        Transformer t = test.Square;
        Console.WriteLine(t.Invoke(10));

        3. Instance Lifetime and Delegate Target

        When an instance method is assigned to a delegate object, the latter maintains a reference
        not only to the method but also to the instance to which the method belongs.

        a. static method targeting
        Delegate (t)
        +--------------------+
        |  Method: Square    | 
        |  Target: null      | (no instance)
        +--------------------+

        Here, the delegate holds a reference to the static method but has no target instance 
        (since static methods don’t belong to any specific instance).

        b. instance method targeting
        Delegate (t)
        +-----------------------+
        |  Method: Square       |
        |  Target: testInstance | (object reference)
        +-----------------------+

        In this case, the delegate holds both the method (Square) and a reference to the specific object (testInstance). 
        When you invoke the delegate, it calls the Square method for that particular object.

        var myReporter = new MyReporter();
        myReporter.Prefix = "Complete percentage";

        Report report = myReporter.ReportProgress;
        report(10); // Complete percentage: 10%

        Console.WriteLine(report.Target); //display fullname: Program + MyReporter (because it is located within Program class)
        Console.WriteLine(report.Target == myReporter); //True
        Console.WriteLine(report.Method); // Void ReportProgress(Int32)

        myReporter.Prefix = "";
        report(99); // : 99%
        */

        /* Multicast Delegates

        All delegate instances have multicast capability. 
        This means that a delegate instance can reference not just a single target method but also 
        a list of target methods. The + and += operators combine delegate instances:

        SomeDelegate d = SomeMethod1;
        d += SomeMethod2;

        The last line is functionally equivalent to :
        d = d + SomeMethod2;

        Invoking d will now call both SomeMethod1 and SomeMethod2. 
        Delegates are invoked in the order in which they are added.

        The - and -= operators remove the right delegate operand from the left delegate operand:
        d -= SomeMethod1;
        Invoking d will now cause only SomeMethod2 to be invoked.

        NOTE: Delegates are immutable, so when you call += or -=, 
        you’re in fact creating a new delegate instance and assigning it to the existing variable.
        */

        /* Example: Task Progress Reporting with Multiple Handlers

        public class FileDownloader
        {
            public static void DownloadFiles(FileDownloaderDelegate fileDownloader)
            {
                for (int i = 0; i < 100; i++)
                {
                    Thread.Sleep(500);
                    fileDownloader(i);
                }
            }
        }

        public class ProgressLogger
        {
            public void LogToConsole(int percentComplete)
            {
                Console.WriteLine($"Progress: {percentComplete}%");
            }

            public void LogToFile(int percentComplete)
            {
                Console.WriteLine($"File: {percentComplete}%{Environment.NewLine}");
            }
        }

        public delegate void FileDownloaderDelegate(int i);

        ProgressLogger progressLogger = new ProgressLogger();

        FileDownloaderDelegate downloaderDelegate = progressLogger.LogToConsole;
        downloaderDelegate += progressLogger.LogToFile;

        FileDownloader.DownloadFiles(downloaderDelegate);

        */

        /* The Func and Action Delegates

        With generic delegates, it becomes possible to write delegate types that
        are so general they can work for methods of any return type. 
        These delegates are Action and Func delegates. (defined in the System namespace)

        1. Func

        The Func delegate is used for methods that return a value. 
        It can take up to 16 input parameters, and the last type parameter specifies the return type.

        delegate TResult Func<in T1, in T2, ..., out TResult>(T1 arg1, T2 arg2, ...);

        int[] numbers = new int[] { 1, 2, 3, 4, 5 };

        // Using Func to transform the array
        Transform(numbers, Square);

        foreach (int i in numbers) Console.WriteLine(i); // Outputs: 1, 4, 9, 16, 25

        2. Action

        The Action delegate is used for methods that do not return a value. 
        It can also take up to 16 input parameters.

        delegate void Action<in T1, in T2, ...>(T1 arg1, T2 arg2, ...);

        string dirtyText = @"
                            Meant balls it if up doubt small purse. 
                            Required hi-s you put the outlived answered position. 
                            An pleasure exertion if believed provided to. 
                            All led ou-t world these music while asked. 
                            Paid mind even sons does he door no. 
                            Attended ov-ercame repeated it is perceive marianne in. 
                            In am think on style child of. 
                            Servants mor-eover in sensible he it ye p-ossible.";

        ParseMessages(dirtyText, LogInput);
        */

        /* Delegate Compatibility

         Delegate types are all incompatible with one another, even if their signatures are the same:

         D1 d1 = Method1;
         D2 d2 = Method1;

         d2 = d1; // Compile-time error.
         d2 = new D2(d1); // the following, however, is permitted.
        */

        /*
        //var chatApp = new ChatApp();
        //chatApp.OnMessage += ChatApp_OnMessage;

        //for (int i = 0; i < 100; i++)
        //{
        //    chatApp.WriteMessage($"second-{DateTime.Now.Second}");
        //    Thread.Sleep(500);
        //}
         */

        /* Standart Event Pattern

        At the core of the standard event pattern is System.EventArgs, a predefined .NET class with no members.
        EventArgs is a base class for conveying information for an event. 

        For reusability, the EventArgs subclass is named according to the information it contains.
        It typically exposes data as properties or as read-only fields.

        public class PriceChangedEventArgs : EventArgs
        {
            //public readonly decimal OldPrice;
            //public readonly decimal NewPrice;

            public PriceChangedEventArgs(decimal oldPrice, decimal newPrice)
            {
                OldPrice = oldPrice;
                NewPrice = newPrice;
            }

            public decimal OldPrice { get; }
            public decimal NewPrice { get; }
        }

        With an EventArgs subclass in place, the next step is to choose or define a delegate for the event. 
        There are three rules:

        1. It must have a void return type.

        2. It must accept two arguments: 
           a. the first of type object and 
           b. the second a subclass of EventArgs. 
        The first argument indicates the event broadcaster, and the second argument contains the extra information.

        3. Its name must end with EventHandler.

        .NET defines a generic delegate called System.EventHandler<> that satisfies these rules:
        public delegate void EventHandler<TEventArgs> (object source, TEventArgs e) where TEventArgs : EventArgs;

        public class Stock
        {
            private string _symbol = string.Empty;
            private decimal _price;

            public Stock(string symbol)
            {
                _symbol = symbol;
            }

            public event EventHandler<PriceChangedEventArgs>? PriceChanged;
            protected virtual void OnPriceChanged(PriceChangedEventArgs e)
            {
                PriceChanged?.Invoke(this, e);
            }

            public decimal Price
            {
                get => _price;
                set
                {
                    if (_price == value) return;
                    else
                    {
                        decimal oldPrice = _price;
                        _price = value;
                        OnPriceChanged(new PriceChangedEventArgs(oldPrice, value));
                    }
                }
            }

            public string StockName { get; set; } = string.Empty;
        }

        Stock stock = new Stock("AZE");
        stock.Price = 27.10M;

        stock.PriceChanged += Stock_PriceChanged;
        stock.Price = 40M;

        private static void Stock_PriceChanged(object? sender, PriceChangedEventArgs e)
        {
            if ((e.NewPrice - e.OldPrice) / e.NewPrice > 0.1M)
            {
                Console.WriteLine("Alert, 10% stock price increase!");
            }
        }

        */

        /* System.ValueTuple

        var anonymousClass = new { LastName = "Ahmadov", Age = 21 };
        Console.WriteLine($"LastName: {anonymousClass.LastName}\nAge: {anonymousClass.Age}");

        (string lastname, int age) = ("Ahmadov", 21);
        Console.WriteLine($"LastName: {lastname}\nAge: {age}");

        Type Erause in Tuple

        We stated previously that the C# compiler handles anonymous types 
        by building custom classes with named properties for each of the elements. 
        With Tuples, c# works differently and uses a preexisting family of generic structs.

        public struct ValueTuple<T1>

        Hence, (string,int) is an alias for ValueTuple<string,int>, 
        and this means that named tuple elements have no corresponding property names in the underlying types.

        Instead, the names exist only in the source code, and in the imagination of the compiler. 
        At runtime, the names mostly disappear, 
        so if you decompile a program that refers to named tuple elements, 
        you’ll see just references to Item1, Item2, and so on.

        The syntax for deconstruction is confusingly similar to the syntax for declaring a tuple with named elements. 
        The following highlights the difference:

        var person = (name: "mahammad", age: 21);

        1. deconstructing a tuple
        (string name, int age) = person; 

        2. declaring a new tuple
        (string name, int age) newPerson = person;

        var tuple = (name: "mahammad", age: 21);
        Console.WriteLine($"name: {tuple.name}\nage: {tuple.age}");

        it converts above code into this:
        first it creates value tuple from our code and then reference our namings to itemN

        DefaultInterpolatedStringHandler handler = new DefaultInterpolatedStringHandler();
        handler.AppendLiteral("name: ");
        handler.AppendFormatted(tuple.name);
        Console.WriteLine(handler.ToStringAndClear());

        ValueTuple<string, int> tup = ValueTuple.Create(tuple.name, tuple.age);
        */

        /* System.Tuple

        You’ll find another family of generic types in the System namespace called Tuple (rather than ValueTuple).
        These were introduced back in 2010 and were defined as classes 
        (whereas the ValueTuple types are structs).

        Why C# Introduced System.Tuple?
        System.Tuple was introduced in .NET 4.0 (C# 4.0) as a way to group multiple related values 
        into a single object without creating a custom class or struct. 
        It provided a convenient way to return multiple values from methods, 
        particularly when you didn't want to create a whole class or struct for temporary data structures.

        Before System.Tuple, developers had to create custom types or use arrays 
        (which were less type-safe) for similar functionality. 
        System.Tuple allowed combining values of different types easily and 
        was particularly useful for functional-style programming patterns.

        What Was the Cause of Creating System.ValueTuple?

        *NOTE: While System.Tuple was helpful, it had several limitations:

        1. Heap Allocation: 
        System.Tuple is a reference type, meaning it is allocated on the heap, 
        which adds overhead in terms of memory usage and garbage collection, especially for short-lived objects.

        2. Lack of Mutability:
        System.Tuple is immutable, which, while useful in some cases, 
        could be restrictive when you wanted to modify the contents after creation.

        3. Naming Issues: 
        System.Tuple didn't provide meaningful names for its elements. 
        It just used names like Item1, Item2, etc., which made code harder to read.

        *NOTE: System.ValueTuple:
        System.ValueTuple was created to solve these limitations:

        1. Stack Allocation: As a value type, System.ValueTuple is stored on the stack, which reduces memory pressure and 
        increases performance, especially for short-lived objects.

        2. Improved Syntax: C# 7 introduced support for deconstruction with System.ValueTuple, 
        allowing you to unpack tuples in a much more readable way. For example:

        (int x, int y) = GetCoordinates();

        3. Named Members: System.ValueTuple supports named members, 
        making it much clearer what each element of the tuple represents.


        */

        /* Records (C# 9)

        A record is a special kind of class that’s designed to work well with immutable (readonly) data. 
        Its most useful feature is nondestructive mutation;
        however, records are also useful in creating types that just combine or hold data.

        A record is purely a C# compile-time construct. At runtime, the CLR sees them just as classes

        ---Background

        Writing immutable classes (whose fields cannot be modified after initialization) is a popular strategy for simplifying software and reducing bugs. 
        It’s also a core aspect of functional programming, where mutable state is avoided and functions are treated as data. 
        LINQ is inspired by this principle.

        ---Functional Programming and Immutability
        In functional programming, one of the core principles is to avoid mutable state (i.e., variables or data that can be changed after creation).
        Instead, functions and data structures are designed to be immutable (unchangeable after initialization). 
        This approach has several benefits:

        1. Concurrency safety: 
        Immutable data structures are easier to work with in multi-threaded environments since 
        there's no risk of one thread modifying data that another thread is using.

        Nondestructive Mutation

        "Nondestructive mutation" means that instead of modifying an object,
        you create a new object with the desired changes, leaving the original unchanged. 
        This concept is at the heart of records and functional programming.

        ---How LINQ Is Inspired by Functional Programming

        1. Immutable Data Transformation:
        LINQ methods, such as .Select(), .Where(), and .OrderBy(), don’t modify the original collection; 
        instead, they return new collections. 
        This is consistent with functional programming, where data is not mutated but transformed into new data structures.

        In order to “modify” an immutable object, you must create a new one and copy over the data while 
        incorporating your modifications (this is called nondestructive mutation).

        ---Defining a Record
        A record definition is like a class definition, and can contain the same kinds of members, including fields, properties, methods, and so on. 
        Records can implement interfaces and subclass other records (but not classes).

        A simple record might contain just a bunch of init-only properties and perhaps a constructor:

        record Point 
        {
            public Point(double x, double y) => (X, Y) = (x, y);
            public double X { get; init; }
            public double Y { get; init; }
            public double Z { get; init; }
        }

        Upon compilation, C# transforms the record definition into a class and performs the following additional steps:

        1. It writes a protected copy constructor (and a hidden Clone method) to facilitate nondestructive mutation.

        C# generates a copy constructor for records. This allows you to create a copy of a record while 
        changing one or more properties—without altering the original object. 
        This is often referred to as nondestructive mutation, because the original object remains unchanged.

        var p1 = new Point(1, 2);
        var p2 = p1 with { X = 5 }; // Copying p1 and updating X
        Console.WriteLine(p1); // Output: Point { X = 1, Y = 2, Z = 0 }
        Console.WriteLine(p2); // Output: Point { X = 5, Y = 2, Z = 0 }

        The most important step that the compiler performs with all records is to write a copy constructor (and a hidden Clone method). 
        This enables nondestructive mutation via C# 9’s with keyword:

        2. It overrides/overloads the equality-related functions to implement structural equality.

        C# records automatically implement structural equality, which means two record instances are considered equal if their properties are equal, 
        not if they reference the same object. 
        This is in contrast to classes, where equality is typically based on reference unless you explicitly override the equality methods.

        The following methods are automatically overridden or overloaded to provide this behavior:
        1. Equals(object obj)
        2. GetHashCode()
        3. == and != operators

        Console.WriteLine(p1 == p2); // Output: False (because of non structural equality)

        3. It overrides the ToString() method (to expand the record’s public properties, as with anonymous types).

        The ToString() method is overridden in records to provide a more useful string representation of the record. 
        It automatically prints out the names and values of the record’s public properties.


        It generates something like this:

        public class GeneratedPoint : IEquatable<GeneratedPoint>
        {
            public double X { get; init; }
            public double Y { get; init; }
            public double Z { get; init; }

            public GeneratedPoint(double x, double y)
            {
                X = x;
                Y = y;
            }

            // Generated copy constructor for nondestructive mutation
            protected GeneratedPoint(GeneratedPoint original)
            {
                X = original.X;
                Y = original.Y;
                Z = original.Z;
            }

            // Overridden Equals for structural equality
            public bool Equals(GeneratedPoint? obj) =>
                obj is GeneratedPoint point &&
                X == point.X &&
                Y == point.Y &&
                Z == point.Z;

            // Overridden GetHashCode for structural equality
            public override int GetHashCode() => HashCode.Combine(X, Y, Z);

            // Overridden ToString for better readability
            public override string ToString() => $"{nameof(GeneratedPoint)} {{ X = {X}, Y = {Y}, Z = {Z} }}";

            // == and != operators
            public static bool operator ==(GeneratedPoint left, GeneratedPoint right) => Equals(left, right);

            public static bool operator !=(GeneratedPoint left, GeneratedPoint right) => !Equals(left, right);
        }

        */

        /* Patterns

        object obj = "mahammad";

        if (obj is string)
        {
            Console.WriteLine(((string)obj).Length);
        }

        Or, more concisely:

        if (obj is string s)
        {
            Console.WriteLine(s.Length);
        }


        This employs one kind of pattern called a type pattern. 
        The is operator also supports other patterns that were introduced in recent versions of C#, such as the property pattern:

        if (obj is string { Length: 4 })
        {
            Console.WriteLine("A string with 4 characters.");
        }

        Patterns are supported in the following contexts:
        1. After the is operator (variable is pattern)
        2. In switch statements
        3. In switch expressions 

        */

        /* Caller Info Attributes

        You can tag optional parameters with one of three caller info attributes, 
        which instruct the compiler to feed information obtained from the caller’s source code 
        into the parameter’s default value:

        1. [CallerMemberName] applies the caller’s member name.
        2. [CallerFilePath] applies the path to the caller’s source code file.
        3. [CallerLineNumber] applies the line number in the caller’s source code file.

        */

        /* Operator Overloading

        You overload an operator by declaring an operator function. 
        An operator function has the following rules:

        1. The name of the function is specified with the operator keyword followed by an operator symbol.
        2. The operator function must be marked as static and public.
        3. The parameters of the operator function represent the operands.
        4. At least one of the parameters of a binary operator must be the containing type.

        In the following example, we define a struct called Note representing a musical note 
        and then overload the + operator:

        public class Note
        {
            private int _value;

            public Note(int Value)
            {
                _value = Value;
            }

            public int Value
            {
                get => _value;
                set
                {
                    if (_value != value)
                    {
                        _value = value;
                    }
                }
            }

            public static Note operator +(Note previousNote, int value)
            {
                return new Note(previousNote._value + value);
            }
        }

        // Using overloaded operators
        Note noteA = new Note(0);  // A4 (440 Hz)
        Note noteB = noteA + 12;   // One octave above A4 (A5)

        Overloading an operator automatically overloads the corresponding compound assignment operator. 
        In our example, because we overrode +, we can use +=, too: CSharp += 2;

        // Loop until note value exceeds a specific frequency threshold
        int loopInitializingCount = 0;
        while ((double)noteA < 800.0)  // Compare note to frequency
        {
            noteA += 1;
            loopInitializingCount++;
        }

        Console.WriteLine($"Loop initializing count: {loopInitializingCount}, new note: {noteA}, frequency: {(double)noteA} Hz");


        ---Overloading Equality and Comparison Operators
        Equality and comparison operators are sometimes overridden when writing structs, 
        and in rare cases when writing classes.

        1. Pairing
        The C# compiler enforces operators that are logical pairs to both be defined.
        These operators are logical pairs: 
        1. (== . !=), 
        2. (< . >), and 
        3. (<= . >=).

        bool isHigher = noteB > noteA.Value;
        Console.WriteLine($"Is {noteB} higher than {noteA}? {isHigher}");

        2. Equals and GetHashCode
        In most cases, if you overload (==) and (!=), you must override the Equals and GetHashCode methods 
        defined on object in order to get meaningful behavior.

        3. IComparable and IComparable<T>
        If you overload (< >) and (<= >=), you should implement IComparable and IComparable<T>.
        */

        /* Custom Implicit and Explicit Conversions
         
        Implicit and explicit conversions are overloadable operators. 
        Overloading implicit and explicit conversions requires you to define methods that 
        convert between types in a natural, readable way.

        For implicit conversions, the conversion should be safe, meaning it won’t lose information. 
        For explicit conversions, which need a cast, 
        there’s a possibility of losing data or the conversion failing under certain conditions.

        public static implicit operator double(Note note)
        {
            return 440 * Math.Pow(2, (double)note.Value / 12);
        }

        public static explicit operator Note(double frequency)
        {
            return new Note((int)(0.5 + 12 * (Math.Log(frequency / 440) / Math.Log(2))));
        }

       // Explicit conversion from frequency to Note
        Note noteFromFrequency = (Note)554.37;
        Console.WriteLine($"Note from 554.37 Hz: {noteFromFrequency}");

        // Implicit conversion from Note to frequency
        double frequency = noteFromFrequency;
        Console.WriteLine($"Frequency from Note {noteFromFrequency}: {frequency} Hz");

        */

        /* example codes for operators
         
                    //// Explicit conversion from frequency to Note
        //Note noteFromFrequency = (Note)554.37;
        //Console.WriteLine($"Note from 554.37 Hz: {noteFromFrequency}");

        //// Implicit conversion from Note to frequency
        //double frequency = noteFromFrequency;
        //Console.WriteLine($"Frequency from Note {noteFromFrequency}: {frequency} Hz");

        //// Using overloaded operators
        //Note noteA = new Note(0);  // A4 (440 Hz)
        //Note noteB = noteA + 12;   // One octave above A4 (A5)

        //bool isHigher = noteB > noteA.Value;
        //Console.WriteLine($"Is {noteB} higher than {noteA}? {isHigher}");

        //// Loop until note value exceeds a specific frequency threshold
        //int loopInitializingCount = 0;
        //while ((double)noteA < 800.0)  // Compare note to frequency
        //{
        //    noteA += 1;
        //    loopInitializingCount++;
        //}

        //Console.WriteLine($"Loop initializing count: {loopInitializingCount}, new note: {noteA}, frequency: {(double)noteA} Hz");

        */

        /* Unsafe Code and Pointers
        
        n C#, using pointers allows direct memory manipulation, but it must be done within an unsafe code block. 
        This is often needed for interacting with unmanaged code (like C APIs) or for optimizing performance. 
        However, it requires the /unsafe compiler option because it's not type-safe.

        ---For every value type or reference type V, there is a corresponding pointer type V*. 
        A pointer instance holds the address of a variable. 
        
        *In C#, the following pointer types are available:

        1. Value Type Pointers: Pointers to primitive types like int*, float*, double*, etc.
           Example: int* p;

        2. Reference Type Pointers: Pointers to reference types like classes or arrays 
           (though it's unsafe and rarely used).
           Example: MyClass* ptr;

        3. Void Pointers: void* can point to any type, but can't be dereferenced directly. 
           Useful for general-purpose memory manipulation.
           Example: 
                    void* pVoid = pNum; 
                    Console.WriteLine("Void pointer holding address: " + (IntPtr)pVoid);

        4. Pointer to Pointers: You can have pointers that point to other pointers, such as int**.

        int x = 10;      // x is stored at address 0x7ffeefbff5b8
        int* p = &x;     // p holds the address 0x7ffeefbff5b8
        int** pointerToPointer = &p;
        int*** pointerToPointerToPointer = &pointerToPointer;

        int dereferencedValue = **pointerToPointer;
        int dereferencedTriple = ***pointerToPointerToPointer;

        ---In C#, pointer types allow you to work directly with memory addresses using the following operators:

        1. & (address-of): Gets the memory address of a variable, returning a pointer to it. 

        unsafe
        {
            int x = 10;
            int* pointer = &x;
        }

        Stack:
        
        x = 10 is stored in the stack since x is a local variable of type int, which is a value type. 
        The stack holds both the value (10) and the variable (x).

        p is also placed in the stack. It's a pointer, so it stores the memory address of the variable x (not the value of x). 
        The pointer p holds the address of where x is located in memory.

        For example, let's say the variable x is stored at memory location 0x7ffeefbff5b8. 
        When you assign p = &x;, the pointer p holds this memory address.

        int x = 10;      // x is stored at address 0x7ffeefbff5b8
        int* p = &x;     // p holds the address 0x7ffeefbff5b8

        x stores the value 10.
        p stores the memory address (e.g., 0x7ffeefbff5b8), which points to where x is located in memory.

        When you use the dereference operator (*p), you access the value 10 by referring to that memory address.

        2. * (dereference): Accesses the value stored at the address a pointer holds.

        int y = *p;
        Console.WriteLine($"y now holds the value stored at p (which is x's value): {y}");

        3. -> (pointer-to-member): Shortcut for accessing a member via a pointer. x->y is equivalent to (*x).y.

        struct Geometry { public int x, y; }

        Geometry geom = new Geometry { x = 10, y = 20 };
        Geometry* gPt = &geom;

        Console.WriteLine($"x val: {gPt->x}, y val: {gPt->y}");
        Console.WriteLine($"[same logic] x val: {(*gPt).x}, y val: {(*gPt).y}");

        By marking a type, type member, or statement block with the unsafe keyword,
        you’re permitted to use pointer types and perform C++ style pointer operations on memory within that scope.
        Unsafe code can run faster than a corresponding safe implementation. 

        */

        /* The fixed statement
         
        The fixed statement in C# is essential when working with pointers and managed memory. 
        Since the garbage collector (GC) can move objects in memory during its operation, 
        using pointers to managed objects can lead to invalid memory references if those objects are relocated. 
        The fixed statement "pins" an object in memory, 
        ensuring that its address remains stable while you work with it.

        ---Key Points about fixed Statement

        1. Purpose: 
        To prevent the garbage collector from moving an object while you're accessing it through pointers.

        2. Usage: 
        Typically used with value types, arrays, or strings that you want to manipulate directly in memory.

        3. Performance Impact: 
        Pinning an object can impact the performance of the garbage collector, 
        so it should be used sparingly and only for short periods.

        int[] numbers = { 1, 2, 3, 4, 5 };
        int length = numbers.Length;

        fixed (int* p = numbers) // p points to the first element of the array
        {
            int* currentPointer = p;
            int i = 0;

            while (i < length)
            {
                Console.WriteLine($"Address of element {i}: {(IntPtr)currentPointer:X}");
                Console.WriteLine($"Value of element {i}: {*currentPointer}");

                currentPointer += 1;
                i++;
            }
        }

        #region About Garbage Collector
        ---What does it mean to move objects, and why is it useful?

        1. Where does the GC move objects?
        Objects in C# are typically allocated on the heap, which is a large pool of memory.
        Over time, as objects are created and destroyed, the heap becomes fragmented.
        This means that while there may be plenty of free memory overall, 
        you might not have a large enough contiguous block for new large objects.

        To avoid this problem, the GC moves live objects closer together in memory, 
        creating large, contiguous free blocks. This process is called compacting the heap.
        Compaction proceeds from the bottom of the heap (low addresses) to the top (high addresses).

        2. What is memory fragmentation?
        Fragmentation occurs when there are small, unused gaps in memory between live objects.
        If you have many small gaps, it can be hard to find large enough blocks of memory for new allocations, 
        even if the total free memory is sufficient.

        3. Why does moving objects improve memory utilization?
        By moving objects and packing them closer together, the GC consolidates free memory into larger blocks.
        This allows for more efficient allocation of new objects and 
        reduces the need to request additional memory from the operating system.

        ---Visual Example of Memory Fragmentation:
        Imagine the heap as a long row of boxes representing memory, and 
        each box can either be occupied or free:

        Before GC moves objects (fragmented memory):
        | Object A | Free | Object B | Free | Object C | Free | Object D |

        After GC moves objects (compact memory):
        | Object A | Object B | Object C | Object D | Free | Free | Free |

        By moving the objects closer together, the GC consolidates the free memory into a large, continuous block, 
        making it easier to allocate memory for new objects.
        #endregion

        */

        /* The stackalloc Keyword
         
        The stackalloc keyword in C# allows you to allocate a block of memory directly on the stack instead of the heap.
        This memory is temporary and is automatically freed when the method in which it is declared returns, just like any other local variable. 
        This is useful when you need fast, temporary memory without the overhead of garbage collection.

        Key Points

        1. Stack Allocation

        1.1) The memory allocated using stackalloc is on the stack, 
        which is much faster to allocate and deallocate than heap memory.
        1.2) The memory is limited to the method's scope and is freed as soon as the method returns.

        2. Limitations:

        2. 1) The size of the stack is much smaller than the heap, so you should only allocate small blocks of memory using stackalloc. 
        If you allocate too much memory, you may encounter a stack overflow.

        */

        #region Unsafe Code Examples
        //int* nums = stackalloc int[10];
        //// int* numbers = stackalloc int[10]; allocates an array of 10 integers on the stack.

        //for (int i = 0; i < 10; ++i)
        //    nums[i] = i;

        //for (int i = 0; i < 10; ++i)
        //    Console.WriteLine($"numbers[{i}] = {nums[i]}");

        //int[] numbers = { 1, 2, 3, 4, 5 };
        //int length = numbers.Length;

        //fixed (int* p = numbers) // p points to the first element of the array
        //{
        //    int* currentPointer = p;
        //    int i = 0;

        //    while (i < length)
        //    {
        //        Console.WriteLine($"Address of element {i}: {(IntPtr)currentPointer:X}");
        //        Console.WriteLine($"Value of element {i}: {*currentPointer}");

        //        currentPointer += 1;
        //        i++;
        //    }
        //}

        //int x = 10;      // x is stored at address 0x7ffeefbff5b8
        //int* y = &x;     // p holds the address 0x7ffeefbff5b8
        //int** pointerToPointer = &y;
        //int*** pointerToPointerToPointer = &pointerToPointer;

        //int dereferencedValue = **pointerToPointer;
        //int dereferencedTriple = ***pointerToPointerToPointer;

        //Geometry geom = new Geometry { x = 10, y = 20 };
        //Geometry* gPt = &geom;

        //Console.WriteLine($"x val: {gPt->x}, y val: {gPt->y}");
        //Console.WriteLine($"[same logic] x val: {(*gPt).x}, y val: {(*gPt).y}");
        #endregion
    }

    static void CallerInfo(
        [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        Console.WriteLine($"member name: {memberName}\nfile path: {filePath}\nline number: {lineNumber}");
    }

    static IEnumerable<string> Foo(bool breakEarly)
    {
        yield return "One";
        yield return "Two";

        if (breakEarly)
            yield break;

        yield return "Three";
    }
    static IEnumerable<string> Foo()
    {
        yield return "One";
        yield return "Two";
        yield return "Three";
    }
    static IEnumerable<int> Fibs(int fibCount)
    {
        for (int i = 0, prevFib = 1, curFib = 1; i < fibCount; i++)
        {
            yield return prevFib;
            int newFib = prevFib + curFib;
            prevFib = curFib;
            curFib = newFib;
        }
    }

    //private static void Stock_PriceChanged(object? sender, PriceChangedEventArgs e)
    //{
    //    if ((e.NewPrice - e.OldPrice) / e.NewPrice > 0.1M)
    //    {
    //        Console.WriteLine("Alert, 10% stock price increase!");
    //    }
    //}


    //private static void ChatApp_OnMessage(int userId, string message)
    //{
    //    Console.WriteLine($"[user: {userId}] {message}");
    //}

    //public delegate void MessageHandler(int userId, string message);
    //public class ChatApp
    //{
    //    private int _userId;
    //    private string _message = string.Empty;

    //    public MessageHandler? OnMessage;

    //    public void WriteMessage(string message)
    //    {
    //        _userId = DateTime.Now.Second;
    //        _message = message;

    //        OnMessage?.Invoke(_userId, _message);
    //    }
    //}

    //delegate void D1();
    //delegate void D2();
    //static void Method1() { }

    //Action example
    //private static void LogInput(string msg) => Console.WriteLine(msg);
    //private static void ParseMessages(string dirtyText, Action<string> action)
    //{
    //    foreach (var item in dirtyText.Split(' '))
    //    {
    //        if (item.Contains('-'))
    //        {
    //            action.Invoke($"dirty word: {item}");
    //        }
    //    }
    //}

    //Func example
    //private static int Square(int x) => x * x;
    //private static void Transform<T>(T[] values, Func<T, T> func)
    //{
    //    for (int i = 0; i < values.Length; i++)
    //    {
    //        values[i] = func.Invoke(values[i]);
    //    }
    //}

    //private static int Square(int x) => (int)Math.Pow(x, 2);

    //private static void Transform(int[] values, Transformer transformer)
    //{
    //    for (int i = 0; i < values.Length; i++)
    //    {
    //        values[i] = transformer(values[i]);
    //    }
    //}
}