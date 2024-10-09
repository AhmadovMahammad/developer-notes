using System.Collections;

namespace Chapter7
{
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
}