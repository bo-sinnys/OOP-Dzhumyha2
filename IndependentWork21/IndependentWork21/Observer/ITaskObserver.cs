namespace IndependentWork21.Observer;

public enum TaskEventType { Created, Completed, PriorityChanged }

public class TaskEventArgs
{
    public string TaskTitle { get; }
    public TaskEventType EventType { get; }
    public string? Details { get; }

    public TaskEventArgs(string taskTitle, TaskEventType eventType, string? details = null)
    {
        TaskTitle = taskTitle;
        EventType = eventType;
        Details = details;
    }
}

/// <summary>Observer pattern — Subject interface.</summary>
public interface ITaskSubject
{
    void Subscribe(ITaskObserver observer);
    void Unsubscribe(ITaskObserver observer);
    void Notify(TaskEventArgs e);
}

/// <summary>Observer pattern — Observer interface.</summary>
public interface ITaskObserver
{
    string Name { get; }
    void OnTaskEvent(TaskEventArgs e);
}
