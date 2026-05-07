namespace IndependentWork24.Composite;

/// <summary>Leaf — окреме завдання без підзавдань.</summary>
public class TaskItem : IComponent
{
    private readonly string _title;
    private bool _completed;

    public TaskItem(string title, bool completed = false)
    {
        _title = title;
        _completed = completed;
    }

    public void Complete() => _completed = true;

    public string GetTitle() => _title;
    public bool IsCompleted() => _completed;

    public void Display(int indent = 0)
    {
        string pad = new(' ', indent * 2);
        string status = _completed ? "[v]" : "[ ]";
        Console.WriteLine($"{pad}{status} {_title}");
    }
}
