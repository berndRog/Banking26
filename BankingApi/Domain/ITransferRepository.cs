using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface ITransferRepository {
   
   Task<Transfer?> FindByIdAsync(Guid id);
   Task<IReadOnlyList<Transfer>> FindByAccountIdAsync(Guid accountId);
   Task AddAsync(Transfer transfer);
   Task<bool> ExistsReversalForAsync(Guid originalTransferId);
   
}
