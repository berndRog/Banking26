using BankingApi.Data.Database;
using BankingApi.Domain;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Repositories;

public sealed class TransactionRepository(
   BankingDbContext _dbContext,
   ILogger<TransactionRepository> _logger
) : ITransactionRepository {

   public Task<Transaction?> FindByIdAsync(Guid transactionId) {
      _logger.LogDebug("Load Transaction {Id}", transactionId);

      return _dbContext.Transactions
         .FirstOrDefaultAsync(t => t.Id == transactionId);
   }

   public async Task AddAsync(Transaction transaction) {
      _logger.LogDebug(
         "Add Transaction {Id} Account={AccId} Amount={Amount}",
         transaction.Id, transaction.AccountId, transaction.Amount);

      await _dbContext.Transactions.AddAsync(transaction);
   }

   public async Task<IReadOnlyList<Transaction>> FindByAccountIdAsync(Guid accountId) {
      _logger.LogDebug("Load Transactions for Account {AccountId}", accountId);

      return await _dbContext.Transactions
         .Where(t => t.AccountId == accountId)
         .OrderByDescending(t => t.BookingDate)
         .ToListAsync();
   }
   
   public async Task<IReadOnlyList<Transaction>> FindByAccountIdAndPeriodAsync(
      Guid accountId,
      DateOnly fromDate,
      DateOnly toDate
   ) {
      var fromUtc = fromDate.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
      var toUtc   = toDate.ToDateTime(TimeOnly.MaxValue, DateTimeKind.Utc);

      _logger.LogDebug("Load transactions for account {Id} from {From} to {To}",
         accountId.To8(), fromDate, toDate);

      return await _dbContext.Transactions
         .Where(t =>
            t.AccountId == accountId &&
            t.BookingDate >= fromUtc &&
            t.BookingDate <= toUtc
         )
         .OrderByDescending(t => t.BookingDate)
         .ToListAsync();
   }
}