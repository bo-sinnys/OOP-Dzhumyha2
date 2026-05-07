using IndependentWork21.Strategy;

namespace IndependentWork21.Singleton;

/// <summary>
/// Singleton pattern — глобальний реєстр завдань.
/// Зберігає список всіх TaskInfo та поточну стратегію сортування.
/// </summary>
public class TaskRegistry
{
    private static TaskRegistry? _instance;
    private static readonly object _lock = new();

    private readonly List<TaskInfo> _tasks = new();
    private ITaskSortStrategy _sortStrategy = new SortByPriority();

    private TaskRegistry() { }

    public static TaskRegistry Instance
    {
        get
        {
            if (_instance is null)
                lock (_lock)
                    _instance ??= new TaskRegistry();
            return _instance;
        }
    }

    /// <summary>Скидає інстанс — лише для тестів.</summary>
    public static void ResetForTesting()
    {
        lock (_lock) _instance = null;
    }

    public void Register(TaskInfo task) => _tasks.Add(task);
    public void Remove(TaskInfo task) => _tasks.Remove(task);
    public void Clear() => _tasks.Clear();

    public void SetStrategy(ITaskSortStrategy strategy) => _sortStrategy = strategy;
    public string CurrentStrategy => _sortStrategy.Name;

    public IEnumerable<TaskInfo> GetSorted() => _sortStrategy.Sort(_tasks);
    public IReadOnlyList<TaskInfo> All => _tasks;
}
