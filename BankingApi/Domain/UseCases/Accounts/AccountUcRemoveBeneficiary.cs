using BankingApi.Domain.Errors;
using BankingApi.Domain.Utils;

namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcRemoveBeneficiary(
   IBeneficiaryRepository _beneficiaryRepository,
   IUnitOfWork _unitOfWork,
   ILogger<AccountUcRemoveBeneficiary> _logger
) : IAccountUcRemoveBeneficiary {

   public async Task<Result<Guid>> ExecuteAsync(Guid beneficiaryId) {
      var beneficiary = await _beneficiaryRepository.FindByIdAsync(beneficiaryId);
      if (beneficiary is null) {
         _logger.LogWarning("Remove Beneficiary failed: not found ({Id})", beneficiaryId.To8());
         return Result<Guid>.Fail(BeneficiaryErrors.NotFound);
      }

      await _beneficiaryRepository.RemoveAsync(beneficiary);
      await _unitOfWork.SaveChangesAsync();

      _logger.LogInformation("Beneficiary removed ({Id})", beneficiaryId.To8());

      return Result<Guid>.Success(beneficiaryId);
   }
}
