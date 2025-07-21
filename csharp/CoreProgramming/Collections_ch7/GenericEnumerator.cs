using System.Collections;

namespace Chapter7;

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
