using StackQueues;

internal class Program
{
    private static void Main(string[] args)
    {
        // stack
        //TypeStack<int> stack = new TypeStack<int>(5);
        //Console.WriteLine($"Is empty: {stack.IsEmpty}"); // True

        //stack.Push(10);
        //stack.Push(20);
        //stack.Push(30);

        //Console.WriteLine($"Count: {stack.Count}"); // 3
        //Console.WriteLine($"Top element: {stack.Peek()}"); // 30

        //int popped = stack.Pop();
        //Console.WriteLine($"Popped: {popped}"); // 30

        //Console.WriteLine(stack.ToString());

        //if (stack.TryPeek(out int topElement))
        //{
        //    Console.WriteLine($"Top element: {topElement}");
        //}

        //stack.Clear();
        //Console.WriteLine($"After clear - Is empty: {stack.IsEmpty}"); // True



        // queue
        TypeQueue<string> queue = new TypeQueue<string>(5);
        Console.WriteLine($"Is empty: {queue.IsEmpty}"); // True

        queue.Enqueue("mahammad");     // First person in line
        queue.Enqueue("ahmadov");      // Second person in line  
        queue.Enqueue("m.ahmadov.");   // Third person in line

        Console.WriteLine($"Count: {queue.Count}"); // 3
        Console.WriteLine($"Front person: {queue.Peek()}"); // mahammad
        Console.WriteLine(queue.ToString());

        // Dequeue elements (people getting served)
        string served = queue.Dequeue();
        Console.WriteLine($"Served: {served}"); // mahammad (first in, first out)
        Console.WriteLine(queue.ToString());

        if (queue.TryPeek(out string nextPerson))
        {
            Console.WriteLine($"Next to be served: {nextPerson}"); // ahmadov
        }

        // Add more people
        queue.Enqueue("twins #1");
        queue.Enqueue("twins #2");

        Console.WriteLine($"Queue after adding more: {queue}");
        while (!queue.IsEmpty)
        {
            Console.WriteLine($"Serving: {queue.Dequeue()}");
        }

        Console.WriteLine($"All done! Is empty: {queue.IsEmpty}"); // True
    }
}