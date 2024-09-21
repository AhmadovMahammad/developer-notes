using System.Reflection.Metadata.Ecma335;

internal class Program
{
    delegate int Transformer(int x);
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

        /*
         
        */
    }
}