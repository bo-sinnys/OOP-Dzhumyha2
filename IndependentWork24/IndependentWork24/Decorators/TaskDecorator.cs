using IndependentWork24.Composite;

namespace IndependentWork24.Decorators;

/// <summary>Базовий абстрактний декоратор.</summary>
public abstract class TaskDecorator : IComponent
{
    protected readonly IComponent _component;
    protected TaskDecorator(IComponent component) => _component = component;

    public virtual string GetTitle() => _component.GetTitle();
    public virtual bool IsCompleted() => _component.IsCompleted();
    public abstract void Display(int indent = 0);
}
