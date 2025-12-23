using BankingApi.Domain.Errors;
using BankingApi.Domain.Utils;

namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcRemoveBeneficiary(
   IBeneficiaryRepository _beneficiaryRepository,
   IUnitOfWork _unitOfWork,
   ILogger<AccountUcRemoveBeneficiary> _logger
) : IAccountUcRemoveBeneficiary {

   public async Task<Result<Guid>> ExecuteAsync(
      Guid beneficiaryId,
      CancellationToken ct = default
   ) {
      var beneficiary = await _beneficiaryRepository.FindByIdAsync(beneficiaryId, ct);
      if (beneficiary is null) {
         _logger.LogWarning("Remove Beneficiary failed: not found ({Id})", beneficiaryId.To8());
         return Result<Guid>.Failure(BeneficiaryErrors.NotFound);
      }

      _beneficiaryRepository.Remove(beneficiary);
      await _unitOfWork.SaveAllChangesAsync("Remove beneficiary", ct);

      _logger.LogInformation("Beneficiary removed ({Id})", beneficiaryId.To8());
      return Result<Guid>.Success(beneficiaryId);
   }
}
