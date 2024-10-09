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
}