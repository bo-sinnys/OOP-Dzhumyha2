namespace IndependentWork23.Facade;

/// <summary>
/// Facade pattern — Subsystem class.
/// Відповідає за операції з рахунком: поповнення та зняття коштів.
/// </summary>
public class AccountService
{
    private decimal _balance;

    public AccountService(decimal initialBalance)
    {
        _balance = initialBalance;
    }

    public void Deposit(string accountId, decimal amount)
    {
        _balance += amount;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [AccountService] Deposit +{amount:C} on {accountId}. Balance: {_balance:C}");
        Console.ResetColor();
    }

    public bool Withdraw(string accountId, decimal amount)
    {
        if (_balance < amount)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  [AccountService] Withdraw FAILED — insufficient funds on {accountId}");
            Console.ResetColor();
            return false;
        }

        _balance -= amount;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [AccountService] Withdraw -{amount:C} on {accountId}. Balance: {_balance:C}");
        Console.ResetColor();
        return true;
    }

    public decimal GetBalance() => _balance;
}
