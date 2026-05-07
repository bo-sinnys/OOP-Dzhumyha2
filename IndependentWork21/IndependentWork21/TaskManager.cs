using IndependentWork21.Observer;

namespace IndependentWork21;

/// <summary>Subject — керує підписниками та сповіщає їх про події.</summary>
public class TaskManager : ITaskSubject
{
    private readonly List<ITaskObserver> _observers = new();

    public void Subscribe(ITaskObserver observer)
    {
        if (!_observers.Contains(observer))
            _observers.Add(observer);
    }

    public void Unsubscribe(ITaskObserver observer)
        => _observers.Remove(observer);

    public void Notify(TaskEventArgs e)
    {
        foreach (var o in _observers)
            o.OnTaskEvent(e);
    }

    public IReadOnlyList<ITaskObserver> Observers => _observers;
}
