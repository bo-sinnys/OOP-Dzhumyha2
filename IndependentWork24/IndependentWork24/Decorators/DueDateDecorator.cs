using IndependentWork24.Composite;

namespace IndependentWork24.Decorators;

/// <summary>Додає інформацію про дедлайн; попереджає про прострочення.</summary>
public class DueDateDecorator : TaskDecorator
{
    private readonly DateTime _dueDate;

    public DueDateDecorator(IComponent component, DateTime dueDate)
        : base(component) => _dueDate = dueDate;

    public bool IsOverdue => _dueDate < DateTime.Today;
    public DateTime DueDate => _dueDate;

    public override void Display(int indent = 0)
    {
        _component.Display(indent);
        string pad = new(' ', (indent + 1) * 2);
        Console.ForegroundColor = IsOverdue ? ConsoleColor.DarkRed : ConsoleColor.DarkCyan;
        string warn = IsOverdue ? " ПРОСТРОЧЕНО!" : "";
        Console.WriteLine($"{pad}Термін: {_dueDate:dd.MM.yyyy}{warn}");
        Console.ResetColor();
    }
}
