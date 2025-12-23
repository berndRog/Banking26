using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcUpdateEmail(
   IOwnerRepository _ownerRepository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcUpdateEmail> _logger
) : IOwnerUcUpdateEmail {
   
   public async Task<Result<Owner>> ExecuteAsync(
      Guid ownerId,
      string newEmail,
      CancellationToken ct = default
   ) {
      var owner = await _ownerRepository.FindByIdAsync(ownerId, ct);
      if (owner is null) {
         _logger.LogWarning("UpdateEmail email failed: owner not found ({Id})", ownerId.To8());
         return Result<Owner>.Failure(OwnerErrors.NotFound);
      }

      var change = owner.ChangeEmail(newEmail);
      if (!change.IsSuccess) {
         _logger.LogWarning("UpdateEmail email failed: {Err}", change.Error!.Code);
         return Result<Owner>.Failure(change.Error);
      }

      await _unitOfWork.SaveAllChangesAsync("Email changes",ct);

      _logger.LogDebug("Owner email updated ({Id})", ownerId.To8());
      return Result<Owner>.Success(owner);
   }

}