using IndependentWork22.Composite;

namespace IndependentWork22.Decorators;

/// <summary>
/// Decorator pattern — ConcreteDecorator.
/// Додає префікс "[HIGH PRIORITY]" до виводу будь-якого IComponent.
/// </summary>
public class PriorityDecorator : TaskDecorator
{
    private readonly string _priorityLabel;

    public PriorityDecorator(IComponent component, string priorityLabel = "HIGH PRIORITY")
        : base(component)
    {
        _priorityLabel = priorityLabel;
    }

    public override void Display(int indent = 0)
    {
        string prefix = new string(' ', indent * 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{prefix}[{_priorityLabel}] ");
        Console.ResetColor();

        _component.Display(0);
    }
}
