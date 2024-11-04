namespace ConcurrencyAndAsynchrony_ch14
{
    // Task.Run() Example
    //public class OrderService
    //{
    //    private readonly PaymentProcessor _paymentProcessor = new PaymentProcessor();

    //    public async Task PlaceOrderAsync()
    //    {
    //        Console.WriteLine("OrderService: Placing order and waiting for payment...");

    //        // Start payment processing and immediately wait for result
    //        bool paymentStatus = await _paymentProcessor.ProcessPaymentAsync();

    //        if (paymentStatus) Console.WriteLine("OrderService: Payment successful, order is complete.");
    //        else Console.WriteLine("OrderService: Payment failed, order is canceled.");
    //    }
    //}

    // TaskCompletionSource Example
    public class OrderService
    {
        private readonly PaymentProcessor _paymentProcessor = new PaymentProcessor();

        public async Task PlaceOrderAsync()
        {
            Console.WriteLine("OrderService: Placing order and waiting for payment...");

            _paymentProcessor.StartPaymentProcessing();

            bool paymentStatus = await _paymentProcessor.PaymentTask;

            if (paymentStatus) Console.WriteLine("OrderService: Payment successful, completing order.");
            else Console.WriteLine("OrderService: Payment failed, order canceled.");
        }
    }
}
