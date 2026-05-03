# Лабораторна робота №31v5
**Тема:** Тестування з Moq (мокінг залежностей)

---

## Реалізація

### Модель `Booking`

Клас-модель, що описує бронювання готельного номера:

| Поле | Тип | Опис |
|---|---|---|
| `Id` | `int` | Унікальний ідентифікатор |
| `GuestName` | `string` | Ім'я гостя |
| `RoomNumber` | `string` | Номер кімнати |
| `CheckIn` | `DateTime` | Дата заїзду |
| `CheckOut` | `DateTime` | Дата виїзду |
| `IsConfirmed` | `bool` | Статус підтвердження |

---

### Інтерфейс `IBookingRepository`

Визначає контракт для роботи з даними бронювань:

```csharp
public interface IBookingRepository
{
    Booking? GetById(int id);
    IEnumerable<Booking> GetAll();
    void Add(Booking booking);
    void Update(Booking booking);
    void Delete(int id);
    bool Exists(int id);
}
```

---

### Інтерфейс `INotificationService`

Визначає контракт для надсилання сповіщень гостям:

```csharp
public interface INotificationService
{
    void SendConfirmation(string guestName, string email, int bookingId);
    void SendCancellation(string guestName, string email, int bookingId);
    void SendReminder(string guestName, string email, DateTime checkIn);
}
```

---

### Клас `BookingService`

Основний сервіс із двома залежностями, що передаються через конструктор (Dependency Injection):

```csharp
public class BookingService
{
    private readonly IBookingRepository _repository;
    private readonly INotificationService _notificationService;

    public BookingService(IBookingRepository repository, INotificationService notificationService)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    }
    // ...
}
```

**Методи сервісу:**

| Метод | Опис |
|---|---|
| `CreateBooking(booking, email)` | Створює бронювання, підтверджує та надсилає email |
| `CancelBooking(id, email)` | Скасовує бронювання та надсилає сповіщення |
| `GetBooking(id)` | Повертає бронювання за ідентифікатором |
| `GetAllBookings()` | Повертає всі бронювання |
| `SendReminders(email)` | Надсилає нагадування для підтверджених бронювань |
| `UpdateBooking(booking)` | Оновлює існуюче бронювання |

---

## Тести з Moq

### Налаштування тестів

Для кожного тесту створюються mock-об'єкти обох залежностей:

```csharp
var repoMock  = new Mock<IBookingRepository>();
var notifMock = new Mock<INotificationService>();
var sut       = new BookingService(repoMock.Object, notifMock.Object);
```

---

### Перелік тестів

| № | Назва тесту | Setup | Verify |
|---|---|---|---|
| 1 | `CreateBooking_ValidBooking_CallsRepositoryAdd` | — | `Add()` викликано 1 раз |
| 2 | `CreateBooking_ValidBooking_SendsConfirmationNotification` | — | `SendConfirmation()` викликано 1 раз |
| 3 | `CreateBooking_ValidBooking_SetsIsConfirmedTrue` | — | Assert.True |
| 4 | `CreateBooking_InvalidDates_ThrowsArgumentException` | — | `Add()` не викликано |
| 5 | `CancelBooking_ExistingBooking_CallsRepositoryDelete` | `Exists` → true, `GetById` → booking | `Delete()` викликано 1 раз |
| 6 | `CancelBooking_ExistingBooking_SendsCancellationNotification` | `Exists` → true, `GetById` → booking | `SendCancellation()` викликано 1 раз |
| 7 | `CancelBooking_NonExistentId_ThrowsInvalidOperationException` | `Exists` → false | `SendCancellation()` не викликано |
| 8 | `GetBooking_ExistingId_ReturnsBookingFromRepository` | `GetById` → booking | `GetById()` викликано 1 раз |
| 9 | `SendReminders_OnlyConfirmedBookings_SendsReminderForEachConfirmed` | `GetAll` → список | `SendReminder()` лише для підтверджених |
| 10 | `UpdateBooking_ExistingBooking_CallsRepositoryUpdate` | `Exists` → true | `Update()` викликано 1 раз |
| 11 | `UpdateBooking_NonExistentId_ThrowsInvalidOperationException` | `Exists` → false | `Update()` не викликано |
| 12 | `GetAllBookings_ReturnsAllFromRepository` | `GetAll` → список | `GetAll()` викликано 1 раз |

---

### Приклад використання `Setup` та `Verify`

```csharp
[Fact]
public void CancelBooking_ExistingBooking_CallsRepositoryDelete()
{
    var repoMock  = new Mock<IBookingRepository>();
    var notifMock = new Mock<INotificationService>();
    var sut       = new BookingService(repoMock.Object, notifMock.Object);
    var booking   = new Booking { Id = 1, GuestName = "Іван", ... };

    // Setup — налаштовуємо поведінку mock-об'єкта
    repoMock.Setup(r => r.Exists(booking.Id)).Returns(true);
    repoMock.Setup(r => r.GetById(booking.Id)).Returns(booking);

    sut.CancelBooking(booking.Id, "guest@example.com");

    // Verify — перевіряємо, що метод було викликано рівно 1 раз
    repoMock.Verify(r => r.Delete(booking.Id), Times.Once);
}
```


**Результат:**

```
Build succeeded
Test summary: total: 12, failed: 0, passed: 12, skipped: 0
```

---

## Використані технології

| Технологія | Версія | Призначення |
|---|---|---|
| .NET | 8.0 | Платформа |
| xUnit | 2.4.2 | Фреймворк для тестування |
| Moq | 4.20.72 | Бібліотека для мокінгу |
| Microsoft.NET.Test.Sdk | 17.6.0 | Запуск тестів |

---

## Висновок

У ході виконання лабораторної роботи було:

- створено два проєкти: основний (`lab31v5`) та тестовий (`lab31v5.Tests`);
- реалізовано клас `BookingService` з двома інтерфейсами залежностей (`IBookingRepository`, `INotificationService`) через Dependency Injection;
- написано **12 тестів** з використанням бібліотеки **Moq**;
- застосовано `Setup` для налаштування поведінки mock-об'єктів та `Verify` для перевірки коректності викликів методів;
- всі тести успішно пройдено.

Мокінг залежностей дозволяє тестувати бізнес-логіку ізольовано, без реальних баз даних чи зовнішніх сервісів, що значно спрощує написання надійних unit-тестів.
