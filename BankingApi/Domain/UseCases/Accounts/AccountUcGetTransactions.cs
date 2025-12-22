using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcGetTransactions(
   IAccountRepository accountRepository,
   ITransactionRepository transactionRepository,
   ILogger<AccountUcGetTransactions> logger
) : IAccountUcGetTransactions {

   public async Task<Result<IReadOnlyList<Entities.Transaction>>> ExecuteAsync(
      Guid accountId,
      DateOnly fromDate,
      DateOnly toDate
   ) {

      if (fromDate > toDate) {
         logger.LogWarning(
            "Invalid period for account {AccountId}: {From} > {To}",
            accountId.To8(), fromDate, toDate
         );
         return Result<IReadOnlyList<Entities.Transaction>>
            .Fail(TransactionErrors.InvalidPeriod);
      }

      var account = await accountRepository.FindByIdAsync(accountId);
      if (account is null) {
         logger.LogWarning(
            "Get transactions failed: account not found ({AccountId})",
            accountId.To8()
         );
         return Result<IReadOnlyList<Entities.Transaction>>
            .Fail(TransactionErrors.AccountNotFound);
      }

      var transactions = await transactionRepository
            .FindByAccountIdAndPeriodAsync(accountId, fromDate, toDate);

      logger.LogInformation(
         "Loaded {Count} transactions for account {AccountId}",
         transactions.Count, accountId.To8()
      );

      return Result<IReadOnlyList<Entities.Transaction>>
         .Success(transactions);
   }
}
