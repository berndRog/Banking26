using BankingApi.Domain;
using BankingApi.Domain.Errors;

public sealed class OwnerUcAddCompanyAddress(
   IOwnerRepository _ownerRepository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcAddCompanyAddress> _logger,
   CancellationToken ct = default
) {
   public async Task<Result> ExecuteAsync(
      Guid ownerId,
      string street,
      string zipCode,
      string city
   ) {
      _logger.LogInformation(
         "Add company address started for OwnerId={OwnerId}", ownerId);

      // 1. Load aggregate root
      var owner = await _ownerRepository.FindByIdAsync(ownerId, ct);
      if (owner is null) {
         _logger.LogWarning("Add company address failed: Owner not found (OwnerId={Id})",
            ownerId);

         return Result.Failure(OwnerErrors.NotFound);
      }

      // 2. Execute domain operation
      var result = owner.SetCompanyAddress(street, zipCode, city);
      if (result.IsFailure) {
         _logger.LogWarning("Add company address failed for OwnerId={Id}. Error={Error}",
            ownerId, result.Error);

         return result;
      }

      // 3. Persist changes
      await _unitOfWork.SaveAllChangesAsync("Add company address", ct);

      _logger.LogInformation("Add company address successful for OwnerId={Id}",
         ownerId);

      return Result.Success();
   }
}