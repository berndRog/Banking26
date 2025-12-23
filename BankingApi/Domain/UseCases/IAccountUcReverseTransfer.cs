using BankingApi.Domain.Entities;

namespace BankingApi.Domain.UseCases;

public interface IAccountUcReverseTransfer {
   Task<Result<Transfer>> ExecuteAsync(
      Guid accountId,
      Guid originalTransferId,
      string reason,
      CancellationToken ct = default
   );
}
