using BankingApi.Data.Database;
using BankingApi.Domain;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Repositories;

public class OwnerRepository(
   BankingDbContext _dbContext,
   ILogger<OwnerRepository> _logger
) : IOwnerRepository {

   public async Task<Owner?> FindByIdAsync(
      Guid ownerId, 
      CancellationToken ct = default
   ) {
      _logger.LogDebug("loading Owner {Id}", ownerId.To8());
      return await _dbContext.Owners
         .AsTracking()
         .FirstOrDefaultAsync(o => o.Id == ownerId, ct);
   }

   public void Add(Owner owner) {
      _logger.LogDebug("adding Owner {Id}", owner.Id.To8());
      _dbContext.Owners.Add(owner);
   }
   
   public void Remove(Owner owner) {
      _logger.LogDebug("removing Owner {Id}", owner.Id.To8());
      _dbContext.Owners.Remove(owner);
   }

   public Task<bool> HasAccountsAsync(Guid ownerId, CancellationToken ct = default) {
      _logger.LogDebug("checking if Owner {Id} has Accounts", ownerId.To8());
      return _dbContext.Accounts
         .AsNoTracking()
         .AnyAsync(a => a.OwnerId == ownerId, ct);
   }
}