using lab20.Models;

namespace lab20.Interfaces
{
    public interface IEmailService
    {
        void SendOrderConfirmation(Order order);
    }
}