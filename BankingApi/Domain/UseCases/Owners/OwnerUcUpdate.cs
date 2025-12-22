using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcUpdate(
   IOwnerRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcUpdate> _logger
) : IOwnerUcUpdate {

   public async Task<Result<Owner>> ExecuteAsync(
      Guid ownerId,
      string email
   ) {
      var owner = await _repository.FindByIdAsync(ownerId);
      if(owner is null) {
         _logger.LogWarning("Update Owner failed: Owner not found ({1})", ownerId);
         return Result<Owner>.Fail(OwnerErrors.NotFound);
      }

      var result = owner.ChangeEmail(email);
      if(!result.IsSuccess) {
         _logger.LogWarning("Update Owner failed: {1} ({2})", result.Error!.Code, ownerId);
         return result;
      }

      // One business transaction: track changes via repository, commit once.
      await _repository.UpdateAsync(owner);
      await _unitOfWork.SaveChangesAsync();

      _logger.LogDebug("Owner updated successfully: {OwnerId}", ownerId);

      return result;
   }
}
