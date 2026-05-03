using lab31v1.Interfaces;
using lab31v1.Models;

namespace lab31v1.Services;

public class BookingService
{
    private readonly IBookingRepository _repository;
    private readonly INotificationService _notificationService;

    public BookingService(IBookingRepository repository, INotificationService notificationService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }

    /// <summary>
    /// Створює нове бронювання та надсилає підтвердження.
    /// </summary>
    public void CreateBooking(Booking booking, string email)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email не може бути порожнім.", nameof(email));
        if (booking.CheckOut <= booking.CheckIn)
            throw new ArgumentException("Дата виїзду має бути пізніше дати заїзду.");

        booking.IsConfirmed = true;
        _repository.Add(booking);
        _notificationService.SendConfirmation(booking.GuestName, email, booking.Id);
    }

    /// <summary>
    /// Скасовує існуюче бронювання та надсилає повідомлення.
    /// </summary>
    public void CancelBooking(int bookingId, string email)
    {
        if (!_repository.Exists(bookingId))
            throw new InvalidOperationException($"Бронювання з Id={bookingId} не знайдено.");

        var booking = _repository.GetById(bookingId)!;
        _repository.Delete(bookingId);
        _notificationService.SendCancellation(booking.GuestName, email, bookingId);
    }

    /// <summary>
    /// Повертає бронювання за Id.
    /// </summary>
    public Booking? GetBooking(int id) => _repository.GetById(id);

    /// <summary>
    /// Повертає всі бронювання.
    /// </summary>
    public IEnumerable<Booking> GetAllBookings() => _repository.GetAll();

    /// <summary>
    /// Надсилає нагадування для всіх підтверджених бронювань.
    /// </summary>
    public void SendReminders(string email)
    {
        var confirmed = _repository.GetAll().Where(b => b.IsConfirmed);
        foreach (var booking in confirmed)
            _notificationService.SendReminder(booking.GuestName, email, booking.CheckIn);
    }

    /// <summary>
    /// Оновлює дані бронювання.
    /// </summary>
    public void UpdateBooking(Booking booking)
    {
        if (booking == null) throw new ArgumentNullException(nameof(booking));
        if (!_repository.Exists(booking.Id))
            throw new InvalidOperationException($"Бронювання з Id={booking.Id} не знайдено.");

        _repository.Update(booking);
    }
}
