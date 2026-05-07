using IndependentWork21;
using IndependentWork21.Factory;
using IndependentWork21.Observer;
using IndependentWork21.Singleton;
using IndependentWork21.Strategy;

Console.WriteLine("=== IndependentWork21 — Factory + Singleton + Strategy + Observer ===\n");

// Observer
var manager = new TaskManager();
var logger = new LoggerObserver();
var completion = new CompletionObserver();
manager.Subscribe(logger);
manager.Subscribe(completion);

// Factory
var factory = new TaskFactory(manager);

Console.WriteLine("-- Створення завдань --");
var t1 = factory.CreateTask("Написати тести",       Priority.High,   DateTime.Today.AddDays(3));
var t2 = factory.CreateTask("Зробити code review",  Priority.Medium, DateTime.Today.AddDays(7));
var t3 = factory.CreateTask("Оновити документацію", Priority.Low,    DateTime.Today.AddDays(14));
var t4 = factory.CreateTask("Виправити баг",        Priority.High,   DateTime.Today.AddDays(1));

// Singleton — всі завдання зареєстровані
Console.WriteLine($"\nУсього в реєстрі: {TaskRegistry.Instance.All.Count}");

// Strategy
Console.WriteLine("\n-- Сортування за пріоритетом --");
TaskRegistry.Instance.SetStrategy(new SortByPriority());
foreach (var t in TaskRegistry.Instance.GetSorted())
    Console.WriteLine($"  [{t.Priority}] {t.Title}");

Console.WriteLine("\n-- Сортування за дедлайном --");
TaskRegistry.Instance.SetStrategy(new SortByDueDate());
foreach (var t in TaskRegistry.Instance.GetSorted())
    Console.WriteLine($"  [{t.DueDate:dd.MM}] {t.Title}");

// Complete + ChangePriority
Console.WriteLine("\n-- Операції --");
factory.CompleteTask(t3);
factory.ChangePriority(t2, Priority.High);

Console.WriteLine("\n-- Сортування: невиконані спочатку --");
TaskRegistry.Instance.SetStrategy(new SortByCompletion());
foreach (var t in TaskRegistry.Instance.GetSorted())
    Console.WriteLine($"  [{(t.IsCompleted ? "v" : " ")}][{t.Priority}] {t.Title}");

Console.WriteLine($"\nЗавершених завдань: {completion.CompletedTasks.Count}");
Console.WriteLine($"Подій в лозі: {logger.Log.Count}");

// Singleton — той самий інстанс
Console.WriteLine($"\nSingleton — той самий об'єкт: {ReferenceEquals(TaskRegistry.Instance, TaskRegistry.Instance)}");

Console.WriteLine("\nГотово!");
