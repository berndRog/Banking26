using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
namespace BankingApi.Domain.UseCases.Accounts;

public sealed class OwnerUcAddAccount(
   IOwnerRepository _ownerRepository,
   IAccountRepository _accountRepository,
   IUnitOfWork _unitOfWork,
   ILogger<OwnerUcAddAccount> _logger
) : IOwnerUcAddAccount {
   
   public async Task<Result<Account>> ExecuteAsync(
      Guid ownerId,
      string iban,
      CancellationToken ct = default
   ) {
      var owner = await _ownerRepository.FindByIdAsync(ownerId, ct);
      if (owner is null) {
         _logger.LogWarning("Add Account failed: owner not found ({Id})", ownerId.To8());
         return Result<Account>.Failure(OwnerErrors.NotFound);
      }
      
      // domain      
      var result = Account.Create(ownerId, iban);
      if (!result.IsSuccess)
         return result;
      owner.AddAccount(result.Value!);
      
      // repository
      _accountRepository.Add(result.Value!);
      // unit of work, save changes to database
      await _unitOfWork.SaveAllChangesAsync("Add account to owner", ct);

      _logger.LogDebug("Account created ({Id}) for Owner ({OwId})",
         result.Value!.Id.To8(), ownerId.To8());

      return result;
   }
}

