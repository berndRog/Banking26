using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcCreate {
   Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string email
   );
}
