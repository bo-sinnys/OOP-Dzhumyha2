using lab20.Models;

namespace lab20.Interfaces
{
    public interface IOrderRepository
    {
        void Save(Order order);
        Order GetById(int id);
    }
}