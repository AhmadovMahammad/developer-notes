using System.Collections;

namespace Chapter7
{
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
}