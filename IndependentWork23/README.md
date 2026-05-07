# IndependentWork23 — Adapter + Facade + Proxy

Самостійна робота №23 з дисципліни «Об'єктно-орієнтоване програмування».  
Тема: **Adapter + Facade + Proxy: кеш і ліміти**.  
Варіант 5 — Обробка фінансових транзакцій.

## Запуск

```bash
cd IndependentWork23
dotnet run
```

Вимоги: .NET 8 SDK.

## Опис

Проєкт демонструє роботу трьох структурних патернів проєктування на прикладі системи обробки фінансових транзакцій.

### Adapter

Є стара платіжна система `OldPaymentSystem` з методом `Process(decimal amount, string account)`. Новий код очікує інтерфейс `ITransactionProcessor` з методом `ProcessTransaction(decimal amount, string accountId, string description)`. Щоб не чіпати legacy-код і не переписувати клієнта, між ними стоїть `OldPaymentAdapter` — він реалізує новий інтерфейс і всередині викликає стару систему. Параметр `description` відкидається, бо стара система його не підтримує.

### Facade

Підсистема складається з двох класів: `AccountService` (відповідає за баланс рахунку — поповнення та зняття) і `TransactionLogger` (записує кожну операцію з часом і статусом). Клієнту не потрібно знати про обидва — він працює через `FinancialFacade`, який сам координує ці класи. Один виклик `PerformWithdrawal()` і за лаштунками автоматично виконується операція та записується лог.

### Proxy

`RealBankAccount` — реальний рахунок, який виконує справжню роботу. `LoggingBankAccountProxy` обгортає його і додає три речі:

- **Кешування** — `GetBalance()` звертається до реального об'єкта лише один раз, далі повертає збережений результат.
- **Інвалідація кешу** — після кожного `Withdraw()` кеш скидається, щоб наступний `GetBalance()` отримав актуальні дані.
- **Ліміт знімань** — після досягнення встановленої кількості операцій `Withdraw()` подальші виклики блокуються самим проксі без звернення до реального об'єкта.

Клієнт при цьому працює через інтерфейс `IBankAccount` і не знає, чи має справу з реальним об'єктом чи проксі.

## Структура проєкту

```
IndependentWork23/
├── Adapter/
│   ├── ITransactionProcessor.cs   ← Target interface
│   ├── OldPaymentSystem.cs        ← Adaptee (legacy)
│   └── OldPaymentAdapter.cs       ← Adapter
├── Facade/
│   ├── AccountService.cs          ← Subsystem: рахунок
│   ├── TransactionLogger.cs       ← Subsystem: логування
│   └── FinancialFacade.cs         ← Facade
├── Proxy/
│   ├── IBankAccount.cs            ← Subject interface
│   ├── RealBankAccount.cs         ← RealSubject
│   └── LoggingBankAccountProxy.cs ← Proxy (кеш + ліміт)
└── Program.cs                     ← демонстрація всіх патернів
```
