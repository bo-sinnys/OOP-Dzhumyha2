namespace IndependentWork22.Composite;

/// <summary>
/// Composite pattern: спільний інтерфейс для листів та складених об'єктів.
/// </summary>
public interface IComponent
{
    void Display(int indent = 0);
}
