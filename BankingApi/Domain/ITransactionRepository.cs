using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface ITransactionRepository {
   Task<Transaction?> FindByIdAsync(Guid transactionId);
   Task<IReadOnlyList<Transaction>> FindByAccountIdAsync(Guid accountId);
   Task<IReadOnlyList<Transaction>> FindByAccountIdAndPeriodAsync(
      Guid accountId,
      DateOnly fromDate,
      DateOnly toDate
   );
   Task AddAsync(Transaction transaction);
}
