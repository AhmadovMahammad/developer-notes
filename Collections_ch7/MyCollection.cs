using System.Collections;

namespace Chapter7
{
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
}