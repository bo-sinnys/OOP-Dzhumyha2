using IndependentWork21.Observer;
using IndependentWork21.Singleton;
using IndependentWork21.Strategy;

namespace IndependentWork21.Factory;

/// <summary>
/// Factory pattern — створює TaskInfo, реєструє у Singleton
/// та сповіщає Observer-підписників.
/// </summary>
public class TaskFactory
{
    private readonly ITaskSubject _subject;

    public TaskFactory(ITaskSubject subject)
    {
        _subject = subject;
    }

    public TaskInfo CreateTask(string title, Priority priority, DateTime dueDate)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Task title cannot be empty.", nameof(title));

        var task = new TaskInfo(title, priority, dueDate);
        TaskRegistry.Instance.Register(task);
        _subject.Notify(new TaskEventArgs(title, TaskEventType.Created));
        return task;
    }

    public void CompleteTask(TaskInfo task)
    {
        task.IsCompleted = true;
        _subject.Notify(new TaskEventArgs(task.Title, TaskEventType.Completed));
    }

    public void ChangePriority(TaskInfo task, Priority newPriority)
    {
        string details = $"{task.Priority} -> {newPriority}";
        task.Priority = newPriority;
        _subject.Notify(new TaskEventArgs(task.Title, TaskEventType.PriorityChanged, details));
    }
}
