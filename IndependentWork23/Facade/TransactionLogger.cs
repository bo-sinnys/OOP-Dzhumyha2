namespace IndependentWork23.Facade;

/// <summary>
/// Facade pattern — Subsystem class.
/// Відповідає за логування фінансових операцій.
/// </summary>
public class TransactionLogger
{
    private readonly List<string> _logs = new();

    public void Log(string accountId, string operation, decimal amount, bool success)
    {
        string status = success ? "OK" : "FAILED";
        string entry = $"[{DateTime.Now:HH:mm:ss}] {operation} | account={accountId} | amount={amount:C} | status={status}";
        _logs.Add(entry);

        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"  [TransactionLogger] {entry}");
        Console.ResetColor();
    }

    public void PrintAll()
    {
        Console.WriteLine("  Transaction log:");
        foreach (string log in _logs)
            Console.WriteLine($"    {log}");
    }
}
