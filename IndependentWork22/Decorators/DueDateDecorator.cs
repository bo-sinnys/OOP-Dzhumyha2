using IndependentWork22.Composite;

namespace IndependentWork22.Decorators;

/// <summary>
/// Decorator pattern — ConcreteDecorator.
/// Додає інформацію про дедлайн до виводу будь-якого IComponent.
/// </summary>
public class DueDateDecorator : TaskDecorator
{
    private readonly DateTime _dueDate;

    public DueDateDecorator(IComponent component, DateTime dueDate)
        : base(component)
    {
        _dueDate = dueDate;
    }

    public override void Display(int indent = 0)
    {
        _component.Display(indent);

        string prefix = new string(' ', (indent + 1) * 2);
        bool isOverdue = _dueDate < DateTime.Today;

        Console.ForegroundColor = isOverdue ? ConsoleColor.DarkRed : ConsoleColor.DarkCyan;
        string overdueLabel = isOverdue ? " ⚠ ПРОСТРОЧЕНО!" : "";
        Console.WriteLine($"{prefix}└─ Термін: {_dueDate:dd.MM.yyyy}{overdueLabel}");
        Console.ResetColor();
    }
}
