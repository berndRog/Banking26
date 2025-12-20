using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface IOwnerRepository {

   Task AddAsync(Owner owner);
   Task<Owner?> FindByIdAsync(Guid ownerId);
   Task UpdateAsync(Owner owner);
   Task RemoveAsync(Owner owner);

   Task<bool> HasAccountsAsync(Guid ownerId);
}
