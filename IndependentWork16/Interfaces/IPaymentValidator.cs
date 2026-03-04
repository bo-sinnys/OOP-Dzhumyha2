namespace IndependentWork16.Interfaces
{
    public interface IPaymentValidator
    {
        bool Validate(string cardNumber, decimal amount);
    }
}