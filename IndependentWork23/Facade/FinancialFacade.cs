namespace IndependentWork23.Facade;

/// <summary>
/// Facade pattern — Facade.
/// Надає єдину точку входу до складної фінансової підсистеми.
/// Клієнт не знає про AccountService та TransactionLogger.
/// </summary>
public class FinancialFacade
{
    private readonly AccountService _accountService;
    private readonly TransactionLogger _logger;

    public FinancialFacade(decimal initialBalance)
    {
        _accountService = new AccountService(initialBalance);
        _logger = new TransactionLogger();
    }

    public void PerformDeposit(string accountId, decimal amount)
    {
        _accountService.Deposit(accountId, amount);
        _logger.Log(accountId, "DEPOSIT", amount, success: true);
    }

    public bool PerformWithdrawal(string accountId, decimal amount)
    {
        bool success = _accountService.Withdraw(accountId, amount);
        _logger.Log(accountId, "WITHDRAW", amount, success);
        return success;
    }

    public decimal GetBalance() => _accountService.GetBalance();

    public void PrintTransactionLog() => _logger.PrintAll();
}
