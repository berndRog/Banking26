using BankingApi.Domain;
using BankingApi.Domain.Errors;
using BankingApi.Domain.UseCases;

public sealed class OwnerUcAddPrivateAddress(
   IOwnerRepository _ownerRepository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcAddPrivateAddress> _logger
): IOwnerUcAddPrivateAddress {
   
   public async Task<Result> ExecuteAsync(
      Guid ownerId,
      string street,
      string zipCode,
      string city,
      CancellationToken ct = default
   ) {
      _logger.LogInformation("Add private address started for OwnerId={Id}", 
         ownerId);

      // 1. Load aggregate root
      var owner = await _ownerRepository.FindByIdAsync(ownerId, ct);
      if (owner is null) {
         _logger.LogWarning("Add private address failed: Owner not found (OwnerId={Id})",
            ownerId);

         return Result.Failure(OwnerErrors.NotFound);
      }

      // 2. Execute domain operation on aggregate
      var result = owner.SetPrivateAddress(street, zipCode, city);
      if (result.IsFailure) {
         _logger.LogWarning("Add private address failed for OwnerId={Id}. Error={Error}",
            ownerId, result.Error);

         return result;
      }


      // 3. Persist changes
      await _unitOfWork.SaveAllChangesAsync("Add private address", ct);
      _logger.LogInformation("Private address added successful for OwnerId={Id}", ownerId);


      // 4. Return success
      return Result.Success();
   }
}

