using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface IOwnerRepository {

   Task<Owner?> FindByIdAsync(Guid ownerId, CancellationToken ct = default);
   void Add(Owner owner);
   void Remove(Owner owner);

   Task<bool> HasAccountsAsync(Guid ownerId, CancellationToken ct = default);
}
