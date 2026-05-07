namespace IndependentWork23.Proxy;

/// <summary>
/// Proxy pattern — RealSubject.
/// Реальний банківський рахунок — виконує справжню роботу.
/// </summary>
public class RealBankAccount : IBankAccount
{
    private decimal _balance;
    public string AccountId { get; }

    public RealBankAccount(string accountId, decimal initialBalance)
    {
        AccountId = accountId;
        _balance = initialBalance;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [RealBankAccount] Account '{accountId}' created. Balance: {_balance:C}");
        Console.ResetColor();
    }

    public decimal GetBalance()
    {
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [RealBankAccount] GetBalance() → {_balance:C}");
        Console.ResetColor();
        return _balance;
    }

    public bool Withdraw(decimal amount)
    {
        if (_balance < amount)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine($"  [RealBankAccount] Withdraw {amount:C} FAILED — insufficient funds");
            Console.ResetColor();
            return false;
        }

        _balance -= amount;
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [RealBankAccount] Withdraw {amount:C} OK. New balance: {_balance:C}");
        Console.ResetColor();
        return true;
    }
}
