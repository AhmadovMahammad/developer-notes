using System.Text;

namespace StackQueues;
public class TypeCircularQueue<T>
{
    #region Circular Queue Documentation
    //
    // What is a Circular Queue?
    //
    // A Circular Queue is an advanced version of a regular queue
    // where the last position is connected back to the first position,
    // forming a circle-like structure. It's also known as a Ring Buffer.
    // 
    //
    // Regular Queue:     [0] [1] [2] [3] [4]
    //                     ↑               ↑
    //                   start           end
    // 
    // Circular Queue:    [0] ← [4]
    //                     ↓     ↑
    //                    [1] → [3]
    //                     ↓     ↑
    //                     → [2] →
    // 
    // Why Do We Need Circular Queues?
    // Problem with Regular Queues
    // 
    // In a regular queue, after several enqueue and dequeue operations, we get wasted space at the beginning:
    // 
    // Initial:     [A] [B] [C] [D] [E]
    //              ↑F              ↑R
    // 
    // After 2 dequeues:  [_] [_] [C] [D] [E]
    //                            ↑F      ↑R
    // 
    // After 1 more enqueue: [_] [_] [C] [D] [E] [F] ← ERROR! Array is full!
    //                               ↑F          ↑R
    // 
    // Problem: Positions 0 and 1 are empty but unusable!
    // The queue appears "full" even though there's space available.
    // 
    // Solution with Circular Queue
    // The circular queue reuses the empty spaces by wrapping around:
    //
    //
    // After 2 dequeues:  [_] [_] [C] [D] [E]
    //                            ↑F      ↑R
    // 
    // After 1 more enqueue: [F] [_] [C] [D] [E] ← REAR wraps to position 0!
    //                            ↑F  ↑R
    //
    //
    // Result: No space is wasted! The queue can use all available positions efficiently.
    //
    //
    // How Circular Queue Works
    // The "circular" behavior is achieved using modulo division (`%` operator):
    // 
    // Next position = (current_position + 1) % array_size
    // 
    // Examples with array size 5:
    // - Position 0: next = (0 + 1) % 5 = 1
    // - Position 1: next = (1 + 1) % 5 = 2
    // - Position 2: next = (2 + 1) % 5 = 3
    // - Position 3: next = (3 + 1) % 5 = 4
    // - Position 4: next = (4 + 1) % 5 = 0 ← Wraps around!
    //
    //
    // Pointers in Circular Queue
    // 
    // - FRONT: Points to the first element (next to be removed)
    // - REAR: Points to the last element (most recently added)
    // - Initially: Both FRONT and REAR = -1 (empty queue)
    //
    //
    //
    //Enqueue Operation (Adding Elements)
    // 
    // 1. Check if queue is full
    // 2. For first element: set FRONT = 0
    // 3. Move REAR circularly: `REAR = (REAR + 1) % SIZE`
    // 4. Add element at REAR position
    // 
    // Example with array size 5:
    // 
    // Initial State:
    // Array: [_] [_] [_] [_] [_]
    // FRONT = -1, REAR = -1
    // 
    // Step 1: Enqueue(A)
    // Array: [A] [_] [_] [_] [_]
    // FRONT = 0, REAR = 0
    // 
    // Step 2: Enqueue(B)
    // Array: [A] [B] [_] [_] [_]
    // FRONT = 0, REAR = 1
    // 
    // Step 3: Enqueue(C)
    // Array: [A] [B] [C] [_] [_]
    // FRONT = 0, REAR = 2
    // 
    // Step 4: Enqueue(D)
    // Array: [A] [B] [C] [D] [_]
    // FRONT = 0, REAR = 3
    // 
    // Step 5: Enqueue(E)
    // Array: [A] [B] [C] [D] [E]
    // FRONT = 0, REAR = 4 (Queue is now full!)
    //
    //
    //
    // Dequeue Operation (Removing Elements)
    // 
    // 1. Check if queue is empty
    // 2. Get element at FRONT position
    // 3. Move FRONT circularly: `FRONT = (FRONT + 1) % SIZE`
    // 4. If queue becomes empty: reset FRONT and REAR to -1
    //
    //
    // Continuing from above example:
    // 
    // Step 6: Dequeue() → Returns A
    // Array: [_] [B] [C] [D] [E]
    // FRONT = 1, REAR = 4
    // 
    // Step 7: Dequeue() → Returns B
    // Array: [_] [_] [C] [D] [E]
    // FRONT = 2, REAR = 4
    // 
    // Step 8: Enqueue(F) → REAR wraps around!
    // Array: [F] [_] [C] [D] [E]
    // FRONT = 2, REAR = 0 (wrapped around!)
    // 
    // Step 9: Enqueue(G) → Continue wrapping
    // Array: [F] [G] [C] [D] [E]
    // FRONT = 2, REAR = 1
    // 
    //
    // Detecting Full vs Empty Queue
    // This is the trickiest part of circular queues!
    // 
    // Empty Queue
    // FRONT == -1 && REAR == -1
    // 
    // Full Queue (Two Cases)
    // Case 1: Normal full condition
    //
    //FRONT == 0 && REAR == SIZE - 1
    // Example: FRONT = 0, REAR = 4 (in size 5 array)
    //
    //
    // Case 2: Circular full condition
    // FRONT == REAR + 1
    // Example: FRONT = 2, REAR = 1
    // 
    // Why Case 2 happens:
    // Array: [F] [G] [C] [D] [E]
    //        ↑R      ↑F
    // 
    // REAR is at position 1, FRONT is at position 2
    // If we try to add one more element, REAR would move to position 2
    // But FRONT is already at position 2! → Collision → Queue is full
    // 
    // Visual Examples
    // Example 1: Normal Operations
    //
    // Size 4 Array:
    // 
    // Empty:     [_] [_] [_] [_]    F=-1, R=-1
    // Add A:     [A] [_] [_] [_]    F=0,  R=0
    // Add B:     [A] [B] [_] [_]    F=0,  R=1
    // Add C:     [A] [B] [C] [_]    F=0,  R=2
    // Add D:     [A] [B] [C] [D]    F=0,  R=3  (FULL!)
    // 
    // Example 2: Circular Wrapping
    //
    // Remove A:  [_] [B] [C] [D]    F=1,  R=3
    // Remove B:  [_] [_] [C] [D]    F=2,  R=3
    // Add E:     [E] [_] [C] [D]    F=2,  R=0  (wrapped!)
    // Add F:     [E] [F] [C] [D]    F=2,  R=1  (FULL again!)
    // 
    // Advantages of Circular Queue
    // 
    // 1. Memory Efficiency: No space is wasted
    // 2. Better Performance: Constant time operations O(1)
    // 3. Continuous Operation: Can keep adding/removing without resetting
    // 4. Fixed Size: Memory usage is predictable
    // 
    // Disadvantages
    // 
    // 1. Complex Implementation: More logic needed for wraparound
    // 2. Fixed Size: Cannot grow dynamically
    // 3. Full/Empty Detection: Requires careful condition checking
    // 
    // Real-World Applications
    // 
    // 1. Buffer Management: 
    //    - Audio/Video streaming buffers
    //    - Network packet buffers
    //    - Keyboard input buffers
    // 
    // 2. Operating Systems:
    //    - CPU scheduling (Round Robin)
    //    - Print job queues
    //    - Process management
    // 
    // 3. Gaming:
    //    - Turn-based games
    //    - Animation frame buffers
    //    - Sound effect queues
    // 
    // 4. IoT Devices:
    //    - Sensor data collection
    //    - Limited memory devices
    //    - Real-time data processing
    // 
    // Key Formulas
    // 
    // Next Position:     (current + 1) % size
    // Previous Position: (current - 1 + size) % size
    // Distance:          (rear - front + size) % size
    // Is Full:           (rear + 1) % size == front
    // Is Empty:          front == -1
    // 
    //
    // Memory Comparison
    // 
    // Regular Queue (size 5):
    // After operations: [_] [_] [C] [D] [_]
    // Usable space: 2/5 = 40%
    //
    //
    // Circular Queue (size 5):
    // After operations: [E] [F] [C] [D] [G]
    // Usable space: 5/5 = 100%
    // 
    #endregion

