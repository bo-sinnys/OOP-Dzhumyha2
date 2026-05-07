namespace IndependentWork23.Adapter;

/// <summary>
/// Adapter pattern — Target.
/// Інтерфейс обробника транзакцій, який очікує клієнт.
/// </summary>
public interface ITransactionProcessor
{
    void ProcessTransaction(decimal amount, string accountId, string description);
}
