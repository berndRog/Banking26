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
      string email
   ) {
      var result = Owner.CreateCompany(firstName, lastName, companyName, email);
      if (!result.IsSuccess) {
         _logger.LogWarning("Create Owner (Company) failed: {Err}", result.Error!.Code);
         return result;
      }

      // The UseCase represents one business transaction:
      // added changes are tracked via repositories and committed once - as unit of Work
      await _repository.AddAsync(result.Value!);
      // save all changes to database using a transaction
      await _unitOfWork.SaveChangesAsync();

      _logger.LogInformation("Owner (Company) created successfully: {Id}", result.Value!.Id.To8());

      return result;
   }
}