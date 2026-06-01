## Bug Fix: Overdraft Fee Applied Twice

**Issue #3** — fix: overdraft fee charged twice when withdrawal equals exact overdraft limit

### Problem
When a withdrawal amount equals exactly `Balance + OverdraftLimit`, the resulting balance
becomes exactly `-OverdraftLimit`. The condition `Balance < 0` is true, so the overdraft fee
is applied — but the user has not actually used any overdraft (they withdrew the maximum allowed).

### Root cause
`CheckingAccount.Withdraw()` — condition `if (Balance < 0)` triggers at boundary value.

### Fix
Changed condition to `if (Balance < 0 && amount > originalBalance)` so the fee is charged
only when the withdrawal caused a genuine overdraft.

### Files changed
- `src/BankingSystem.Domain/Entities/Account.cs` — CheckingAccount.Withdraw()
- `tests/BankingSystem.Tests/CheckingAccountTests.cs` — added boundary test
