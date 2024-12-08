namespace ConcurrencyAndAsynchrony_ch14;

// Task.Run() Example
//public class PaymentProcessor
//{
//    public Task<bool> ProcessPaymentAsync()
//    {
//        return Task.Run(async () =>
//        {
//            Console.WriteLine("PaymentProcessor: Processing Payment...");
//            await Task.Delay(2 * 1000);
//            Console.WriteLine("PaymentProcessor: Payment Successful!");

//            return true;
//        });
//    }
//}

// TaskCompletionSource Example
public class PaymentProcessor
{
    private TaskCompletionSource<bool> _paymentCompletionSource = new TaskCompletionSource<bool>();
    public Task<bool> PaymentTask => _paymentCompletionSource.Task;

    public void StartPaymentProcessing()
    {
        Console.WriteLine("PaymentProcessor: Starting payment processing...");

        new Thread(async () =>
        {
            await Task.Delay(2 * 1000);
            Console.WriteLine("PaymentProcessor: Payment processing complete.");
            _paymentCompletionSource.SetResult(true);
        }).Start();
    }
}
