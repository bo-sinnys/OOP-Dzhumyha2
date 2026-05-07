using IndependentWork24.Composite;
using IndependentWork24.Decorators;

namespace IndependentWork24.Tests;

// Tests for IndependentWork24 -- Composite + Decorator + Proxy

public class CompositeTests
{
    // Test 1: TaskItem stores Title and IsCompleted correctly
    [Fact]
    public void TaskItem_StoresTitle_And_IsCompleted()
    {
        var task = new TaskItem("Написати тести", completed: true);

        Assert.Equal("Написати тести", task.GetTitle());
        Assert.True(task.IsCompleted());
    }

    // Test 2: ProjectTask IsCompleted only when all children are done
    [Fact]
    public void ProjectTask_IsCompleted_OnlyWhenAllChildrenCompleted()
    {
        var project = new ProjectTask("Sprint");
        var t1 = new TaskItem("Завдання 1", completed: true);
        var t2 = new TaskItem("Завдання 2", completed: false);

        project.Add(t1);
        project.Add(t2);

        Assert.False(project.IsCompleted());

        t2.Complete();
        Assert.True(project.IsCompleted());
    }

    // Test 3: Remove() deletes child correctly
    [Fact]
    public void ProjectTask_Remove_DeletesChild()
    {
        var project = new ProjectTask("Проект");
        var task = new TaskItem("Завдання");

        project.Add(task);
        Assert.Single(project.Children);

        project.Remove(task);
        Assert.Empty(project.Children);
    }

    // Test 4: nested Composite delegates IsCompleted recursively
    [Fact]
    public void ProjectTask_Nested_IsCompleted_DelegatesRecursively()
    {
        var inner = new ProjectTask("Підпроект");
        inner.Add(new TaskItem("A", completed: true));
        inner.Add(new TaskItem("B", completed: true));

        var outer = new ProjectTask("Головний");
        outer.Add(inner);
        outer.Add(new TaskItem("C", completed: true));

        Assert.True(outer.IsCompleted());
    }

    // Test 5 (boundary): empty ProjectTask returns false
    [Fact]
    public void ProjectTask_Empty_IsCompleted_ReturnsFalse()
    {
        var project = new ProjectTask("Порожній проект");

        Assert.False(project.IsCompleted());
    }
}

public class DecoratorTests
{
    // Test 6: PriorityDecorator adds prefix to GetTitle()
    [Fact]
    public void PriorityDecorator_AddsPrefix_ToTitle()
    {
        var task = new TaskItem("Виправити баг");
        var decorated = new PriorityDecorator(task);

        Assert.Contains("HIGH PRIORITY", decorated.GetTitle());
        Assert.Contains("Виправити баг", decorated.GetTitle());
    }

    // Test 7: DueDateDecorator IsOverdue returns true for past date
    [Fact]
    public void DueDateDecorator_IsOverdue_PastDate_ReturnsTrue()
    {
        var task = new TaskItem("Старе завдання");
        var decorated = new DueDateDecorator(task, new DateTime(2020, 1, 1));

        Assert.True(decorated.IsOverdue);
    }

    // Test 8: DueDateDecorator IsOverdue returns false for future date
    [Fact]
    public void DueDateDecorator_IsOverdue_FutureDate_ReturnsFalse()
    {
        var task = new TaskItem("Нове завдання");
        var decorated = new DueDateDecorator(task, DateTime.Today.AddDays(30));

        Assert.False(decorated.IsOverdue);
    }

    // Test 9: combined decorators preserve IsCompleted
    [Fact]
    public void CombinedDecorators_PreserveIsCompleted()
    {
        var task = new TaskItem("Завдання", completed: true);
        IComponent layered = new PriorityDecorator(
            new DueDateDecorator(task, DateTime.Today.AddDays(5)));

        Assert.True(layered.IsCompleted());
    }

    // Test 10 (boundary): decorator on empty Composite does not throw
    [Fact]
    public void PriorityDecorator_OnEmptyComposite_DoesNotThrow()
    {
        var project = new ProjectTask("Порожній");
        IComponent decorated = new PriorityDecorator(project);

        var ex = Record.Exception(() => decorated.Display());
        Assert.Null(ex);
    }
}

public class ProxyTests
{
    // Test 11: first GetTitle() is cache miss, second is cache hit
    [Fact]
    public void CachingProxy_GetTitle_CachesAfterFirstCall()
    {
        var task = new TaskItem("Завдання");
        var proxy = new CachingProxyDecorator(task);

        Assert.False(proxy.IsCached);
        _ = proxy.GetTitle();
        Assert.True(proxy.IsCached);
    }

    // Test 12: InvalidateCache clears the cache
    [Fact]
    public void CachingProxy_InvalidateCache_ClearsCache()
    {
        var task = new TaskItem("Завдання");
        var proxy = new CachingProxyDecorator(task);

        _ = proxy.GetTitle();
        Assert.True(proxy.IsCached);

        proxy.InvalidateCache();
        Assert.False(proxy.IsCached);
    }

    // Test 13: DisplayCallCount counts correctly
    [Fact]
    public void CachingProxy_CountsDisplayCalls()
    {
        var task = new TaskItem("Завдання");
        var proxy = new CachingProxyDecorator(task);

        var sw = new StringWriter();
        Console.SetOut(sw);

        proxy.Display();
        proxy.Display();
        proxy.Display();

        Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });

        Assert.Equal(3, proxy.DisplayCallCount);
    }

    // Test 14 (boundary): Proxy on Composite returns composite title
    [Fact]
    public void CachingProxy_OnComposite_GetTitle_ReturnsCompositeTitle()
    {
        var project = new ProjectTask("Мій проект");
        var proxy = new CachingProxyDecorator(project);

        Assert.Equal("Мій проект", proxy.GetTitle());
        Assert.True(proxy.IsCached);
    }
}