    private readonly T[] _array;
    private readonly int _capacity;
    private int _count;
    private int _front;
    private int _rear;

    public TypeCircularQueue(int capacity)
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
    public bool IsEmpty()
    {
        return _front == -1;
    }

    public bool IsFull()
    {
        // case 1
        if (_front == 0 && _rear == Capacity - 1)
        {
            return true;
        }

        if (_front == _rear + 1)
        {
            return true;
        }

        return false;
    }

    public void Enqueue(T item)
    {
        if (IsFull())
        {
            throw new InvalidOperationException("Cannot enqueue to a full queue");
        }

        if (IsEmpty())
        {
            _front = 0;
        }

        // Move rear pointer to next position
        _rear = (_rear + 1) % Capacity;

        _array[_rear] = item;
        _count++;
    }

    public T Dequeue()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Cannot dequeue from an empty queue");
        }

        T item = _array[_front];

        _array[_front] = default;

        if (_front == _rear) // means there was only one element in array
        {
            _front = -1;
            _rear = -1;
        }
        else
        {
            _front = (_front + 1) % Capacity;
        }

        return item;
    }

    public T Peek()
    {
        if (IsEmpty())
        {
            throw new InvalidOperationException("Cannot peek at an empty queue");
        }

        return _array[_front];
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

    public override string ToString()
    {
        if (IsEmpty())
        {
            return "Queue: Empty";
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Queue (front to rear):\n");

        for (int i = 0; i < _array.Length; i++)
        {
            string position = i == _front ? " -> FRONT" : (i == _rear ? " -> REAR" : "");
            sb.AppendLine($" [{i}] {_array[i]} {position}");
        }

        sb.AppendLine($"Count: {Count}, Capacity: {Capacity}");
        return sb.ToString();
    }
}
