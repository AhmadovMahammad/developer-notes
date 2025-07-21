namespace ConcurrencyAndAsynchrony_ch14;

public class Foo // LeetCode Question
{
    private readonly ManualResetEvent firstDone = new ManualResetEvent(false);
    private readonly ManualResetEvent secondDone = new ManualResetEvent(false);

    public Foo() { }

    public void First(Action printFirst)
    {
        printFirst();
        firstDone.Set();
    }

    public void Second(Action printSecond)
    {
        firstDone.WaitOne();

        printSecond();

        secondDone.Set();
    }

    public void Third(Action printThird)
    {
        secondDone.WaitOne();

        printThird();
    }
}
