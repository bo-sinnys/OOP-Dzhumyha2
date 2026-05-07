# IndependentWork24 — Composite + Decorator + Proxy

Самостійна робота №24 з дисципліни «Об'єктно-орієнтоване програмування».  
Тема: **Інтеграція структурних патернів + тести**.

## Запуск

```bash
# Запустити демонстрацію
cd IndependentWork24
dotnet run

# Запустити тести
cd IndependentWork24.Tests
dotnet test
```

Вимоги: .NET 8 SDK.

## Інтегровані патерни

**Composite** — ієрархія завдань. `TaskItem` (Leaf) — окреме завдання. `ProjectTask` (Composite) — проєкт з підзавданнями. Обидва реалізують `IComponent`, тому клієнтський код не розрізняє їх.

**Decorator** — динамічне розширення поведінки. `PriorityDecorator` додає мітку `[HIGH PRIORITY]`. `DueDateDecorator` додає дедлайн і попереджає про прострочення. Декоратори можна комбінувати і застосовувати до будь-якого `IComponent`.

**Proxy** (`CachingProxyDecorator`) — кешує результат `GetTitle()`, лічить кількість викликів `Display()`. Реалізований як декоратор, щоб зберегти сумісність з `IComponent`.

## Тести (14 штук)

| Група | Що перевіряється |
|---|---|
| CompositeTests | Title, IsCompleted, Add/Remove, рекурсія, порожній проєкт (граничний) |
| DecoratorTests | Префікс PriorityDecorator, IsOverdue, комбіновані декоратори, декоратор на порожньому Composite (граничний) |
| ProxyTests | Cache miss/hit, InvalidateCache, лічильник Display, Proxy поверх Composite |

## Компроміси

- **GetTitle() з кешем** практично не має накладних витрат після першого виклику — кеш повертає значення без звернення до обгорнутого об'єкта.
- **Display() з двома декораторами** додає незначні накладні витрати через ланцюг викликів, але це компенсується гнучкістю: поведінку можна змінювати без зміни вихідних класів.
- **Комбінування декораторів** дає велику гнучкість, але при великій кількості шарів ускладнює налагодження — в стеку видно всі обгортки.

## Структура

```
IndependentWork24/
├── IndependentWork24/
│   ├── Composite/
│   │   ├── IComponent.cs
│   │   ├── TaskItem.cs
│   │   └── ProjectTask.cs
│   ├── Decorators/
│   │   ├── TaskDecorator.cs
│   │   ├── PriorityDecorator.cs
│   │   ├── DueDateDecorator.cs
│   │   └── CachingProxyDecorator.cs
│   └── Program.cs
├── IndependentWork24.Tests/
│   └── PatternTests.cs
└── IndependentWork24.sln
```
