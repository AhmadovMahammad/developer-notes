using System.Collections;

namespace Chapter7;

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