namespace IndependentWork23.Adapter;

/// <summary>
/// Adapter pattern — Adaptee.
/// Стара платіжна система з несумісним інтерфейсом.
/// Не можна змінювати (legacy-код).
/// </summary>
public class OldPaymentSystem
{
    public void Process(decimal amount, string account)
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [OldPaymentSystem] Processing: account={account}, amount={amount:C}");
        Console.ResetColor();
    }
}
