namespace IndependentWork23.Proxy;

/// <summary>
/// Proxy pattern — Subject.
/// Спільний інтерфейс для реального рахунку та проксі.
/// </summary>
public interface IBankAccount
{
    decimal GetBalance();
    bool Withdraw(decimal amount);
}
