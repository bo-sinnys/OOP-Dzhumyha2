namespace IndependentWork24.Composite;

public interface IComponent
{
    string GetTitle();
    bool IsCompleted();
    void Display(int indent = 0);
}
