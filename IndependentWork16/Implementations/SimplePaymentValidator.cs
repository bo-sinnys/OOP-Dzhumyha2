using IndependentWork16.Interfaces;

namespace IndependentWork16.Implementations
{
    public class SimplePaymentValidator : IPaymentValidator
    {
        public bool Validate(string cardNumber, decimal amount)
        {
            return !string.IsNullOrWhiteSpace(cardNumber) && amount > 0;
        }
    }
}