namespace AdvancedThreading_ch21;

public class Bank
{
    private readonly object _lock = new object();
    public double balance { get; private set; } = 1500;

    public void WithdrawWithLock(double amount)
    {
        lock (_lock)
        {
            if (balance >= amount)
            {
                Console.WriteLine($"Transaction started: Withdrawing {amount}");
                balance -= amount;
                Console.WriteLine($"Transaction completed: {amount} withdrawn. Current balance: {balance}");
                LogTransaction($"Withdrawn: {amount}");
            }
            else
            {
                Console.WriteLine("Insufficient funds!");
            }
        }
    }

    private void LogTransaction(string message)
    {
        lock (_lock) // Same lock, reentrant behavior allows it
        {
            Console.WriteLine($"Logging transaction: {message}");
            Thread.Sleep(100);
        }
    }
}
