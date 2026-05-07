# IndependentWork21 — Factory + Singleton + Strategy + Observer

Самостійна робота №21 з дисципліни «Об'єктно-орієнтоване програмування».  
Тема: **Інтеграційні тести патернів (підготовка до екзамену)**.

Вимоги: .NET 8 SDK.

## Опис

Проєкт реалізує систему керування завданнями з використанням чотирьох патернів, інтегрованих в єдиний сценарій.

### Factory

`TaskFactory` відповідає за створення завдань `TaskInfo`. При кожному створенні автоматично реєструє завдання в `TaskRegistry` (Singleton) і сповіщає всіх підписників (Observer). Також надає методи `CompleteTask()` і `ChangePriority()`, які оновлюють стан і сповіщають підписників.

### Singleton

`TaskRegistry` — глобальний реєстр завдань. Гарантує єдиний інстанс через thread-safe double-checked locking. Зберігає список всіх завдань і поточну стратегію сортування. Надає метод `ResetForTesting()` для скидання стану між тестами.

### Strategy

Три стратегії сортування реалізують `ITaskSortStrategy`: `SortByPriority` (High -> Low), `SortByDueDate` (найближчий дедлайн першим), `SortByCompletion` (невиконані спочатку, потім за пріоритетом). Стратегію можна змінювати в runtime через `TaskRegistry.SetStrategy()`.

### Observer

`TaskManager` реалізує `ITaskSubject` — керує підписниками і сповіщає їх. `LoggerObserver` записує всі події. `CompletionObserver` відстежує лише завершені завдання. Підписка і відписка доступні в будь-який момент.

## Тести

15 інтеграційних тестів (xUnit). Кожен тест скидає Singleton через `ResetForTesting()`.

| # | Тест | Тип |
|---|------|-----|
| 1 | Factory реєструє завдання в Singleton | Позитивний |
| 2 | Factory сповіщає Observer при створенні | Позитивний |
| 3 | CompleteTask оновлює стан і сповіщає Observer | Позитивний |
| 4 | Singleton завжди повертає той самий інстанс | Позитивний |
| 5 | Singleton зберігає стан між кількома викликами Factory | Позитивний |
| 6 | SortByPriority — High іде першим | Позитивний |
| 7 | SortByDueDate — найближчий дедлайн першим | Позитивний |
| 8 | Зміна Strategy в runtime змінює результат | Позитивний |
| 9 | ChangePriority сповіщає Logger з деталями | Позитивний |
| 10 | Unsubscribe — відписаний observer не отримує події | Позитивний |
| 11 | Порожня назва — ArgumentException | Негативний |
| 12 | Назва з пробілів — ArgumentException | Негативний |
| 13 | Порожній реєстр — GetSorted повертає пустий список | Граничний |
| 14 | CompletionObserver ігнорує не-completion події | Граничний |
| 15 | SortByCompletion — завершені йдуть в кінець | Граничний |

## Структура

```
IndependentWork21/
├── IndependentWork21/
│   ├── Factory/
│   │   └── TaskFactory.cs
│   ├── Observer/
│   │   ├── ITaskObserver.cs
│   │   └── ConcreteObservers.cs
│   ├── Singleton/
│   │   └── TaskRegistry.cs
│   ├── Strategy/
│   │   └── ITaskSortStrategy.cs
│   ├── TaskManager.cs
│   └── Program.cs
└── IndependentWork21.Tests/
    └── IntegrationTests.cs
```
