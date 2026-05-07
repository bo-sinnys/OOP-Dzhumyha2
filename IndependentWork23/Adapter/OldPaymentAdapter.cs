namespace IndependentWork23.Adapter;

/// <summary>
/// Adapter pattern — Adapter.
/// Адаптує OldPaymentSystem до ITransactionProcessor.
/// </summary>
public class OldPaymentAdapter : ITransactionProcessor
{
    private readonly OldPaymentSystem _oldSystem;

    public OldPaymentAdapter(OldPaymentSystem oldSystem)
    {
        _oldSystem = oldSystem;
    }

    public void ProcessTransaction(decimal amount, string accountId, string description)
    {
        // Адаптуємо новий виклик до старого інтерфейсу — ігноруємо description,
        // бо стара система його не підтримує.
        Console.WriteLine($"  [Adapter] Adapting call → description \"{description}\" dropped (legacy system)");
        _oldSystem.Process(amount, accountId);
    }
}
