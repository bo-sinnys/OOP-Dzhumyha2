using BankingSystem.Domain.Entities;
using BankingSystem.Domain.Exceptions;
using BankingSystem.Domain.Interfaces;

namespace BankingSystem.Application.Services;

/// <summary>
/// Сервіс для закриття рахунку з перевіркою бізнес-правил.
/// Issue #2 — feat: add account close functionality
/// </summary>
public class AccountCloseService
{
    private readonly IAccountRepository _accountRepo;

    public AccountCloseService(IAccountRepository accountRepo)
    {
        _accountRepo = accountRepo ?? throw new ArgumentNullException(nameof(accountRepo));
    }

    /// <summary>
    /// Закриває рахунок. Перед закриттям баланс має бути нульовим.
    /// </summary>
    /// <exception cref="AccountNotFoundException">Якщо рахунок не знайдено.</exception>
    /// <exception cref="DomainException">Якщо баланс не нульовий або рахунок вже закрито.</exception>
    public void CloseAccount(Guid accountId)
    {
        var account = _accountRepo.GetById(accountId)
            ?? throw new AccountNotFoundException(accountId);

        account.Close();
        _accountRepo.Save(account);
    }

    /// <summary>
    /// Повертає true якщо рахунок можна закрити (активний і баланс = 0).
    /// </summary>
    public bool CanClose(Guid accountId)
    {
        var account = _accountRepo.GetById(accountId);
        return account is { IsActive: true, Balance: 0 };
    }
}
