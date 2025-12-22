using BankingApi.Domain;
using BankingApi.Domain.Entities;
using BankingApi.Data.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BankingApi.Data.Repositories;

public sealed class TransferRepository(
   BankingDbContext dbContext,
   ILogger<TransferRepository> logger
) : ITransferRepository {

   private readonly BankingDbContext _dbContext = dbContext;
   private readonly ILogger<TransferRepository> _logger = logger;

   public async Task<Transfer?> FindByIdAsync(Guid transferId) {
      _logger.LogDebug("Loading Transfer {Id}", transferId);

      return await _dbContext.Transfers
         .FirstOrDefaultAsync(t => t.Id == transferId);
   }

   public async Task AddAsync(Transfer transfer) {
      _logger.LogDebug(
         "Adding Transfer {Id} From={From} To={To} Amount={Amount}",
         transfer.Id,
         transfer.FromAccountId,
         transfer.ToAccountId,
         transfer.Amount
      );

      await _dbContext.Transfers.AddAsync(transfer);
   }

   public Task<IReadOnlyList<Transfer>> FindByAccountIdAsync(Guid accountId) {
      throw new NotImplementedException();
   }
   
   public async Task<bool> ExistsReversalForAsync(Guid originalTransferId) {
      return await _dbContext.Transfers
         .AnyAsync(t => t.ReversalOfTransferId == originalTransferId);
   }


}
