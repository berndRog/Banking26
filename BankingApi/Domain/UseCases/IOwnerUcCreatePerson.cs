using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcCreatePerson {
   Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string email
   );
}
