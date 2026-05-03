using lab31v1.Models;

namespace lab31v1.Interfaces;

public interface IBookingRepository
{
    Booking? GetById(int id);
    IEnumerable<Booking> GetAll();
    void Add(Booking booking);
    void Update(Booking booking);
    void Delete(int id);
    bool Exists(int id);
}
