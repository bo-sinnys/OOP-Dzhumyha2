using System.Diagnostics;
using IndependentWork24.Composite;
using IndependentWork24.Decorators;

// ─────────────────────────────────────────────────────────────────────
// Самостійна робота №24 — Composite + Decorator + Proxy
// Варіант 5: система завдань та підзавдань
// ─────────────────────────────────────────────────────────────────────

Print("ДЕМОНСТРАЦІЯ ПАТЕРНІВ", ConsoleColor.Cyan);

// ── Composite ────────────────────────────────────────────────────────
var write     = new TaskItem("Написати юніт-тести");
var review    = new TaskItem("Провести code review");
var deploy    = new TaskItem("Задеплоїти на staging", completed: true);
var fixBug    = new TaskItem("Виправити баг авторизації");
var updateDoc = new TaskItem("Оновити документацію");

var backend  = new ProjectTask("Backend: API v2");
backend.Add(write);
backend.Add(review);
backend.Add(deploy);

var sprint = new ProjectTask("Sprint #14");
sprint.Add(backend);
sprint.Add(fixBug);
sprint.Add(updateDoc);

Print("\n-- Ієрархія спринту (без декораторів) --", ConsoleColor.Yellow);
sprint.Display();

// ── Decorator ────────────────────────────────────────────────────────
IComponent urgentBug   = new PriorityDecorator(fixBug);
IComponent timedWrite  = new DueDateDecorator(write, new DateTime(2025, 4, 28));
IComponent critical    = new PriorityDecorator(
                             new DueDateDecorator(write, new DateTime(2025, 4, 28)));
IComponent priorityBackend = new PriorityDecorator(backend);

Print("\n-- Декоровані об'єкти --", ConsoleColor.Yellow);
Console.Write("Баг (HIGH PRIORITY): ");
urgentBug.Display();

Console.WriteLine("Тести з дедлайном:");
timedWrite.Display(1);

Console.WriteLine("Тести — critical (комбінований):");
critical.Display(1);

Console.WriteLine("Backend-проєкт — HIGH PRIORITY:");
priorityBackend.Display();

// ── Proxy (CachingProxyDecorator) ────────────────────────────────────
Print("\n-- Proxy: кешування GetTitle() --", ConsoleColor.Yellow);
var proxy = new CachingProxyDecorator(fixBug);

Console.WriteLine($"Кеш заповнений: {proxy.IsCached}");
Console.WriteLine($"GetTitle() #1: {proxy.GetTitle()}");
Console.WriteLine($"Кеш заповнений: {proxy.IsCached}");
Console.WriteLine($"GetTitle() #2 (з кешу): {proxy.GetTitle()}");
proxy.InvalidateCache();
Console.WriteLine($"Після інвалідації: {proxy.IsCached}");
Console.WriteLine($"GetTitle() #3 (знову MISS): {proxy.GetTitle()}");

proxy.Display();
proxy.Display();
Console.WriteLine($"Display() викликано разів: {proxy.DisplayCallCount}");

// ── Порівняння продуктивності ─────────────────────────────────────────
Print("\n-- Порівняння продуктивності --", ConsoleColor.Yellow);
const int N = 100_000;

// Без кешу
var taskDirect = new TaskItem("Тестове завдання для бенчмарку");
var sw = Stopwatch.StartNew();
for (int i = 0; i < N; i++) _ = taskDirect.GetTitle();
sw.Stop();
Console.WriteLine($"TaskItem.GetTitle()    x{N}: {sw.ElapsedMilliseconds} мс");

// З проксі (кеш)
var proxyBench = new CachingProxyDecorator(taskDirect);
sw.Restart();
for (int i = 0; i < N; i++) _ = proxyBench.GetTitle();
sw.Stop();
Console.WriteLine($"CachingProxy.GetTitle() x{N}: {sw.ElapsedMilliseconds} мс  (кеш заповнено після 1-го виклику)");

// Display без декоратора vs з двома декораторами
sw.Restart();
var output = new StringWriter();
Console.SetOut(output);
for (int i = 0; i < N; i++) taskDirect.Display();
sw.Stop();
long baseMs = sw.ElapsedMilliseconds;

IComponent layered = new PriorityDecorator(new DueDateDecorator(taskDirect, DateTime.Today.AddDays(5)));
sw.Restart();
for (int i = 0; i < N; i++) layered.Display();
sw.Stop();
long decorMs = sw.ElapsedMilliseconds;
Console.SetOut(new StreamWriter(new FileStream("/dev/stdout", FileMode.Open, FileAccess.Write)) { AutoFlush = true });

Console.WriteLine($"Display() без декораторів x{N}: {baseMs} мс");
Console.WriteLine($"Display() з 2 декораторами x{N}: {decorMs} мс  (накладні витрати: ~{decorMs - baseMs} мс)");

Print("\nГотово!", ConsoleColor.Green);

static void Print(string text, ConsoleColor color)
{
    Console.ForegroundColor = color;
    Console.WriteLine(text);
    Console.ResetColor();
}
