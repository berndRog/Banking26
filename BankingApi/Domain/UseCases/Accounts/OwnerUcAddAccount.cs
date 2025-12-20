using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
using Microsoft.Extensions.Logging;

namespace BankingApi.Domain.UseCases.Accounts;

public sealed class OwnerUcAddAccount(
   IOwnerRepository ownerRepository,
   IAccountRepository accountRepository,
   IUnitOfWork unitOfWork,
   ILogger<OwnerUcAddAccount> logger
) : IOwnerUcAddAccount {

   private readonly IOwnerRepository _ownerRepository = ownerRepository;
   private readonly IAccountRepository _accountRepository = accountRepository;
   private readonly IUnitOfWork _unitOfWork = unitOfWork;
   private readonly ILogger<OwnerUcAddAccount> _logger = logger;

   public async Task<Result<Entities.Account>> ExecuteAsync(
      Guid ownerId,
      string iban
   ) {
      
      var owner = await _ownerRepository.FindByIdAsync(ownerId);
      if (owner is null) {
         _logger.LogWarning("Add Account failed: owner not found ({Id})", ownerId.To8());
         return Result<Entities.Account>.Fail(AccountErrors.OwnerNotFound);
      }

      var result = Account.Create(ownerId, iban);
      if (!result.IsSuccess)
         return result;

      owner.AddAccount(result.Value!);

      await _accountRepository.AddAsync(result.Value!);
      await _unitOfWork.SaveChangesAsync();

      _logger.LogDebug("Account created ({AcId}) for Owner ({OwId})",
         result.Value!.Id.To8(), ownerId.To8());

      return result;
   }
}

