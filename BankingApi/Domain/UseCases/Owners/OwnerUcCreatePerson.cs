using BankingApi.Domain.Entities;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcCreatePerson(
   IOwnerRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcCreatePerson> _logger
) : IOwnerUcCreatePerson {

   public async Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string email
   ) {
      var result = Owner.CreatePerson(firstName, lastName, email);
      if (!result.IsSuccess) {
         _logger.LogWarning("Create Owner (Person) failed: {Err}", result.Error!.Code);
         return result;
      }

      // The UseCase represents one business transaction:
      // added changes are tracked via repositories and committed once - as unit of Work
      await _repository.AddAsync(result.Value!);
      // save all changes to database using a transaction
      await _unitOfWork.SaveChangesAsync();

      _logger.LogInformation("Owner (Person) created successfully: {Id}", result.Value!.Id.To8());

      return result;
   }
}