using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApi.Domain.UseCases.Accounts;

public sealed class OwnerUcAddAccount(
   IOwnerRepository _ownerRepository,
   IAccountRepository _accountRepository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcAddAccount> _logger
) : IOwnerUcAddAccount {
   
   public async Task<Result<Entities.Account>> ExecuteAsync(
      Guid ownerId,
      string iban
   ) {
      
      var owner = await _ownerRepository.FindByIdAsync(ownerId);
      if (owner is null) {
         _logger.LogWarning("Add Account failed: owner not found ({Id})", ownerId.To8());
         return Result<Account>.Fail(OwnerErrors.NotFound);
      }
      
      var result = Account.Create(ownerId, iban);
      if (!result.IsSuccess)
         return result;

      owner.AddAccount(result.Value!);

      await _accountRepository.AddAsync(result.Value!);
      await _unitOfWork.SaveChangesAsync();

      _logger.LogDebug("Account created ({Id}) for Owner ({OwId})",
         result.Value!.Id.To8(), ownerId.To8());

      return result;
   }
}

