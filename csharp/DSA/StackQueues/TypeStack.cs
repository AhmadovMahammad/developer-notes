using System.Numerics;
using System.Text;

namespace StackQueues;
public class TypeStack<T>
{
    #region Stack Documentation
    //
    // A stack is a linear data structure that follows the LIFO principle:
    // LIFO = Last In, First Out
    // 
    // Think of it like a pile of plates:
    // - You can only add plates to the top
    // - You can only remove plates from the top
    // - To get the bottom plate, you must remove all plates above it
    // 
    //
    //Visual representation:
    // 
    //     [3] ← TOP (last added, first to be removed)
    //     [2]
    //     [1] ← BOTTOM (first added, last to be removed)
    // 
    //
    // BASIC OPERATIONS
    // 
    // • PUSH: Add element to top of stack
    // • POP: Remove and return top element
    // • PEEK/TOP: View top element without removing it
    // • ISEMPTY: Check if stack has no elements
    // • ISFULL: Check if stack has reached maximum capacity
    // • SIZE: Get current number of elements
    //
    //
    // HOW IT WORKS
    // 
    // 1. We use an array to store stack elements
    // 2. A pointer called _top tracks the index of the top element
    // 3. Initially _top = -1 (empty stack)
    // 4. When we PUSH: increment _top, then add element at _top position
    // 5. When we POP: return element at _top position, then decrement _top
    //
    //
    // Example operations:
    // Initial: _top = -1, array = [_, _, _]
    // Push(1): _top = 0,  array = [1, _, _]
    // Push(2): _top = 1,  array = [1, 2, _]
    // Push(3): _top = 2,  array = [1, 2, 3]
    // Pop():   _top = 1,  array = [1, 2, _], returns 3
    // Pop():   _top = 0,  array = [1, _, _], returns 2
    //
    //
    // TIME COMPLEXITY
    // • Push: O(1) - constant time
    // • Pop: O(1) - constant time
    // • Peek: O(1) - constant time
    // • IsEmpty: O(1) - constant time
    // • IsFull: O(1) - constant time
    // 
    //
    // SPACE COMPLEXITY
    // • O(n) where n is the maximum capacity of the stack

    #endregion

    private readonly T[] _array;
    private int _top;
    private readonly int _capacity;

    public TypeStack(int capacity)
    {
        if (capacity <= 0)
        {
            throw new ArgumentException("Capacity must be greater than 0", nameof(capacity));
        }

        _array = new T[capacity];
        _top = -1;
        _capacity = capacity;
    }

    // Public Properties
    public int Count => _top + 1;
    public int Capacity => _capacity;
    public bool IsFull => _top + 1 == _capacity;
    public bool IsEmpty => _top == -1;

    // Core Stack Operations
    public void Push(T item)
    {
        if (IsFull)
        {
            throw new InvalidOperationException("Cannot push to a full stack");
        }

        // Increment top pointer and add element
        _top++;
        _array[_top] = item;
    }

    public T Pop()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot pop from an empty stack");
        }

        T item = _array[_top];
        _top--;

        return item;
    }

    public T Peek()
    {
        if (IsEmpty)
        {
            throw new InvalidOperationException("Cannot peek at an empty stack");
        }

        return _array[_top];
    }

    // Utility Methods
    public void Clear()
    {
        for (int i = 0; i < _top; i++)
        {
            _array[i] = default;
        }

        _top = -1;
    }

    public bool TryPush(T item)
    {
        if (IsFull)
        {
            return false;
        }

        // Increment top pointer and add element
        Push(item);
        return true;
    }

    public bool TryPop(out T item)
    {
        if (IsEmpty)
        {
            item = default;
            return false;
        }

        item = Pop();
        return true;
    }

    public bool TryPeek(out T item)
    {
        if (IsEmpty)
        {
            item = default;
            return false;
        }

        item = Peek();
        return true;
    }

    public T[] ToArray()
    {
        T[] result = new T[Count];

        for (int i = 0; i < result.Length; i++)
        {
            result[i] = _array[_top - i];
        }

        return result;
    }

    public override string ToString()
    {
        if (IsEmpty)
        {
            return "Stack: Empty";
        }

        StringBuilder sb = new StringBuilder();
        sb.AppendLine("Stack (top to bottom):");

        for (int i = _top; i >= 0; i--)
        {
            sb.AppendLine($"  [{i}] {_array[i]}");
        }

        sb.AppendLine($"Count: {Count}, Capacity: {Capacity}");
        return sb.ToString();
    }
}
