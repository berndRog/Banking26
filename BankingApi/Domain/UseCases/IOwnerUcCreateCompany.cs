using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcCreateCompany {
   Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string companyName,
      string email,
      CancellationToken ct = default
   );
}
