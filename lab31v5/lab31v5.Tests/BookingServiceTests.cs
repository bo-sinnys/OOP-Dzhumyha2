using lab31v1.Interfaces;
using lab31v1.Models;
using lab31v1.Services;
using Moq;

namespace lab31v1.Tests;

public class BookingServiceTests
{
    // ── Допоміжні методи ──────────────────────────────────────────────────────

    private static (BookingService service, Mock<IBookingRepository> repoMock, Mock<INotificationService> notifMock)
        CreateSut()
    {
        var repoMock = new Mock<IBookingRepository>();
        var notifMock = new Mock<INotificationService>();
        var service = new BookingService(repoMock.Object, notifMock.Object);
        return (service, repoMock, notifMock);
    }

    private static Booking MakeBooking(int id = 1) => new()
    {
        Id = id,
        GuestName = "Іван Петренко",
        RoomNumber = "101",
        CheckIn = DateTime.Today.AddDays(1),
        CheckOut = DateTime.Today.AddDays(3)
    };

    // ── Тест 1: CreateBooking — репозиторій викликає Add ──────────────────────

    [Fact]
    public void CreateBooking_ValidBooking_CallsRepositoryAdd()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking();

        sut.CreateBooking(booking, "guest@example.com");

        repoMock.Verify(r => r.Add(booking), Times.Once);
    }

    // ── Тест 2: CreateBooking — надсилається підтвердження ───────────────────

    [Fact]
    public void CreateBooking_ValidBooking_SendsConfirmationNotification()
    {
        var (sut, _, notifMock) = CreateSut();
        var booking = MakeBooking();

        sut.CreateBooking(booking, "guest@example.com");

        notifMock.Verify(
            n => n.SendConfirmation(booking.GuestName, "guest@example.com", booking.Id),
            Times.Once);
    }

    // ── Тест 3: CreateBooking — IsConfirmed виставляється у true ─────────────

    [Fact]
    public void CreateBooking_ValidBooking_SetsIsConfirmedTrue()
    {
        var (sut, _, _) = CreateSut();
        var booking = MakeBooking();

        sut.CreateBooking(booking, "guest@example.com");

        Assert.True(booking.IsConfirmed);
    }

    // ── Тест 4: CreateBooking — невалідні дати кидають виняток ───────────────

    [Fact]
    public void CreateBooking_InvalidDates_ThrowsArgumentException()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking();
        booking.CheckOut = booking.CheckIn; // рівні дати — помилка

        Assert.Throws<ArgumentException>(() => sut.CreateBooking(booking, "guest@example.com"));
        repoMock.Verify(r => r.Add(It.IsAny<Booking>()), Times.Never);
    }

    // ── Тест 5: CancelBooking — видаляє з репозиторію ────────────────────────

    [Fact]
    public void CancelBooking_ExistingBooking_CallsRepositoryDelete()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking();

        repoMock.Setup(r => r.Exists(booking.Id)).Returns(true);
        repoMock.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.CancelBooking(booking.Id, "guest@example.com");

        repoMock.Verify(r => r.Delete(booking.Id), Times.Once);
    }

    // ── Тест 6: CancelBooking — надсилає повідомлення про скасування ─────────

    [Fact]
    public void CancelBooking_ExistingBooking_SendsCancellationNotification()
    {
        var (sut, repoMock, notifMock) = CreateSut();
        var booking = MakeBooking();

        repoMock.Setup(r => r.Exists(booking.Id)).Returns(true);
        repoMock.Setup(r => r.GetById(booking.Id)).Returns(booking);

        sut.CancelBooking(booking.Id, "guest@example.com");

        notifMock.Verify(
            n => n.SendCancellation(booking.GuestName, "guest@example.com", booking.Id),
            Times.Once);
    }

    // ── Тест 7: CancelBooking — неіснуюче Id кидає InvalidOperationException ─

    [Fact]
    public void CancelBooking_NonExistentId_ThrowsInvalidOperationException()
    {
        var (sut, repoMock, notifMock) = CreateSut();
        repoMock.Setup(r => r.Exists(99)).Returns(false);

        Assert.Throws<InvalidOperationException>(() => sut.CancelBooking(99, "guest@example.com"));
        notifMock.Verify(
            n => n.SendCancellation(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>()),
            Times.Never);
    }

    // ── Тест 8: GetBooking — повертає бронювання з репозиторію ───────────────

    [Fact]
    public void GetBooking_ExistingId_ReturnsBookingFromRepository()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking();
        repoMock.Setup(r => r.GetById(booking.Id)).Returns(booking);

        var result = sut.GetBooking(booking.Id);

        Assert.Equal(booking, result);
        repoMock.Verify(r => r.GetById(booking.Id), Times.Once);
    }

    // ── Тест 9: SendReminders — надсилає нагадування лише підтвердженим ──────

    [Fact]
    public void SendReminders_OnlyConfirmedBookings_SendsReminderForEachConfirmed()
    {
        var (sut, repoMock, notifMock) = CreateSut();
        var confirmed = MakeBooking(1);
        confirmed.IsConfirmed = true;
        var unconfirmed = MakeBooking(2);
        unconfirmed.IsConfirmed = false;

        repoMock.Setup(r => r.GetAll()).Returns(new[] { confirmed, unconfirmed });

        sut.SendReminders("guest@example.com");

        notifMock.Verify(
            n => n.SendReminder(confirmed.GuestName, "guest@example.com", confirmed.CheckIn),
            Times.Once);
        notifMock.Verify(
            n => n.SendReminder(unconfirmed.GuestName, It.IsAny<string>(), It.IsAny<DateTime>()),
            Times.Never);
    }

    // ── Тест 10: UpdateBooking — оновлює існуюче бронювання ──────────────────

    [Fact]
    public void UpdateBooking_ExistingBooking_CallsRepositoryUpdate()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking();
        repoMock.Setup(r => r.Exists(booking.Id)).Returns(true);

        sut.UpdateBooking(booking);

        repoMock.Verify(r => r.Update(booking), Times.Once);
    }

    // ── Тест 11: UpdateBooking — неіснуюче Id кидає виняток ──────────────────

    [Fact]
    public void UpdateBooking_NonExistentId_ThrowsInvalidOperationException()
    {
        var (sut, repoMock, _) = CreateSut();
        var booking = MakeBooking(42);
        repoMock.Setup(r => r.Exists(42)).Returns(false);

        Assert.Throws<InvalidOperationException>(() => sut.UpdateBooking(booking));
        repoMock.Verify(r => r.Update(It.IsAny<Booking>()), Times.Never);
    }

    // ── Тест 12: GetAllBookings — повертає всі записи репозиторію ────────────

    [Fact]
    public void GetAllBookings_ReturnsAllFromRepository()
    {
        var (sut, repoMock, _) = CreateSut();
        var bookings = new[] { MakeBooking(1), MakeBooking(2) };
        repoMock.Setup(r => r.GetAll()).Returns(bookings);

        var result = sut.GetAllBookings().ToList();

        Assert.Equal(2, result.Count);
        repoMock.Verify(r => r.GetAll(), Times.Once);
    }
}
