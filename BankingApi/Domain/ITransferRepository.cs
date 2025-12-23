using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface ITransferRepository {
   Task<Transfer?> FindByIdAsync(
      Guid id, 
      CancellationToken ct = default
   );
   
   Task<IReadOnlyList<Transfer>> FindByAccountIdAsync(
      Guid accountId, 
      CancellationToken ct = default
   );
   
   void Add(Transfer transfer);
   
   Task<bool> ExistsReversalForAsync(
      Guid originalTransferId, 
      CancellationToken ct = default
   );
   
}
