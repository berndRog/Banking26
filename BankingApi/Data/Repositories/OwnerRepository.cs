using BankingApi.Data.Database;
using BankingApi.Domain;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Repositories;

public class OwnerRepository(
   BankingDbContext db,
   ILogger<OwnerRepository> logger
) : IOwnerRepository {
   
   private readonly BankingDbContext _db = db;
   private readonly ILogger<OwnerRepository> _logger = logger;

   public async Task<Owner?> FindByIdAsync(Guid ownerId) {
      _logger.LogDebug("loading Owner {Id}", ownerId.To8());
      return await _db.Owners
         .AsTracking()
         .FirstOrDefaultAsync(o => o.Id == ownerId);
   }

   public async Task AddAsync(Owner owner) {
      _logger.LogDebug("adding Owner {Id}", owner.Id.To8());

      await _db.Owners.AddAsync(owner);
   }

   public Task UpdateAsync(Owner owner) {
      _logger.LogDebug("Repository: updating Owner {Id}", owner.Id.To8());
      _db.Owners.Update(owner);
      return Task.CompletedTask;
   }

   public Task RemoveAsync(Owner owner) {
      _logger.LogDebug("removing Owner {Id}", owner.Id.To8());
      _db.Owners.Remove(owner);
      return Task.CompletedTask;
   }

   public Task<bool> HasAccountsAsync(Guid ownerId) {
      throw new NotImplementedException();
   }
}