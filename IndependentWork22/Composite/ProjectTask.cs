namespace IndependentWork22.Composite;

/// <summary>
/// Composite pattern — Composite.
/// Представляє проєктне завдання, яке може містити підзавдання або інші проєктні завдання.
/// </summary>
public class ProjectTask : IComponent
{
    public string Title { get; }
    private readonly List<IComponent> _children = new();

    public ProjectTask(string title)
    {
        Title = title;
    }

    public void Add(IComponent component)
    {
        _children.Add(component);
    }

    public void Remove(IComponent component)
    {
        _children.Remove(component);
    }

    public virtual void Display(int indent = 0)
    {
        string prefix = new string(' ', indent * 2);
        Console.WriteLine($"{prefix}[PROJECT] {Title}");

        foreach (IComponent child in _children)
        {
            child.Display(indent + 1);
        }
    }
}
