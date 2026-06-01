# BankingSystem

Консольна банківська система — підсумковий міні-проєкт з курсу ООП (варіант 5).

## Поточний стан

| Ітерація | Лабораторна | Що реалізовано |
|----------|-------------|----------------|
| 1 | Lab 34 | Доменна модель, перший вертикальний зріз (переказ між рахунками), юніт-тести, CI |
| 2 | Lab 38 | Docker-контейнеризація, multi-stage build, Docker Compose, graceful shutdown |
| 3 | Lab 39 | GitHub Flow, branch protection, PR/Issue шаблони, code review workflow |

## Запуск

### Локально
```bash
git clone https://github.com/bo-sinnys/OOP-MiniProject-BankingSystem.git
cd BankingSystem
dotnet run --project src/BankingSystem.Console
```

### У Docker
```bash
docker build -t bankingsystem:v1 .
docker run -it --rm -v $(pwd)/data:/app/data bankingsystem:v1
```

### Через Docker Compose
```bash
docker compose up --build
docker compose down
```

## Запуск тестів
```bash
dotnet test
```

## Структура проєкту

```
BankingSystem/
├── src/
│   ├── BankingSystem.Domain/           # Сутності, інтерфейси, винятки
│   │   ├── Entities/                   # Account, Customer, Transaction
│   │   ├── Interfaces/                 # IRepository<T>, IInterestCalculator
│   │   └── Exceptions/                 # DomainException і нащадки
│   ├── BankingSystem.Application/      # Сервіси та бізнес-логіка
│   │   └── Services/                   # AccountService, CsvExportService, AccountValidator
│   ├── BankingSystem.Infrastructure/   # Репозиторії (in-memory)
│   │   └── Repositories/
│   └── BankingSystem.Console/          # Консольний UI, меню
│       └── Program.cs
├── tests/
│   └── BankingSystem.Tests/            # xUnit юніт і інтеграційні тести
├── docs/
│   ├── vision.md
│   ├── backlog.md
│   ├── class-diagram.md
│   ├── sequence-diagram.md
│   └── iteration-1.md
├── .github/
│   ├── workflows/dotnet.yml            # GitHub Actions CI
│   ├── PULL_REQUEST_TEMPLATE.md
│   └── ISSUE_TEMPLATE/
├── Dockerfile                          # Multi-stage build (sdk → runtime)
├── Dockerfile.alpine                   # Alpine-варіант (~100 MB)
├── docker-compose.yml
├── DOCKER.md                           # Інструкція з Docker
├── CONTRIBUTING.md                     # Workflow для командної роботи
└── BankingSystem.sln
```

## Архітектура

```
Console ──► Application (AccountService)
                  │
                  ├──► Domain (Account, Customer, Transaction)
                  │         бізнес-правила та інваріанти
                  │
                  └──► Infrastructure (IRepository<T>)
                              in-memory (Lab 34) → JSON (Lab 35)
```

**Ключові принципи:**
- **DIP**: `AccountService` залежить від `IAccountRepository`, не від конкретного класу
- **OCP**: нові типи рахунків — просто новий підклас `Account`, без зміни сервісу
- **SRP**: кожен сервіс має одну зону відповідальності

## Реалізовані сценарії

- Реєстрація клієнта
- Відкриття рахунку (`CheckingAccount`, `SavingsAccount`)
- Поповнення / зняття
- Переказ між рахунками (головний vertical slice)
- Перегляд виписки
- Експорт транзакцій у CSV

## Майбутні ітерації

| Lab | Що додається |
|-----|--------------|
| Lab 35 | JSON-персистентність, нарахування відсотків (Strategy), кредити |
| Lab 36 | Coverage, інтеграційні тести, fault handling, CI quality gate |
| Lab 37 | Release-документація, DEMO, фінальний звіт |
