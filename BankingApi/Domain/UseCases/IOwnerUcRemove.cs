using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcRemove {
   Task<Result<Guid>> ExecuteAsync(
      Guid ownerId
   );
}

