using IndependentWork24.Composite;

namespace IndependentWork24.Decorators;

/// <summary>Додає префікс [HIGH PRIORITY] до будь-якого IComponent.</summary>
public class PriorityDecorator : TaskDecorator
{
    private readonly string _label;

    public PriorityDecorator(IComponent component, string label = "HIGH PRIORITY")
        : base(component) => _label = label;

    public override string GetTitle() => $"[{_label}] {_component.GetTitle()}";

    public override void Display(int indent = 0)
    {
        string pad = new(' ', indent * 2);
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{pad}[{_label}] ");
        Console.ResetColor();
        _component.Display(0);
    }
}
