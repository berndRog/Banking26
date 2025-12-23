using BankingApi.Domain.Errors;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcRemove(
   IOwnerRepository _repository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcRemove> _logger
) : IOwnerUcRemove {

   public async Task<Result<Guid>> ExecuteAsync(
      Guid ownerId,
      CancellationToken ct = default
   ){
      var owner = await _repository.FindByIdAsync(ownerId, ct);
      if (owner is null) {
         _logger.LogWarning("Delete Owner failed: not found ({Id})", ownerId.To8());
         return Result<Guid>.Fail(OwnerErrors.NotFound);
      }

      if (owner.HasAccounts()) {
          _logger.LogWarning("Delete Owner failed: owner has accounts ({Id})",ownerId.To8());
          return Result<Guid>.Fail(OwnerErrors.HasAccounts);
      }

      _repository.Remove(owner);
      await _unitOfWork.SaveAllChangesAsync("Delete Owner", ct);
      _logger.LogDebug("Owner deleted successfully ({Id})", ownerId.To8());

      return Result<Guid>.Success(ownerId);
   }
}