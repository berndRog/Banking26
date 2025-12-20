using BankingApi.Domain.Errors;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcRemove(
   IOwnerRepository repository,
   IUnitOfWork unitOfWork,
   ILogger<OwnerUcRemove> logger
) : IOwnerUcRemove {
   private readonly IOwnerRepository _repository = repository;
   private readonly IUnitOfWork _unitOfWork = unitOfWork;
   private readonly ILogger<OwnerUcRemove> _logger = logger;

   public async Task<Result<Guid>> ExecuteAsync(Guid ownerId) {
      var owner = await _repository.FindByIdAsync(ownerId);
      if (owner is null) {
         _logger.LogWarning("Delete Owner failed: not found ({Id})", ownerId.To8());
         return Result<Guid>.Fail(OwnerErrors.NotFound);
      }

      // if (owner.HasAccounts()) {
      //    _logger.LogWarning("Delete Owner failed: owner has accounts ({Id})",ownerId.To8());
      //    return Result<Guid>.Fail(OwnerErrors.HasAccounts);
      // }

      await _repository.RemoveAsync(owner);
      await _unitOfWork.SaveChangesAsync();
      _logger.LogDebug("Owner deleted successfully ({Id})", ownerId.To8());

      return Result<Guid>.Success(ownerId);
   }
}