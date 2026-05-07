namespace IndependentWork24.Composite;

/// <summary>Composite — проєкт, що містить підзавдання або інші проєкти.</summary>
public class ProjectTask : IComponent
{
    private readonly string _title;
    private readonly List<IComponent> _children = new();

    public ProjectTask(string title) => _title = title;

    public void Add(IComponent c) => _children.Add(c);
    public void Remove(IComponent c) => _children.Remove(c);
    public IReadOnlyList<IComponent> Children => _children;

    public string GetTitle() => _title;

    /// <summary>Проєкт вважається завершеним, коли всі дочірні елементи завершені.</summary>
    public bool IsCompleted() => _children.Count > 0 && _children.All(c => c.IsCompleted());

    public void Display(int indent = 0)
    {
        string pad = new(' ', indent * 2);
        Console.WriteLine($"{pad}[PROJECT] {_title}");
        foreach (var child in _children)
            child.Display(indent + 1);
    }
}
