namespace BankingSystem.Application.Services;

/// <summary>
/// Виділений клас для валідації бізнес-правил рахунків.
/// Issue #4 — refactor: extract validation logic from AccountService into separate class
/// </summary>
public static class AccountValidator
{
    /// <summary>Перевіряє що сума операції є позитивною.</summary>
    public static void ValidateAmount(decimal amount, string paramName = "amount")
    {
        if (amount <= 0)
            throw new ArgumentException(
                $"Сума повинна бути більше нуля. Отримано: {amount}.", paramName);
    }

    /// <summary>Перевіряє що email має коректний формат.</summary>
    public static void ValidateEmail(string email, string paramName = "email")
    {
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException(
                $"Некоректна електронна адреса: '{email}'.", paramName);
    }

    /// <summary>Перевіряє що рядок не порожній.</summary>
    public static void ValidateNotEmpty(string value, string paramName)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                $"Параметр '{paramName}' не може бути порожнім.", paramName);
    }
}
