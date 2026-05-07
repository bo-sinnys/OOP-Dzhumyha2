using IndependentWork21;
using IndependentWork21.Factory;
using IndependentWork21.Observer;
using IndependentWork21.Singleton;
using IndependentWork21.Strategy;

namespace IndependentWork21.Tests;

// Кожен тест скидає Singleton щоб уникнути залежності між тестами
public class IntegrationTests : IDisposable
{
    private readonly TaskManager _manager;
    private readonly TaskFactory _factory;
    private readonly LoggerObserver _logger;
    private readonly CompletionObserver _completion;

    public IntegrationTests()
    {
        TaskRegistry.ResetForTesting();
        _manager = new TaskManager();
        _logger = new LoggerObserver();
        _completion = new CompletionObserver();
        _manager.Subscribe(_logger);
        _manager.Subscribe(_completion);
        _factory = new TaskFactory(_manager);
    }

    public void Dispose() => TaskRegistry.ResetForTesting();

    // ----------------------------------------------------------------
    // ПОЗИТИВНІ СЦЕНАРІЇ
    // ----------------------------------------------------------------

    // Test 1: Factory створює завдання і реєструє його в Singleton
    [Fact]
    public void Factory_CreatesTask_RegistersInSingleton()
    {
        var task = _factory.CreateTask("Завдання 1", Priority.High, DateTime.Today.AddDays(3));

        Assert.NotNull(task);
        Assert.Equal("Завдання 1", task.Title);
        Assert.Contains(task, TaskRegistry.Instance.All);
        Assert.Single(TaskRegistry.Instance.All);
    }

    // Test 2: Factory сповіщає Observer при створенні завдання
    [Fact]
    public void Factory_CreatesTask_NotifiesObservers()
    {
        _factory.CreateTask("Тестове завдання", Priority.Medium, DateTime.Today.AddDays(5));

        Assert.Single(_logger.Log);
        Assert.Contains("Created", _logger.Log[0]);
        Assert.Contains("Тестове завдання", _logger.Log[0]);
    }

    // Test 3: CompleteTask сповіщає CompletionObserver і оновлює стан
    [Fact]
    public void Factory_CompleteTask_UpdatesStateAndNotifiesObserver()
    {
        var task = _factory.CreateTask("Завдання для завершення", Priority.Low, DateTime.Today.AddDays(1));
        _factory.CompleteTask(task);

        Assert.True(task.IsCompleted);
        Assert.Contains("Завдання для завершення", _completion.CompletedTasks);
    }

    // Test 4: Singleton повертає той самий інстанс
    [Fact]
    public void Singleton_AlwaysReturnsSameInstance()
    {
        var a = TaskRegistry.Instance;
        var b = TaskRegistry.Instance;

        Assert.Same(a, b);
    }

    // Test 5: Singleton зберігає стан між кількома зверненнями через Factory
    [Fact]
    public void Singleton_PersistsState_AcrossMultipleFactoryCalls()
    {
        _factory.CreateTask("Завдання A", Priority.High, DateTime.Today);
        _factory.CreateTask("Завдання B", Priority.Low, DateTime.Today.AddDays(2));
        _factory.CreateTask("Завдання C", Priority.Medium, DateTime.Today.AddDays(5));

        Assert.Equal(3, TaskRegistry.Instance.All.Count);
    }

    // Test 6: Strategy SortByPriority — High іде першим
    [Fact]
    public void Strategy_SortByPriority_HighFirst()
    {
        _factory.CreateTask("Low task",    Priority.Low,    DateTime.Today.AddDays(1));
        _factory.CreateTask("High task",   Priority.High,   DateTime.Today.AddDays(2));
        _factory.CreateTask("Medium task", Priority.Medium, DateTime.Today.AddDays(3));

        TaskRegistry.Instance.SetStrategy(new SortByPriority());
        var sorted = TaskRegistry.Instance.GetSorted().ToList();

        Assert.Equal("High task", sorted[0].Title);
        Assert.Equal("Low task",  sorted[2].Title);
    }

