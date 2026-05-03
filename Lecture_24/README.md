# Code Smells та рефакторинг: практичний аналіз
---

## Вступ

Рефакторинг — це процес покращення внутрішньої структури коду без зміни його зовнішньої поведінки. Мартін Фаулер, автор однойменної книги, описує його як серію невеликих, контрольованих перетворень, що підвищують читабельність, знижують складність та спрощують подальшу підтримку.

У цьому есе я проаналізую код з лабораторної роботи №31 (`BookingService`) і виявлю реальні code smells, що виникли в процесі написання, а також запропоную конкретні техніки їхнього усунення.

---

## Code Smell №1 — Long Parameter List у методі `CreateBooking`

### Проблема

У первинній реалізації метод `CreateBooking` приймав об'єкт `Booking` та окремо рядок `email`. Однак у реальній системі список параметрів міг би розростися: email, телефон, мова сповіщення тощо. Це класичний приклад **Long Parameter List** — code smell, де метод отримує забагато аргументів, що ускладнює його виклик і розуміння.

### Код до рефакторингу

```csharp
// Погано: параметри не пов'язані між собою явно
public void CreateBooking(Booking booking, string email)
{
    if (booking == null) throw new ArgumentNullException(nameof(booking));
    if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentException("Email не може бути порожнім.", nameof(email));

    booking.IsConfirmed = true;
    _repository.Add(booking);
    _notificationService.SendConfirmation(booking.GuestName, email, booking.Id);
}
```

### Техніка рефакторингу: **Introduce Parameter Object**

Вирішення — згрупувати пов'язані параметри в окремий об'єкт `BookingRequest`, який інкапсулює всю необхідну інформацію для створення бронювання.

### Код після рефакторингу

```csharp
// Новий об'єкт-параметр
public class BookingRequest
{
    public Booking Booking { get; init; } = null!;
    public string GuestEmail { get; init; } = string.Empty;
}

// Метод стає чистішим і розширюваним
public void CreateBooking(BookingRequest request)
{
    if (request.Booking.CheckOut <= request.Booking.CheckIn)
        throw new ArgumentException("Дата виїзду має бути пізніше дати заїзду.");

    request.Booking.IsConfirmed = true;
    _repository.Add(request.Booking);
    _notificationService.SendConfirmation(
        request.Booking.GuestName, request.GuestEmail, request.Booking.Id);
}
```

**Переваги:** якщо у майбутньому знадобиться додати поле `PhoneNumber` або `Language` — достатньо розширити клас `BookingRequest`, не змінюючи сигнатуру методу.

---

## Code Smell №2 — Duplicate Code у методах `CancelBooking` та `UpdateBooking`

### Проблема

Обидва методи містять ідентичну логіку перевірки існування бронювання перед виконанням дії. Це **Duplicate Code** — один із найпоширеніших code smells, який ускладнює підтримку: при зміні логіки перевірки потрібно знаходити всі копії.

### Код до рефакторингу

```csharp
public void CancelBooking(int bookingId, string email)
{
    // Ця перевірка...
    if (!_repository.Exists(bookingId))
        throw new InvalidOperationException($"Бронювання з Id={bookingId} не знайдено.");

    var booking = _repository.GetById(bookingId)!;
    _repository.Delete(bookingId);
    _notificationService.SendCancellation(booking.GuestName, email, bookingId);
}

public void UpdateBooking(Booking booking)
{
    // ...повторюється тут
    if (!_repository.Exists(booking.Id))
        throw new InvalidOperationException($"Бронювання з Id={booking.Id} не знайдено.");

    _repository.Update(booking);
}
```

### Техніка рефакторингу: **Extract Method**

Виділяємо спільну логіку перевірки в окремий приватний метод із зрозумілою назвою.

### Код після рефакторингу

```csharp
// Виділений метод з однією відповідальністю
private void EnsureBookingExists(int bookingId)
{
    if (!_repository.Exists(bookingId))
        throw new InvalidOperationException($"Бронювання з Id={bookingId} не знайдено.");
}

public void CancelBooking(int bookingId, string email)
{
    EnsureBookingExists(bookingId);
    var booking = _repository.GetById(bookingId)!;
    _repository.Delete(bookingId);
    _notificationService.SendCancellation(booking.GuestName, email, bookingId);
}

public void UpdateBooking(Booking booking)
{
    EnsureBookingExists(booking.Id);
    _repository.Update(booking);
}
```

**Переваги:** логіка перевірки існування знаходиться в одному місці. Якщо потрібно додати логування або змінити текст помилки — правимо лише один метод.

---

