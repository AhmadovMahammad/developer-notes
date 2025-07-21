using System.Text;

namespace StackQueues;
public class TypeQueue<T>
{
    #region Queue Documentation
    // 
    // A queue is a linear data structure that follows the FIFO principle:
    // FIFO = First In, First Out
    // 
    // Think of it like a line of people waiting for tickets at a cinema:
    // - People join the line at the REAR (back)
    // - People leave the line from the FRONT (beginning)
    // - The first person to join is the first person to get served
    //
    //
    // Visual representation:
    // 
    //   FRONT → [1] [2] [3] [4] ← REAR
    //           ↑               ↑
    //       Remove here     Add here
    //      (Dequeue/Pop) (Enqueue/Push)
    // 
    // Person 1 was first to join, so they'll be first to leave
    // Person 4 was last to join, so they'll be last to leave
    //
    //
    // BASIC OPERATIONS
    // 
    // • ENQUEUE: Add element to the rear (end) of the queue
    // • DEQUEUE: Remove and return element from the front of the queue  
    // • PEEK/FRONT: View front element without removing it
    // • ISEMPTY: Check if queue has no elements
    // • ISFULL: Check if queue has reached maximum capacity
    // • SIZE: Get current number of elements
    //
    //
    // HOW IT WORKS WITH POINTERS
    // 
    // We use TWO pointers to track the queue:
    // 1. FRONT: Points to the first element (next to be removed)
    // 2. REAR: Points to the last element (most recently added)
    // 
    // Initially: FRONT = -1, REAR = -1 (empty queue)
    //
    //
    // Example operations:
    //
    // Initial:     FRONT = -1, REAR = -1, array = [_, _, _, _]
    // Enqueue(A):  FRONT = 0,  REAR = 0,  array = [A, _, _, _]
    // Enqueue(B):  FRONT = 0,  REAR = 1,  array = [A, B, _, _]
    // Enqueue(C):  FRONT = 0,  REAR = 2,  array = [A, B, C, _]
    // Dequeue():   FRONT = 1,  REAR = 2,  array = [_, B, C, _], returns A
    // Dequeue():   FRONT = 2,  REAR = 2,  array = [_, _, C, _], returns B
    // Enqueue(D):  FRONT = 2,  REAR = 3,  array = [_, _, C, D]
    //
    //
    //
    // QUEUE vs STACK COMPARISON
    // 
    // STACK (LIFO):        QUEUE (FIFO):
    // Last In, First Out   First In, First Out
    // 
    //     [3] ← TOP            FRONT → [1] [2] [3] ← REAR
    //     [2]                          ↑         ↑
    //     [1]                      Remove     Add
    // 
    // Stack: Add/Remove from same end (top)
    // Queue: Add at rear, Remove from front
    //
    //
    //
    // LIMITATION OF SIMPLE ARRAY QUEUE
    // 
    // As we dequeue elements, the front moves forward, leaving empty spaces:
    // After some operations: [_, _, C, D] (spaces 0,1 are wasted)
    // 
    // Solution: Circular Queue (advanced topic)
    // 
    // TIME COMPLEXITY
    // • Enqueue: O(1) - constant time
    // • Dequeue: O(1) - constant time  
    // • Peek: O(1) - constant time
    // • IsEmpty: O(1) - constant time
    // • IsFull: O(1) - constant time
    //
    //
    // SPACE COMPLEXITY
    // • O(n) where n is the maximum capacity of the queue
    //
    //
    // REAL-WORLD APPLICATIONS
    // • CPU task scheduling (first come, first served)
    // • Printer job queues
    // • Call center phone systems
    // • Breadth-First Search (BFS) in graphs
    // • Handling requests in web servers
    // • Buffer for data streams
    #endregion

    private readonly T[] _array;
    private readonly int _capacity;
    private int _count;
    private int _front;
    private int _rear;

    public TypeQueue(int capacity)
    {
        if (capacity < 0)
        {
            throw new ArgumentException("Capacity must be greater than 0", nameof(capacity));
        }

        _array = new T[capacity];
        _capacity = capacity;
        _count = 0;
        _front = -1;            // -1 indicates empty queue
        _rear = -1;             // -1 indicates empty queue
    }

    public int Count => _count;
    public int Capacity => _capacity;
    public bool IsEmpty => _count == 0;
    public bool IsFull => _count == _capacity;

    public void Enqueue(T item)
    {
        if (IsFull)
        {
            throw new InvalidOperationException("Cannot enqueue to a full queue");
        }

        // Initial:     FRONT = -1, REAR = -1, array = [_, _, _, _]
        // Enqueue(A):  FRONT = 0,  REAR = 0,  array = [A, _, _, _]
        // Enqueue(B):  FRONT = 0,  REAR = 1,  array = [A, B, _, _]
        // Enqueue(C):  FRONT = 0,  REAR = 2,  array = [A, B, C, _]
        // Dequeue():   FRONT = 1,  REAR = 2,  array = [_, B, C, _], returns A
        // Dequeue():   FRONT = 2,  REAR = 2,  array = [_, _, C, _], returns B
        // Enqueue(D):  FRONT = 2,  REAR = 3,  array = [_, _, C, D]

        // If this is the first element, set front to 0
        if (IsEmpty)
        {
            _rear = 0;
        }
        
        // Move rear pointer to next position
        _rear++;

        _count++;
        _array[_rear] = item;
    }

    public T Dequeue()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot dequeue from an empty queue");
        }

        T item = _array[_front];

        _array[_front] = default;

        _count--;

        if (IsEmpty)
        {
            _front = -1;
            _rear = -1;
        }
        else
        {
            _front++;
        }

        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot peek at an empty queue");
        }

        return _array[_front];
    }

    public T Front() => Peek();
    public T Back()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot get rear of an empty queue");
        }

        return _array[_rear];
    }

    public void Clear()
    {
        for (int i = _front; i < _rear; i++)
        {
            _array[i] = default;
        }

        _front = -1;
        _rear = -1;
        _count = 0;
    }

    public bool TryEnqueue(T item)
    {
        if (IsFull)
        {
            return false;
        }

        Enqueue(item);
        return true;
    }

    public bool TryDequeue(out T item)
    {
        if (IsEmpty)
        {
            item = default;
            return false;
        }

        item = Dequeue();
        return true;
    }

    public bool TryPeek(out T item)
    {
        if (IsEmpty)
        {
            item = default(T);
            return false;
        }

        item = Peek();
        return true;
    }

    public T[] ToArray()
    {
        T[] result = new T[_count];

        for (int i = 0; i < _count; i++)
        {
            result[i] = _array[_front + i];
        }

        return result;
    }

    public override string ToString()
    {
        if (IsEmpty)
        {
            return "Queue: Empty";
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Queue (front to rear):\n");

        for (int i = _front; i <= _rear; i++)
        {
            string position = i == _front ? " -> FRONT" : (i == _rear ? " -> REAR" : "");
            sb.AppendLine($" [{i}] {_array[i]} {position}");
        }

        sb.AppendLine($"Count: {Count}, Capacity: {Capacity}");
        return sb.ToString();
    }
}
