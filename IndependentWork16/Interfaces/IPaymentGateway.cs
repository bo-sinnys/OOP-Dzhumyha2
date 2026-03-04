namespace IndependentWork16.Interfaces
{
    public interface IPaymentGateway
    {
        void Charge(string cardNumber, decimal amount);
    }
}