## Code Smell №3 — Magic Numbers у тестах

### Проблема

У тестовому файлі `BookingServiceTests.cs` зустрічаються числові та рядкові літерали (`1`, `2`, `42`, `"guest@example.com"`), розкидані по різних тестах без пояснення їхнього значення. Це **Magic Numbers** — code smell, що знижує читабельність і ускладнює підтримку тестів.

### Код до рефакторингу

```csharp
[Fact]
public void UpdateBooking_NonExistentId_ThrowsInvalidOperationException()
{
    var booking = MakeBooking(42);          // чому 42?
    repoMock.Setup(r => r.Exists(42)).Returns(false);

    Assert.Throws<InvalidOperationException>(() => sut.UpdateBooking(booking));
}

[Fact]
public void GetAllBookings_ReturnsAllFromRepository()
{
    var bookings = new[] { MakeBooking(1), MakeBooking(2) };  // що означають 1 і 2?
    repoMock.Setup(r => r.GetAll()).Returns(bookings);

    var result = sut.GetAllBookings().ToList();
    Assert.Equal(2, result.Count);          // чому 2?
}
```

### Техніка рефакторингу: **Extract Constant**

Виносимо всі "магічні" значення у іменовані константи на рівні класу тестів.

### Код після рефакторингу

```csharp
public class BookingServiceTests
{
    // Іменовані константи замість магічних чисел
    private const int ExistingBookingId    = 1;
    private const int AnotherBookingId     = 2;
    private const int NonExistentBookingId = 99;
    private const string GuestEmail        = "guest@example.com";

    [Fact]
    public void UpdateBooking_NonExistentId_ThrowsInvalidOperationException()
    {
        var booking = MakeBooking(NonExistentBookingId);
        repoMock.Setup(r => r.Exists(NonExistentBookingId)).Returns(false);

        Assert.Throws<InvalidOperationException>(() => sut.UpdateBooking(booking));
    }

    [Fact]
    public void GetAllBookings_ReturnsAllFromRepository()
    {
        var bookings = new[] { MakeBooking(ExistingBookingId), MakeBooking(AnotherBookingId) };
        repoMock.Setup(r => r.GetAll()).Returns(bookings);

        var result = sut.GetAllBookings().ToList();
        Assert.Equal(2, result.Count);
    }
}
```

**Переваги:** читач тесту одразу розуміє намір — перевіряється саме неіснуючий ідентифікатор, а не довільне число.

---

## Чому рефакторинг без тестів є ризикованим

Рефакторинг без тестів — це зміна коду наосліп. Розглянемо конкретний приклад з нашого проєкту.

Припустимо, ми рефакторимо метод `CancelBooking`, виділяємо `EnsureBookingExists`, і випадково змінюємо логіку:

```csharp
// Помилка: змінили ! на відсутність заперечення
private void EnsureBookingExists(int bookingId)
{
    if (_repository.Exists(bookingId))  // ← баг: логіку інвертовано!
        throw new InvalidOperationException($"Бронювання з Id={bookingId} не знайдено.");
}
```

**Без тестів:** цей баг непомітний. Код компілюється, програма запускається. Але тепер `CancelBooking` кидає виняток для *існуючих* бронювань і успішно виконується для *неіснуючих* — повністю протилежна поведінка.

**З тестами:** тест `CancelBooking_ExistingBooking_CallsRepositoryDelete` негайно впаде з повідомленням про помилку. Ми дізнаємося про баг за секунди, а не після деплою в продакшн.

```
// Результат dotnet test після помилкового рефакторингу:
FAILED: CancelBooking_ExistingBooking_CallsRepositoryDelete
Expected: Delete() called once
Actual:   System.InvalidOperationException was thrown
```

Саме тому перший крок будь-якого рефакторингу — переконатися, що тести існують і проходять. Тести є страховою сіткою, яка дозволяє вносити зміни впевнено.

---

## Висновок

Аналіз коду лабораторної роботи №31 показав, що навіть у відносно невеликому проєкті виникають типові code smells:

| Code Smell | Виявлено в | Техніка усунення |
|---|---|---|
| Long Parameter List | `CreateBooking` | Introduce Parameter Object |
| Duplicate Code | `CancelBooking`, `UpdateBooking` | Extract Method |
| Magic Numbers | `BookingServiceTests` | Extract Constant |

Рефакторинг — це не одноразова дія, а постійна практика. Дотримання правила скаутів — *"залишай код чистішим, ніж ти його знайшов"* — у поєднанні з наявністю тестів дозволяє підтримувати якість коду на високому рівні протягом усього життєвого циклу проєкту.
