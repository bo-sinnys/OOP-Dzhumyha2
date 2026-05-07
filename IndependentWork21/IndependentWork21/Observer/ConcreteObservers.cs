namespace IndependentWork21.Observer;

/// <summary>Логує всі події завдань.</summary>
public class LoggerObserver : ITaskObserver
{
    private readonly List<string> _log = new();
    public string Name => "Logger";
    public IReadOnlyList<string> Log => _log;

    public void OnTaskEvent(TaskEventArgs e)
    {
        string entry = $"[{e.EventType}] {e.TaskTitle}" + (e.Details != null ? $" — {e.Details}" : "");
        _log.Add(entry);
        Console.WriteLine($"  [LOG] {entry}");
    }
}

/// <summary>Сповіщає про завершені завдання.</summary>
public class CompletionObserver : ITaskObserver
{
    public string Name => "CompletionTracker";
    public List<string> CompletedTasks { get; } = new();

    public void OnTaskEvent(TaskEventArgs e)
    {
        if (e.EventType == TaskEventType.Completed)
        {
            CompletedTasks.Add(e.TaskTitle);
            Console.WriteLine($"  [DONE] Task completed: {e.TaskTitle}");
        }
    }
}
