using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IAccountUcAddBeneficiary {
   Task<Result<Beneficiary>> ExecuteAsync(
      Guid accountId,
      string firstName,
      string lastName,
      string companyName,
      string iban,
      CancellationToken ct = default
   );
}