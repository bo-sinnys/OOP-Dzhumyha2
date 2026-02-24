using lab20.Models;

namespace lab20.Interfaces
{
    public interface IOrderValidator
    {
        bool IsValid(Order order);
    }
}