using IndependentWork24.Composite;

namespace IndependentWork24.Decorators;

/// <summary>
/// Proxy-декоратор: кешує результат GetTitle() та лічить кількість звернень до Display().
/// Демонструє інтеграцію патерну Proxy разом із Decorator.
/// </summary>
public class CachingProxyDecorator : TaskDecorator
{
    private string? _cachedTitle;
    private int _displayCallCount;

    public int DisplayCallCount => _displayCallCount;
    public bool IsCached => _cachedTitle is not null;

    public CachingProxyDecorator(IComponent component) : base(component) { }

    public override string GetTitle()
    {
        if (_cachedTitle is null)
            _cachedTitle = _component.GetTitle();   // cache MISS
        return _cachedTitle;
    }

    public void InvalidateCache() => _cachedTitle = null;

    public override void Display(int indent = 0)
    {
        _displayCallCount++;
        _component.Display(indent);
    }
}
