namespace IndependentWork21.Strategy;

public enum Priority { Low, Medium, High }

public class TaskInfo
{
    public string Title { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }

    public TaskInfo(string title, Priority priority, DateTime dueDate, bool isCompleted = false)
    {
        Title = title;
        Priority = priority;
        DueDate = dueDate;
        IsCompleted = isCompleted;
    }
}

/// <summary>Strategy pattern — interface.</summary>
public interface ITaskSortStrategy
{
    string Name { get; }
    IEnumerable<TaskInfo> Sort(IEnumerable<TaskInfo> tasks);
}

/// <summary>Сортування за пріоритетом (High -> Low).</summary>
public class SortByPriority : ITaskSortStrategy
{
    public string Name => "ByPriority";
    public IEnumerable<TaskInfo> Sort(IEnumerable<TaskInfo> tasks)
        => tasks.OrderByDescending(t => t.Priority);
}

/// <summary>Сортування за дедлайном (найближчий спочатку).</summary>
public class SortByDueDate : ITaskSortStrategy
{
    public string Name => "ByDueDate";
    public IEnumerable<TaskInfo> Sort(IEnumerable<TaskInfo> tasks)
        => tasks.OrderBy(t => t.DueDate);
}

/// <summary>Сортування: спочатку невиконані, потім за пріоритетом.</summary>
public class SortByCompletion : ITaskSortStrategy
{
    public string Name => "ByCompletion";
    public IEnumerable<TaskInfo> Sort(IEnumerable<TaskInfo> tasks)
        => tasks.OrderBy(t => t.IsCompleted).ThenByDescending(t => t.Priority);
}
