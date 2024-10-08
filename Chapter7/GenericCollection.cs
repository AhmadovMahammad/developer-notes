using System.Collections;
using System.Collections.Generic;

namespace Chapter7
{
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
}
