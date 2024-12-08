namespace AdvancedThreading;

public class UnsafeThread
{
    private readonly object _lock = new object();
    private readonly int A = 1;
    private int B = 1;

    public void Start(bool isInitial)
    {
        if (B != 0)
        {
            Console.WriteLine(A / B);
        }
        B = 0;
    }
}

public class BankAccount
{
    private readonly object _lock = new object();
    private int _balance;

    public BankAccount(int initialBalance)
    {
        _balance = initialBalance;
    }

    //public void Deposit(int amount)
    //{
    //    Thread.Sleep(1);
    //    _balance += amount;
    //}

    //public void Withdraw(int amount)
    //{
    //    Thread.Sleep(1);
    //    if (_balance >= amount) _balance -= amount;
    //}

    public void Deposit(int amount)
    {
        lock (_lock)
        {
            Thread.Sleep(1);
            _balance += amount;
        }
    }

    public void Withdraw(int amount)
    {
        lock (_lock)
        {
            Thread.Sleep(1);
            if (_balance >= amount) _balance -= amount;
        }
    }

    public int GetBalance() => _balance;
}
