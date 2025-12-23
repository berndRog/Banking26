using BankingApi.Domain.Entities;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcCreateCompany(
   IOwnerRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcCreateCompany> _logger
) : IOwnerUcCreateCompany {

   public async Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string companyName,
      string email,
      CancellationToken ct = default
   ) {
      var owner = Owner.CreateCompany(firstName, lastName, companyName, email);
      if (!owner.IsSuccess) {
         _logger.LogWarning("Create Owner (Company) failed: {Err}", owner.Error!.Code);
         return owner;
      }

      // The UseCase represents one business transaction:
      // added changes are tracked via repositories and committed once - as unit of Work
      _repository.Add(owner.Value!);
      // save all changes to database using a transaction
      await _unitOfWork.SaveAllChangesAsync("Create Owner(Company)",ct);

      _logger.LogInformation("Owner (Company) created successfully: {Id}", owner.Value!.Id.To8());

      return owner;
   }
}