    // Test 7: Strategy SortByDueDate — найближчий дедлайн першим
    [Fact]
    public void Strategy_SortByDueDate_EarliestFirst()
    {
        _factory.CreateTask("Далеке",   Priority.Low,  DateTime.Today.AddDays(10));
        _factory.CreateTask("Близьке",  Priority.High, DateTime.Today.AddDays(1));
        _factory.CreateTask("Середнє", Priority.Medium, DateTime.Today.AddDays(5));

        TaskRegistry.Instance.SetStrategy(new SortByDueDate());
        var sorted = TaskRegistry.Instance.GetSorted().ToList();

        Assert.Equal("Близьке", sorted[0].Title);
        Assert.Equal("Далеке",  sorted[2].Title);
    }

    // Test 8: Runtime зміна Strategy — результат сортування змінюється
    [Fact]
    public void Strategy_RuntimeSwitch_ChangesSortResult()
    {
        _factory.CreateTask("Low-Far",   Priority.Low,  DateTime.Today.AddDays(10));
        _factory.CreateTask("High-Near", Priority.High, DateTime.Today.AddDays(1));

        TaskRegistry.Instance.SetStrategy(new SortByPriority());
        var byPriority = TaskRegistry.Instance.GetSorted().First().Title;

        TaskRegistry.Instance.SetStrategy(new SortByDueDate());
        var byDate = TaskRegistry.Instance.GetSorted().First().Title;

        Assert.Equal("High-Near", byPriority);
        Assert.Equal("High-Near", byDate);
        Assert.Equal("ByDueDate", TaskRegistry.Instance.CurrentStrategy);
    }

    // Test 9: ChangePriority сповіщає Logger з деталями
    [Fact]
    public void Factory_ChangePriority_NotifiesObserverWithDetails()
    {
        var task = _factory.CreateTask("Завдання", Priority.Low, DateTime.Today.AddDays(3));
        _factory.ChangePriority(task, Priority.High);

        Assert.Equal(Priority.High, task.Priority);
        Assert.Equal(2, _logger.Log.Count);
        Assert.Contains("PriorityChanged", _logger.Log[1]);
        Assert.Contains("Low -> High", _logger.Log[1]);
    }

    // Test 10: Unsubscribe — відписаний observer не отримує події
    [Fact]
    public void Observer_Unsubscribe_StopsReceivingEvents()
    {
        _manager.Unsubscribe(_logger);
        _factory.CreateTask("Після відписки", Priority.Medium, DateTime.Today);

        Assert.Empty(_logger.Log);
    }

    // ----------------------------------------------------------------
    // НЕГАТИВНІ / ГРАНИЧНІ СЦЕНАРІЇ
    // ----------------------------------------------------------------

    // Test 11 (negative): Factory кидає виняток при порожній назві
    [Fact]
    public void Factory_EmptyTitle_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _factory.CreateTask("", Priority.High, DateTime.Today));
    }

    // Test 12 (negative): Factory кидає виняток при назві з пробілів
    [Fact]
    public void Factory_WhitespaceTitle_ThrowsArgumentException()
    {
        Assert.Throws<ArgumentException>(() =>
            _factory.CreateTask("   ", Priority.Medium, DateTime.Today));
    }

    // Test 13 (boundary): порожній реєстр — GetSorted повертає порожній список
    [Fact]
    public void Strategy_EmptyRegistry_ReturnsEmptyList()
    {
        TaskRegistry.Instance.SetStrategy(new SortByPriority());
        var result = TaskRegistry.Instance.GetSorted().ToList();

        Assert.Empty(result);
    }

    // Test 14 (boundary): CompletionObserver не реагує на Created/PriorityChanged
    [Fact]
    public void Observer_CompletionObserver_IgnoresNonCompletionEvents()
    {
        var task = _factory.CreateTask("Завдання", Priority.Low, DateTime.Today.AddDays(1));
        _factory.ChangePriority(task, Priority.High);

        Assert.Empty(_completion.CompletedTasks);
    }

    // Test 15 (boundary): SortByCompletion — завершені йдуть в кінець
    [Fact]
    public void Strategy_SortByCompletion_CompletedLast()
    {
        var t1 = _factory.CreateTask("Активне",    Priority.Low,  DateTime.Today);
        var t2 = _factory.CreateTask("Завершене",  Priority.High, DateTime.Today);
        _factory.CompleteTask(t2);

        TaskRegistry.Instance.SetStrategy(new SortByCompletion());
        var sorted = TaskRegistry.Instance.GetSorted().ToList();

        Assert.False(sorted[0].IsCompleted);
        Assert.True(sorted[1].IsCompleted);
    }
}
