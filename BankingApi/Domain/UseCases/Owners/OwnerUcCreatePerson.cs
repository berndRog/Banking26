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
      string email,
      CancellationToken ct = default
   ) {
      var owner = Owner.CreatePerson(firstName, lastName, email);
      if (!owner.IsSuccess) {
         _logger.LogWarning("Create Owner(Person) failed: {Err}", owner.Error!.Code);
         return owner;
      }

      // The UseCase represents one business transaction:
      // added changes are tracked via repositories and committed once - as unit of Work
      _repository.Add(owner.Value!);
      // save all changes to database using a transaction
      await _unitOfWork.SaveAllChangesAsync("Create Owner(Person)", ct);

      _logger.LogInformation("Owner(Person) created successfully: {Id}", owner.Value!.Id.To8());

      return owner;
   }
}