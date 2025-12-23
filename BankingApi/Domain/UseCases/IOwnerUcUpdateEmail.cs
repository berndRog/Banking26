using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcUpdateEmail {
   Task<Result<Owner>> ExecuteAsync(
      Guid ownerId,
      string email,
      CancellationToken ct = default
   );
}

