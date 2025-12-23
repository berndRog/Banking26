using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcAddAccount {
   Task<Result<Account>> ExecuteAsync(
      Guid ownerId,
      string iban,
      CancellationToken ct = default
   );
}