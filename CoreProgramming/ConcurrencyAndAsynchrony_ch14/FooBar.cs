namespace ConcurrencyAndAsynchrony_ch14
{
    public class FooBar
    {
        private ManualResetEvent fooEvent = new ManualResetEvent(false);
        private ManualResetEvent barEvent = new ManualResetEvent(false);
        private int n;

        public FooBar(int n)
        {
            this.n = n;
            fooEvent.Set();
        }

        public void Foo(Action printFoo)
        {
            for (int i = 0; i < n; i++)
            {
                fooEvent.WaitOne();
                printFoo();
                barEvent.Set();
                fooEvent.Reset();
            }
        }

        public void Bar(Action printBar)
        {
            for (int i = 0; i < n; i++)
            {
                barEvent.WaitOne();
                printBar();
                fooEvent.Set();
                barEvent.Reset();
            }
        }
    }
}
