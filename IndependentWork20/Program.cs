public interface IDataProcessorStrategy
{
    void Process(string data);
}

public class ProcessOnlineOrderStrategy : IDataProcessorStrategy
{
    public void Process(string data)
    {
        Console.WriteLine($"  [Online] Обробка онлайн-замовлення: {data}");
        Console.WriteLine($"  [Online] Перевірка платіжних даних...");
    }
}

public class ProcessCashOrderStrategy : IDataProcessorStrategy
{
    public void Process(string data)
    {
        Console.WriteLine($"  [Cash] Обробка готівкового замовлення: {data}");
        Console.WriteLine($"  [Cash] Формування касового чеку...");
    }
}

public class ProcessCreditCardOrderStrategy : IDataProcessorStrategy
{
    public void Process(string data)
    {
        Console.WriteLine($"  [CreditCard] Обробка замовлення кредитною карткою: {data}");
        Console.WriteLine($"  [CreditCard] Авторизація транзакції через банк...");
    }
}

public class DataContext
{
    private IDataProcessorStrategy _strategy;

    public DataContext(IDataProcessorStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SetStrategy(IDataProcessorStrategy strategy)
    {
        _strategy = strategy;
        Console.WriteLine($"  >> Стратегію змінено на: {strategy.GetType().Name}");
    }

    public void ExecuteProcessing(string data)
    {
        _strategy.Process(data);
    }
}

public class DataPublisher
{
    public event Action<string>? DataProcessed;

    public void PublishDataProcessed(string data)
    {
        Console.WriteLine($"  [Publisher] Подія DataProcessed: \"{data}\"");
        DataProcessed?.Invoke(data);
    }
}

public class OrderConfirmationEmailObserver
{
    public void Subscribe(DataPublisher publisher)
    {
        publisher.DataProcessed += OnDataProcessed;
    }

    private void OnDataProcessed(string data)
    {
        Console.WriteLine($"    [EmailObserver] Email-підтвердження надіслано: \"{data}\"");
    }
}

public class InventoryUpdateObserver
{
    public void Subscribe(DataPublisher publisher)
    {
        publisher.DataProcessed += OnDataProcessed;
    }

    private void OnDataProcessed(string data)
    {
        Console.WriteLine($"    [InventoryObserver] Склад оновлено для: \"{data}\"");
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("=== IndependentWork20: Strategy + Observer ===\n");

        var context = new DataContext(new ProcessOnlineOrderStrategy());
        var publisher = new DataPublisher();

        var emailObserver = new OrderConfirmationEmailObserver();
        var inventoryObserver = new InventoryUpdateObserver();

        emailObserver.Subscribe(publisher);
        inventoryObserver.Subscribe(publisher);

        Console.WriteLine("Спостерігачі підписані: EmailObserver, InventoryObserver\n");

        Console.WriteLine("--- Замовлення #1 (Online) ---");
        context.ExecuteProcessing("Order #1001 - Laptop");
        publisher.PublishDataProcessed("Order #1001 - Laptop");
        Console.WriteLine();

        Console.WriteLine("--- Замовлення #2 (Cash) ---");
        context.SetStrategy(new ProcessCashOrderStrategy());
        context.ExecuteProcessing("Order #1002 - Mouse");
        publisher.PublishDataProcessed("Order #1002 - Mouse");
        Console.WriteLine();

        Console.WriteLine("--- Замовлення #3 (CreditCard) ---");
        context.SetStrategy(new ProcessCreditCardOrderStrategy());
        context.ExecuteProcessing("Order #1003 - Monitor");
        publisher.PublishDataProcessed("Order #1003 - Monitor");

        Console.WriteLine("\n=== Роботу завершено ===");
    }
}
