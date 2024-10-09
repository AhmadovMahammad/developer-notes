using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace Chapter7
{
    public class Program
    {
        private static void Main(string[] args)
        {
            /* Collections

            .NET provides a standard set of types for storing and managing collections of objects. 
            These include resizable lists, linked lists, sorted and unsorted dictionaries, and arrays. 
            Of these, only arrays form part of the C# language; 
            the remaining collections are just classes you instantiate like any other.

            NOTE:
            
            Enumeration-only interfaces: IEnumerable and IEnumerable<T> only allow 
            traversal of the collection (you can’t add/remove elements).

            Countable and rich functionality interfaces: ICollection<T>, IList<T>, and IDictionary<K,V> 
            allow more advanced functionalities like counting elements, adding, and removing items.

            */

            /* IEnumerable and IEnumerator

            The IEnumerator interface defines the basic low-level protocol
            by which elements in a collection are traversed—or enumerated—in a forward-only manner. 
            Its declaration is as follows:

            public interface IEnumerator
            {
                bool MoveNext();
                object Current { get; }
                void Reset();
            }

            MoveNext advances the current element or “cursor” to the next position, 
            returning false if there are no more elements in the collection. 
            Current returns the element at the current position

            MoveNext must be called before retrieving the first element. This is to allow for an empty collection.

            Collections do not usually implement enumerators; 
            instead, they provide enumerators, via the interface IEnumerable:

            public interface IEnumerable
            {
                IEnumerator GetEnumerator();
            }

            You can think of IEnumerable as “IEnumeratorProvider,” and 
            it is the most basic interface that collection classes implement.

            The following example illustrates low-level use of IEnumerable and IEnumerator:

            string s = "Hello";
            // Because string implements IEnumerable, we can call GetEnumerator():
            IEnumerator rator = s.GetEnumerator();

            while (rator.MoveNext())
            {
                char c = (char) rator.Current;
                Console.Write (c + ".");
            }

            // Output: H.e.l.l.o.


            ---Creating Custom Collection

            public class MyCollection : IEnumerable
            {
                private readonly List<int> _list = new();

                public void Add(int item)
                {
                    _list.Add(item);
                }

                public IEnumerator GetEnumerator()
                {
                    return new MyEnumerator(_list);
                }
            }

            public class MyEnumerator : IEnumerator
            {
                private readonly List<int> _list;
                private int _position = -1; // Enumerator's position (starts before the first element)

                public MyEnumerator(List<int> list)
                {
                    _list = list;
                }

                public object Current
                {
                    get
                    {
                        if (_position < 0 || _position >= _list.Count)
                        {
                            throw new IndexOutOfRangeException();
                        }

                        return _list[_position];
                    }
                }

                public bool MoveNext()
                {
                    _position++;
                    return _position < _list.Count;
                }

                public void Reset()
                {
                    _position = -1;
                }
            }

              MyCollection myCollection = new() { 10, 20, 30, 40 };
              IEnumerator enumerator = myCollection.GetEnumerator();

              while (enumerator.MoveNext())
              {
                  var current = enumerator.Current;
                  Console.Write(enumerator.Current + " ");
              }

            */

            /* IEnumerable<T> and IEnumerator<T>
             
            IEnumerator and IEnumerable are nearly always implemented in conjunction 
            with their extended generic versions:

            public interface IEnumerator<T> : IEnumerator, IDisposable
            {
                T Current { get; }
            }

            public interface IEnumerable<T> : IEnumerable
            {
                IEnumerator<T> GetEnumerator();
            }

            public class GenericCollection<T> : IEnumerable<T>
            {
                private readonly List<T> _list = new();

                public void Add(T item)
                {
                    _list.Add(item);
                }

                public IEnumerator<T> GetEnumerator()
                {
                    return new GenericEnumerator<T>(_list);
                }

                IEnumerator IEnumerable.GetEnumerator()
                {
                    return GetEnumerator();
                }
            }

            public class GenericEnumerator<T> : IEnumerator<T>
            {
                private readonly List<T> _list;
                private int _position = -1;

                public GenericEnumerator(List<T> list)
                {
                    _list = list;
                }

                public T Current
                {
                    get
                    {
                        if (_position < 0 || _position >= _list.Count)
                        {
                            throw new IndexOutOfRangeException();
                        }

                        return _list[_position];
                    }
                }

                object IEnumerator.Current => Current!;

                public bool MoveNext()
                {
                    _position++;
                    return _position < _list.Count;
                }

                public void Reset()
                {
                    _position = -1;
                }

                public void Dispose()
                {

                }
            }

            GenericCollection<int> genericCollection = new() { 10, 20, 30, 40, 50 };
            IEnumerator<int> enumerator = genericCollection.GetEnumerator();

            while (enumerator.MoveNext())
            {
                int cur = enumerator.Current;
                Console.WriteLine(cur);
            }

            GenericCollection<string> genericStringCollection = new() { "s10", "s20", "s30", "s40", "s50" };
            IEnumerator<string> enumeratorString = genericStringCollection.GetEnumerator();

            while (enumeratorString.MoveNext())
            {
                string cur = enumeratorString.Current;
                Console.WriteLine(cur);
            }

            */

            /* Why Generic versions inherit IDisposable
             
            The reason the generic version of IEnumerator<T> implements IDisposable while 
            the non-generic IEnumerator does not is mainly for resource management.

            1. Non-generic IEnumerator
             
            In early versions of .NET, the non-generic IEnumerator was quite basic, 
            designed just for moving through collections. 
            Most simple collections (like arrays, lists, etc.) didn’t need to release any unmanaged resources 
            (like file handles or network connections) during enumeration. 
            Therefore, the non-generic IEnumerator did not implement IDisposable because it wasn’t typically needed.

            2. Generic IEnumerator<T> and IDisposable

            When generic collections were introduced in .NET 2.0, 
            the designers wanted to make the new IEnumerator<T> more flexible and future-proof.
            With generic collections, there could be cases where the enumerator needs to manage 
            external or unmanaged resources during enumeration, 
            such as when dealing with files, streams, or database connections.
            By implementing IDisposable, IEnumerator<T> allows for proper cleanup of those resources when enumeration is finished.


            *** Why IEnumerable<T> Does Not Implement IDisposable

            IEnumerable<T> only provides an enumerator through GetEnumerator() and 
            does not directly involve resource management or lifecycle handling. 
            It is a lightweight interface that just represents a collection that can be enumerated, 
            but it leaves the actual traversal and potential resource management to the IEnumerator<T>. 
            Therefore, IEnumerable<T> doesn’t need to implement IDisposable. 
            The actual iteration logic is handled by IEnumerator<T>

            */

            /* Yield return statement
             
             public class BlackMagic : IEnumerable
             {
                 private readonly int[] _data = { 1, 2, 3 };
             
                 public IEnumerator GetEnumerator()
                 {
                     foreach (int i in _data)
                     {
                         yield return i;
                     }
                 }
             }

            var enumerable = new BlackMagic();

            Notice the “black magic”: GetEnumerator doesn’t appear to return an enumerator at all! 
            Upon parsing the yield return statement, the compiler writes a hidden nested enumerator class behind the scenes and 
            then refactors GetEnumerator to instantiate and return that class.

            */

            /* ICollection<T> and ICollection
            
            ICollection<T> is the standard interface for countable collections of objects. 
            It provides the ability to determine 
            
            1. the size of a collection (Count), 
            2. determine whether an item exists in the collection (Contains), 
            3. copy the collection into an array (ToArray),
            4. and determine whether the collection is read-only (IsReadOnly).

            For writable collections, you can also Add, Remove, and Clear items from the collection. 
            And because it extends IEnumerable<T>, it can also be traversed via the foreach statement:

            public interface ICollection<T> : IEnumerable<T>, IEnumerable
            {
                int Count { get; }


                bool Contains(T item);
                void CopyTo(T[] array, int arrayIndex);
                bool IsReadOnly { get; }

                void Add(T item);
                bool Remove(T item);
                void Clear();
            }
            
            The nongeneric ICollection is similar in providing a countable collection, 
            but it doesn’t provide functionality for altering the list or checking for element membership:

            public interface ICollection<T> : IEnumerable<T>, IEnumerable
            {
                int Count { get; }
                bool IsSynchronized { get; }
                object SyncRoot { get; }
                void CopyTo(T[] array, int arrayIndex);
            }

            Both interfaces are fairly straightforward to implement. 
            If implementing a read-only ICollection<T>, 
            the Add, Remove, and Clear methods should throw a NotSupportedException.

            The properties IsSynchronized and SyncRoot are part of the non-generic ICollection interface and 
            were used to provide a thread-safety mechanism for collections in earlier versions of .NET.

            ---Why use IsSynchronized and SyncRoot?

            IsSynchronized: This indicates whether access to the collection is thread-safe. 
            If it returns true, it means the collection is synchronized and can be safely accessed by multiple threads.

            SyncRoot: This property provides an object that can be used to lock the collection during multi-threaded access. 
            You can use SyncRoot with a lock statement to ensure only one thread accesses the collection at a time.

            However, these properties were more important before modern concurrency practices
            (like the use of concurrent collections or locks). 
            In generic collections (e.g., ICollection<T>), these properties were dropped, 
            as thread-safety was seen as something that should be managed separately 
            (using synchronization techniques like locks, ConcurrentDictionary, or BlockingCollection).

            public class MySynchronizedCollection : ICollection
            {
                private readonly ArrayList _items = new ArrayList();
                private readonly object _syncRoot = new object();

                public int Count
                {
                    get
                    {
                        lock (_syncRoot)
                        {
                            return _items.Count;
                        }
                    }
                }

                public bool IsSynchronized => true;
                public object SyncRoot => _syncRoot;

                public void Add(object item)
                {
                    lock (_syncRoot)
                    {
                        _items.Add(item);
                    }
                }

                public void Remove(object item)
                {
                    lock (_syncRoot)
                    {
                        _items.Remove(item);
                    }
                }

                public void CopyTo(Array array, int index)
                {
                    lock (_syncRoot)
                    {
                        _items.CopyTo(array, index);
                    }
                }

                public IEnumerator GetEnumerator()
                {
                    return _items.GetEnumerator();
                }
            }

            MySynchronizedCollection collection = new MySynchronizedCollection { 1, 2, 3 };
            lock (collection.SyncRoot)
            {
                foreach (var item in collection)
                {
                    Console.WriteLine(item); // Output: 1 2 3
                }
            }

            */

            /* IList<T> and IList
             
            IList<T> is the standard interface for collections indexable by position. 
            In addition to the functionality inherited from ICollection<T> and IEnumerable<T>, 
            it provides 
            
            1. the ability to read or write an element by position (via an indexer) and 
            2. insert / remove by position:

            public interface IList<T> : ICollection<T>, IEnumerable<T>, IEnumerable
            {
                T this[int index] { get; set; }
                void Insert(int index, T item);
                void RemoveAt(int index);
                int IndexOf(T item);
            }

            Note: 
            The IndexOf methods perform a linear search on the list, returning −1 if the specified item is not found.

            The Add method on the nongeneric IList interface returns an integer—this is the index of the newly added item. 
            In contrast, the Add method on ICollection<T> has a void return type.

            */

            /* The Array Class
             
            The Array class is the implicit base class for all single and multidimensional arrays, 
            and it is one of the most fundamental types implementing the standard collection interfaces. 
            The Array class provides type unification, so a common set of methods is available to all arrays, 
            regardless of their declaration or underlying element type.

            The CLR also treats array types specially upon construction, assigning them a contiguous space in memory. 
            This makes indexing into arrays highly efficient, but prevents them from being resized later on.
             
            An array can contain value-type or reference-type elements. 
            
            Value-type elements are stored in place in the array, so an array of three long integers (each 8 bytes) 
            will occupy 24 bytes of contiguous memory. 
            
            A reference type element, however, occupies only as much space in the array as a reference 
            (4 bytes in a 32-bit environment or 8 bytes in a 64-bit environment).

            StringBuilder[] builders = new StringBuilder [5];
            builders [0] = new StringBuilder ("builder1");
            builders [1] = new StringBuilder ("builder2");
            builders [2] = new StringBuilder ("builder3");
            
            long[] numbers = new long [3];
            numbers [0] = 12345;
            numbers [1] = 54321;

            1. Why is long 64 bits?
            The long data type in C# is a value type that holds a 64-bit integer (also known as Int64). 
            This means that whenever you declare a long array, 
            the actual values are stored in-place within the array itself.

            2. Why does StringBuilder use 32 or 64 bits?

            StringBuilder is a reference type.
            In C#, reference types are stored on the heap, 
            but the reference (or pointer) to that object is stored in the array.

            In this case, the StringBuilder[] array stores references to StringBuilder objects. 
            The size of each reference (whether 32 or 64 bits) depends on whether you're running on a 32-bit or 64-bit environment.


            1. For Value-Type Array (long[] numbers)
            
            Stack:
            +------------------+
            | numbers (array)  | --> +---------+---------+---------+
            |                  |     | 12345   | 54321   |    0    |
            +------------------+     +---------+---------+---------+
                                        (contiguous memory)
            
            Heap:
            (nothing, because values are directly stored on the stack)

            2. 
            Stack:
            +------------------+
            | builders (array) | --> +---------+---------+---------+---------+---------+
            |                  |     | Ref1    | Ref2    | Ref3    |  null   |  null   |
            +------------------+     +---------+---------+---------+---------+---------+
            
            Heap:
            +---------------------------+
            | StringBuilder object (Ref1)| --> "builder1"
            +---------------------------+
            +---------------------------+
            | StringBuilder object (Ref2)| --> "builder2"
            +---------------------------+
            +---------------------------+
            | StringBuilder object (Ref3)| --> "builder3"
            +---------------------------+

            Arrays can be duplicated by calling the Clone method: arrayB = arrayA.Clone().
            However, this results in a shallow clone, meaning that only the memory represented by the array itself is copied. 
            
            1. If the array contains value-type objects, the values themselves are copied; 
            2. if the array contains reference type objects, just the references are copied

            To create a deep copy—for which reference type subobjects are duplicated, 
            you must loop through the array and clone each element manually. 

            */

            /* HashSet<T> and Sorted<T>
            
            HashSet<T> and SortedSet<T> have the following distinguishing features:
            • Their Contains methods execute quickly using a hash-based lookup.
            • They do not store duplicate elements and silently ignore requests to add duplicates.
            • You cannot access an element by position.

            SortedSet<T> keeps elements in order, whereas HashSet<T> does not.

            Both collections implement ICollection<T> and offer methods that you would expect, such as Contains, Add, and Remove. 
            In addition, there’s a predicate-based removal method called RemoveWhere.

            1. Contains Methods Execute Quickly Using Hash-Based Lookup

            HashSet<T> uses a hash table under the hood, which provides fast lookups. 
            When you call Contains(item) on a HashSet<T>, 
            it calculates the hash code of the item and uses it to quickly locate the item in the collection.

            Why is it fast? 
            Hash tables allow for near-constant time complexity, typically O(1), 
            meaning the time it takes to find an element does not grow significantly as the set gets larger.

            SortedSet<T> uses a red/black tree (a self-balancing binary search tree), which allows for efficient searching, 
            It is slightly slower than hash-based lookups, with a time complexity of O(log n) for the Contains method.

            2. No Duplicates Allowed

            Both HashSet<T> and SortedSet<T> do not allow duplicates. 
            If you try to add an element that is already in the set, 
            it will be ignored without throwing an exception.

            HashSet<T> checks for duplicates using the hash code of the element. 
            If two elements have the same hash code (calculated via GetHashCode()), it considers them as duplicates.

            SortedSet<T> checks for duplicates by comparing elements with a sorting rule (based on IComparable<T> or a custom comparator) 
            because it maintains elements in a sorted order.

            3. Commonality in Interface: ISet<T> and IReadOnlySet<T>

            Both HashSet<T> and SortedSet<T> implement the ISet<T> interface. 
            This interface defines the common set operations, such as Add, Remove, Contains, 
            and set-specific operations like union and intersection.

            CODE EXAMPLES

            var hashSet1 = new HashSet<int> { 1, 2, 3, 4 };
            var hashSet2 = new HashSet<int> { 3, 4, 5, 6 };

            // Union: Combines elements from both sets, keeping only unique elements.
            hashSet1.UnionWith(hashSet2);
            Console.WriteLine("Union: " + string.Join(", ", hashSet1)); // Output: 1, 2, 3, 4, 5, 6

            UnionWith adds all the elements in the second set to the original set (excluding duplicates).

            // Intersection: Finds common elements between the two sets.
            var intersection = hashSet1.Intersect(hashSet2);
            Console.WriteLine("Intersection: " + string.Join(", ", intersection)); // Output: 3, 4, 5, 6
            Console.WriteLine("HashSet_1: " + string.Join(", ", hashSet1)); // Output: 1,2, 3, 4, 5, 6

            IntersectWith removes the elements that are not in both sets.

            // Difference: Finds elements in one set that are not in the other.
            var difference = new HashSet<int>(hashSet1);
            difference.ExceptWith(hashSet2);
            Console.WriteLine("Difference: " + string.Join(", ", difference)); // Output: 1, 2

            // Symmetric Difference: Finds elements that are in either of the sets but not in both.
            var symmetricDifference = new HashSet<int>(hashSet1);
            symmetricDifference.SymmetricExceptWith(hashSet2);
            Console.WriteLine("Symmetric Difference: " + string.Join(", ", symmetricDifference)); // Output: 1, 2, 5, 6

            ---
            SortedSet<T> offers all the members of HashSet<T>, plus the following:
            
            public virtual SortedSet<T> GetViewBetween (T lowerValue, T upperValue)
            public IEnumerable<T> Reverse()
            public T Min { get; }
            public T Max { get; }

            */

            /* Understanding Hash Codes, Buckets, and Collisions in a Dictionary
             
            1. Hash Code

            When you add a key to a dictionary, it is transformed into a hash code, 
            which is a numerical value (usually an integer) representing that key. 
            This hash code is essential for locating the corresponding value in the dictionary.

            Adding the key "apple" might generate a hash code of 123456.
            Adding the key "banana" might generate a hash code of 789012.

            2. Buckets

            The dictionary organizes its entries into buckets. Each bucket can hold one or more key-value pairs. 
            The hash code helps determine which bucket an entry belongs to.

            Bucket 0: | (Key: "apple", Value: 1)
            Bucket 1: | (Key: "banana", Value: 2)
            Bucket 2: | 
            Bucket 3: | 
            Bucket 4: | 

            If we add another entry with a hash code that points to the same bucket as "apple", it would look like this:

            Bucket 0: | (Key: "apple", Value: 1)
                      | (Key: "grape", Value: 3) // Collision occurs here
            Bucket 1: | (Key: "banana", Value: 2)
            Bucket 2: | 
            Bucket 3: | 
            Bucket 4: | 

            3. Handling Collisions

            Sometimes, two different keys can produce the same hash code, which is called a collision. 
            Here’s how the dictionary deals with it:

            Distribution: A good hash function aims to spread hash codes evenly across the available buckets, minimizing collisions.
            Linear Search: If a collision occurs (like with "apple" and "grape" in Bucket 0), 
            the dictionary will search through the entries in that bucket to find the correct key-value pair.

            The average time complexity for operations like adding, removing, and accessing items in a dictionary is O(1) (constant time), meaning it’s fast. 
            However, in cases of collisions where many items share the same bucket, 
            the time can increase to O(n) (linear time) because the dictionary may need to check each item in the bucket.

            */

            /* Ordered Dictionary
             
            An OrderedDictionary is a collection type in .NET that combines features of both a Hashtable and an ArrayList. 
            Here’s a breakdown of its key characteristics:

            1. Order Preservation: 

            The primary feature of OrderedDictionary is that it maintains the order of elements as they are added. 
            This means that if you add elements in a specific sequence, you can retrieve them in the same order.

            The term "primary feature" is used to highlight that the main distinction of OrderedDictionary from other dictionary types, 
            like a standard Dictionary<TKey, TValue>, is its ability to maintain the insertion order of elements.

            Clarification on Dictionary Behavior:

            1.1. Standard Dictionary:

            In a standard Dictionary<TKey, TValue>, elements are not guaranteed to maintain the order of insertion.
            The underlying structure of a standard dictionary uses a hash table, which organizes elements based on their hash codes. 
            As a result, the retrieval order may differ from the order in which elements were added.

            1.2. OrderedDictionary:

            In contrast, an OrderedDictionary explicitly maintains the order of elements as they are added.
            When you iterate over the elements of an OrderedDictionary, 
            you will retrieve them in the exact sequence you added them.

            Dictionary<string, int> dict = new Dictionary<string, int>();
            dict.Add("One", 1);
            dict.Add("Two", 2);
            dict.Add("Three", 3);

            Console.WriteLine("Dictionary output:");
            foreach (var kv in dict)
            {
                Console.WriteLine($"{kv.Key}: {kv.Value}");
            }

            Output could vary: The order of elements may not match the order of insertion 
            (the output order could be Three, One, Two, etc.).

            Summary:
            OrderedDictionary: Guarantees retrieval in the order of insertion.
            Dictionary<TKey, TValue>: Does not guarantee order, as it organizes items based on hash codes.


            2. Key and Index Access: 

            Unlike a standard Hashtable, which allows access only by key, 
            an OrderedDictionary lets you access elements both 1. by their key (like in a dictionary) and 2. by their index (like in an array). 
            This dual access method makes it more flexible in certain scenarios.

            3. Functionality: 
            It has the methods and properties of both 
            
            1. a Hashtable (like Add, Remove, and Contains) and 
            2. an ArrayList (like RemoveAt and an integer indexer). 

            It also provides properties like Keys and Values, which return the keys and values in the order they were added.

            */

        }
    }
}