using StackQueues;

internal class Program
{
    private static void Main(string[] args)
    {
        #region Stack
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
        #endregion

        #region Regular Queue
        //TypeQueue<string> queue = new TypeQueue<string>(5);
        //Console.WriteLine($"Is empty: {queue.IsEmpty}"); // True

        //queue.Enqueue("mahammad");     // First person in line
        //queue.Enqueue("ahmadov");      // Second person in line  
        //queue.Enqueue("m.ahmadov.");   // Third person in line

        //Console.WriteLine($"Count: {queue.Count}"); // 3
        //Console.WriteLine($"Front person: {queue.Peek()}"); // mahammad
        //Console.WriteLine(queue.ToString());

        //// Dequeue elements (people getting served)
        //string served = queue.Dequeue();
        //Console.WriteLine($"Served: {served}"); // mahammad (first in, first out)
        //Console.WriteLine(queue.ToString());

        //if (queue.TryPeek(out string nextPerson))
        //{
        //    Console.WriteLine($"Next to be served: {nextPerson}"); // ahmadov
        //}

        //// Add more people
        //queue.Enqueue("twins #1");
        //queue.Enqueue("twins #2");

        //Console.WriteLine($"Queue after adding more: {queue}");
        //while (!queue.IsEmpty)
        //{
        //    Console.WriteLine($"Serving: {queue.Dequeue()}");
        //}

        //Console.WriteLine($"All done! Is empty: {queue.IsEmpty}"); // True
        #endregion

        #region Circular Queue
        TypeCircularQueue<int> typeCircularQueue = new TypeCircularQueue<int>(5);

        try
        {
            typeCircularQueue.Dequeue();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        typeCircularQueue.Enqueue(1);
        typeCircularQueue.Enqueue(2);
        typeCircularQueue.Enqueue(3);
        typeCircularQueue.Enqueue(4);
        typeCircularQueue.Enqueue(5);

        try
        {
            typeCircularQueue.Enqueue(6);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        Console.WriteLine($"Before deleted: {typeCircularQueue}");

        Console.WriteLine("Deleting 2 items");
        for (int i = 0; i < 2; i++)
        {
            int dequeued = typeCircularQueue.Dequeue();
            Console.WriteLine($"#{i} Dequeued: {dequeued}");
        }

        Console.WriteLine($"After deleted: {typeCircularQueue}\n\n");

        for (int i = 0; i < 2; i++)
        {
            typeCircularQueue.Enqueue(Random.Shared.Next(10, 100));
        }

        Console.WriteLine($"Now circual queue format is created: {typeCircularQueue}");

        Console.WriteLine("Trying to enqueue elements");
        try
        {
            typeCircularQueue.Enqueue(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }

        while (!typeCircularQueue.IsEmpty())
        {
            Console.WriteLine($"Deleted item: {typeCircularQueue.Dequeue()}");
        }

        Console.WriteLine(typeCircularQueue.ToString());

        #endregion
    }
}