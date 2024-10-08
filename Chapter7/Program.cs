using System.Collections;

namespace Chapter7
{
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
        }
        // 345
    }
}