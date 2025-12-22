using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcAddBeneficiary(
   IAccountRepository _accountRepository,
   IBeneficiaryRepository _beneficiaryRepository,
   IUnitOfWork _unitOfWork,
   ILogger<AccountUcAddBeneficiary> _logger
) : IAccountUcAddBeneficiary {
   
   public async Task<Result<Beneficiary>> ExecuteAsync(
      Guid accountId,
      string firstName,
      string lastName,
      string companyName,
      string iban
   ) {

      var account = await _accountRepository.FindByIdAsync(accountId);
      if (account is null) {
         _logger.LogWarning("Add Beneficiary failed: account not found ({Id})", accountId.To8());
         return Result<Beneficiary>.Fail(BeneficiaryErrors.AccountNotFound);
      }

      var result = Beneficiary.Create(accountId, firstName, lastName, companyName, iban);
      if (!result.IsSuccess) {
         _logger.LogWarning("Add Beneficiary failed: {Err}", result.Error!.Code);
         return result;
      }

      await _beneficiaryRepository.AddAsync(result.Value!);
      await _unitOfWork.SaveChangesAsync();

      _logger.LogDebug("Beneficiary added ({Id}) to Account ({AccountId})",
         result.Value!.Id.To8(), accountId.To8());

      return result;
   }
}
