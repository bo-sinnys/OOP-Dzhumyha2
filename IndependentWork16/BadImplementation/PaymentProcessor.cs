using System;

namespace IndependentWork16.BadImplementation
{
    public class PaymentProcessor
    {
        public void ProcessPayment(string cardNumber, decimal amount)
        {
            if (string.IsNullOrWhiteSpace(cardNumber) || amount <= 0)
            {
                Console.WriteLine("Invalid payment data");
                return;
            }

            Console.WriteLine($"Charging {amount} from card {cardNumber}");
            Console.WriteLine($"Transaction logged at {DateTime.Now}");
            Console.WriteLine("SMS sent to client");
        }
    }
}