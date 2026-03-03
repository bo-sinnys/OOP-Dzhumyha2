using System;
using IndependentWork16.Interfaces;

namespace IndependentWork16.Implementations
{
    public class MockPaymentGateway : IPaymentGateway
    {
        public void Charge(string cardNumber, decimal amount)
        {
            Console.WriteLine($"[Gateway] Charged {amount} from card {cardNumber}");
        }
    }
}