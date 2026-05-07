using IndependentWork22.Composite;

namespace IndependentWork22.Decorators;

/// <summary>
/// Decorator pattern — абстрактний базовий декоратор.
/// Реалізує IComponent та зберігає посилання на обгорнутий компонент.
/// </summary>
public abstract class TaskDecorator : IComponent
{
    protected readonly IComponent _component;

    protected TaskDecorator(IComponent component)
    {
        _component = component;
    }

    public abstract void Display(int indent = 0);
}
