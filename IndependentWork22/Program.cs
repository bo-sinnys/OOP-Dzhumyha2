using IndependentWork22.Composite;
using IndependentWork22.Decorators;

// ─────────────────────────────────────────────────────────────────────────────
// Самостійна робота №22 — Composite + Decorator
// Варіант 5: Сценарій «Завдання та підзавдання»
// ─────────────────────────────────────────────────────────────────────────────

PrintHeader("СИСТЕМА КЕРУВАННЯ ЗАВДАННЯМИ");

// ── 1. Прості завдання (Leaf) ────────────────────────────────────────────────
var designMockup    = new TaskItem("Створити макет UI", isCompleted: true);
var writeTests      = new TaskItem("Написати юніт-тести", isCompleted: false);
var codeReview      = new TaskItem("Провести code review", isCompleted: false);
var deployStaging   = new TaskItem("Задеплоїти на staging", isCompleted: false);
var updateDocs      = new TaskItem("Оновити документацію", isCompleted: false);
var fixLoginBug     = new TaskItem("Виправити баг авторизації", isCompleted: false);
var optimizeQueries = new TaskItem("Оптимізувати SQL-запити", isCompleted: true);
var backupDB        = new TaskItem("Зробити резервну копію БД", isCompleted: false);

// ── 2. Вкладені ProjectTask (Composite) ─────────────────────────────────────
var backendProject = new ProjectTask("Backend: API v2");
backendProject.Add(writeTests);
backendProject.Add(codeReview);
backendProject.Add(optimizeQueries);

var frontendProject = new ProjectTask("Frontend: Новий дашборд");
frontendProject.Add(designMockup);
frontendProject.Add(deployStaging);

var maintenanceProject = new ProjectTask("Технічне обслуговування");
maintenanceProject.Add(backupDB);
maintenanceProject.Add(updateDocs);

// Кореневий композит — весь спринт
var sprint = new ProjectTask("Sprint #14 — Реліз 2.0");
sprint.Add(backendProject);
sprint.Add(frontendProject);
sprint.Add(maintenanceProject);
sprint.Add(fixLoginBug);

// ── 3. Декоратори ────────────────────────────────────────────────────────────

IComponent urgentBug = new PriorityDecorator(fixLoginBug);

IComponent timedDeploy = new DueDateDecorator(deployStaging, new DateTime(2025, 5, 10));

IComponent criticalTests = new PriorityDecorator(
    new DueDateDecorator(writeTests, new DateTime(2025, 4, 28))
);

IComponent timedBackend = new DueDateDecorator(backendProject, new DateTime(2025, 5, 20));

IComponent priorityFrontend = new PriorityDecorator(frontendProject);

// ── 4. Виведення ─────────────────────────────────────────────────────────────

PrintSection("Повна ієрархія спринту (без декораторів)");
sprint.Display();

PrintSection("Окремі завдання з декораторами");

Console.WriteLine("▶ Баг авторизації — HIGH PRIORITY:");
urgentBug.Display(1);

Console.WriteLine();
Console.WriteLine("▶ Задеплоїти на staging + дедлайн:");
timedDeploy.Display(1);

Console.WriteLine();
Console.WriteLine("▶ Юніт-тести — HIGH PRIORITY + прострочений дедлайн:");
criticalTests.Display(1);

PrintSection("Composite з декораторами");

Console.WriteLine("▶ Backend-проєкт з дедлайном:");
timedBackend.Display();

Console.WriteLine();
Console.WriteLine("▶ Frontend-проєкт — HIGH PRIORITY:");
priorityFrontend.Display();

PrintSection("Демонстрація Remove() — видаляємо code review з backend");
backendProject.Remove(codeReview);
backendProject.Display();

PrintFooter();


// ── Допоміжні методи виводу ──────────────────────────────────────────────────

static void PrintHeader(string title)
{
    Console.ForegroundColor = ConsoleColor.Cyan;
    Console.WriteLine(new string('═', 60));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('═', 60));
    Console.ResetColor();
    Console.WriteLine();
}

static void PrintSection(string title)
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine(new string('─', 60));
    Console.WriteLine($"  {title}");
    Console.WriteLine(new string('─', 60));
    Console.ResetColor();
}

static void PrintFooter()
{
    Console.WriteLine();
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine(new string('═', 60));
    Console.WriteLine("  Програму завершено успішно.");
    Console.WriteLine(new string('═', 60));
    Console.ResetColor();
}
