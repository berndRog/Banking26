using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcUpdate {
   Task<Result<Owner>> ExecuteAsync(
      Guid ownerId,
      string email
   );
}

