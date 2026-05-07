namespace IndependentWork22.Composite;

/// <summary>
/// Composite pattern — Leaf.
/// Представляє окреме завдання без підзавдань.
/// </summary>
public class TaskItem : IComponent
{
    public string Title { get; }
    public bool IsCompleted { get; }

    public TaskItem(string title, bool isCompleted = false)
    {
        Title = title;
        IsCompleted = isCompleted;
    }

    public virtual void Display(int indent = 0)
    {
        string prefix = new string(' ', indent * 2);
        string status = IsCompleted ? "[✓]" : "[ ]";
        Console.WriteLine($"{prefix}{status} {Title}");
    }
}
