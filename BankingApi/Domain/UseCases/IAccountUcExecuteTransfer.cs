using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IAccountUcExecuteTransfer {
   Task<Result<Transfer>> ExecuteAsync(
      Guid fromAccountId,
      Guid beneficiaryId,
      decimal amount,
      string purpose,
      CancellationToken ctToken = default
   );
}