namespace lab31v1.Interfaces;

public interface INotificationService
{
    void SendConfirmation(string guestName, string email, int bookingId);
    void SendCancellation(string guestName, string email, int bookingId);
    void SendReminder(string guestName, string email, DateTime checkIn);
}
