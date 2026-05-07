using IndependentWork23.Adapter;
using IndependentWork23.Facade;
using IndependentWork23.Proxy;

// ─────────────────────────────────────────────────────────────────────────────
// Самостійна робота №23 — Adapter + Facade + Proxy
// Варіант 5: Обробка фінансових транзакцій
// ─────────────────────────────────────────────────────────────────────────────

PrintHeader("ОБРОБКА ФІНАНСОВИХ ТРАНЗАКЦІЙ");

// ════════════════════════════════════════════════════════════════════════════
// 1. ADAPTER
// ════════════════════════════════════════════════════════════════════════════
PrintSection("ADAPTER — інтеграція зі старою платіжною системою");

// Клієнт очікує ITransactionProcessor, але є лише OldPaymentSystem
OldPaymentSystem legacySystem = new();
ITransactionProcessor processor = new OldPaymentAdapter(legacySystem);

Console.WriteLine("Виклик через новий інтерфейс ITransactionProcessor:");
processor.ProcessTransaction(500.00m, "UA-001", "Оплата рахунку #1042");
processor.ProcessTransaction(1200.50m, "UA-002", "Переказ партнеру");

// ════════════════════════════════════════════════════════════════════════════
// 2. FACADE
// ════════════════════════════════════════════════════════════════════════════
PrintSection("FACADE — спрощений інтерфейс до фінансової підсистеми");

FinancialFacade facade = new(initialBalance: 5000m);

Console.WriteLine($"Початковий баланс: {facade.GetBalance():C}\n");

Console.WriteLine("Поповнення рахунку UA-010 на 1500.00:");
facade.PerformDeposit("UA-010", 1500.00m);

Console.WriteLine("\nЗняття 800.00:");
facade.PerformWithdrawal("UA-010", 800.00m);

Console.WriteLine("\nСпроба зняти більше, ніж є на рахунку (10 000.00):");
facade.PerformWithdrawal("UA-010", 10_000.00m);

Console.WriteLine($"\nПоточний баланс: {facade.GetBalance():C}");

Console.WriteLine("\nЛог транзакцій:");
facade.PrintTransactionLog();

// ════════════════════════════════════════════════════════════════════════════
// 3. PROXY
// ════════════════════════════════════════════════════════════════════════════
PrintSection("PROXY — логування, кешування, ліміт знімань");

RealBankAccount realAccount = new("UA-999", 3000m);
// Ліміт — 3 операції зняття
LoggingBankAccountProxy proxy = new(realAccount, withdrawLimit: 3);

Console.WriteLine("\n--- GetBalance() двічі (друге — з кешу) ---");
proxy.GetBalance();
proxy.GetBalance();   // має повернути з кешу

Console.WriteLine("\n--- Withdraw операції ---");
proxy.Withdraw(200m);   // #1 — кеш інвалідується
proxy.Withdraw(300m);   // #2
proxy.Withdraw(100m);   // #3 — останній дозволений

Console.WriteLine("\n--- Спроба 4-го зняття (має бути заблокована проксі) ---");
proxy.Withdraw(50m);

Console.WriteLine("\n--- GetBalance() після всіх знімань ---");
proxy.GetBalance();     // кеш пустий після останнього Withdraw → звернення до реального об'єкта

PrintFooter();


// ── Допоміжні методи виводу ──────────────────────────────────────────────────

static void PrintHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(new string('═', 62));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('═', 62));
    Console.ResetColor();
    Console.WriteLine();
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(new string('─', 62));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('─', 62));
    Console.ResetColor();
    Console.WriteLine();
}

static void PrintFooter()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(new string('═', 62));
    Console.WriteLine("  Програму завершено успішно.");
    Console.WriteLine(new string('═', 62));
    Console.ResetColor();
}
