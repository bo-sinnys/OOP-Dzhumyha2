namespace IndependentWork23.Proxy;

/// <summary>
/// Proxy pattern — Proxy.
/// Логує всі звернення до IBankAccount та кешує баланс.
/// Також обмежує кількість знімань (ліміт доступу).
/// </summary>
public class LoggingBankAccountProxy : IBankAccount
{
    private readonly RealBankAccount _realAccount;
    private readonly int _withdrawLimit;
    private int _withdrawCount = 0;

    // Кеш балансу — інвалідується після кожного Withdraw
    private decimal? _cachedBalance = null;

    public LoggingBankAccountProxy(RealBankAccount realAccount, int withdrawLimit = 3)
    {
        _realAccount = realAccount;
        _withdrawLimit = withdrawLimit;
    }

    public decimal GetBalance()
    {
        if (_cachedBalance.HasValue)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"  [Proxy] GetBalance() → returned from CACHE: {_cachedBalance:C}");
            Console.ResetColor();
            return _cachedBalance.Value;
        }

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("  [Proxy] GetBalance() → cache MISS, calling RealBankAccount...");
        Console.ResetColor();

        _cachedBalance = _realAccount.GetBalance();
        return _cachedBalance.Value;
    }

    public bool Withdraw(decimal amount)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  [Proxy] Withdraw {amount:C} requested. Attempts: {_withdrawCount}/{_withdrawLimit}");
        Console.ResetColor();

        if (_withdrawCount >= _withdrawLimit)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  [Proxy] BLOCKED — withdraw limit of {_withdrawLimit} reached!");
            Console.ResetColor();
            return false;
        }

        bool success = _realAccount.Withdraw(amount);
        _withdrawCount++;

        // Інвалідуємо кеш після зміни балансу
        _cachedBalance = null;

        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine($"  [Proxy] Withdraw {(success ? "succeeded" : "failed")}. Cache invalidated.");
        Console.ResetColor();

        return success;
    }
}
