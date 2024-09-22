using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;

internal class Program
{
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

    private static void Main(string[] args)
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

        Delegates are immutable, so when you call += or -=, 
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

        ProgressLogger progressLogger = new ProgressLogger();

        FileDownloaderDelegate downloaderDelegate = progressLogger.LogToConsole;
        downloaderDelegate += progressLogger.LogToFile;

        FileDownloader.DownloadFiles(downloaderDelegate);
    }

    //private static int Square(int x) => (int)Math.Pow(x, 2);

    //private static void Transform(int[] values, Transformer transformer)
    //{
    //    for (int i = 0; i < values.Length; i++)
    //    {
    //        values[i] = transformer(values[i]);
    //    }
    //}
}