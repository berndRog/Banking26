using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;

namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcExecuteTransfer(
   IAccountRepository _accountRepository,
   IBeneficiaryRepository _beneficiaryRepository,
   ITransferRepository _transferRepository,
   ITransactionRepository _transactionRepository,
   IUnitOfWork _unitOfWork,
   ILogger<AccountUcExecuteTransfer> _logger
) : IAccountUcExecuteTransfer {
   
   public async Task<Result<Transfer>> ExecuteAsync(
      Guid fromAccountId,
      Guid beneficiaryId,
      decimal amount,
      string purpose
   ) {
      if (amount <= 0m)
         return Result<Transfer>.Fail(TransferErrors.InvalidAmount);

      // 1) Load sender account
      var fromAccount = await _accountRepository.FindByIdAsync(fromAccountId);
      if (fromAccount is null)
         return Result<Transfer>.Fail(TransferErrors.AccountNotFound);

      // 2) Load beneficiary (must belong to sender account)
      var beneficiary = await _beneficiaryRepository.FindByIdAsync(beneficiaryId);
      if (beneficiary is null || beneficiary.AccountId != fromAccountId)
         return Result<Transfer>.Fail(TransferErrors.BeneficiaryNotFound);

      // 3) Load receiver account via IBAN
      var toAccount = await _accountRepository.FindByIbanAsync(beneficiary.Iban);
      if (toAccount is null)
         return Result<Transfer>.Fail(TransferErrors.AccountNotFound);

      if (toAccount.Id == fromAccount.Id)
         return Result<Transfer>.Fail(TransferErrors.SameAccount);

      // 4) Create transfer
      var transfer = new Transfer(
         fromAccount.Id,
         toAccount.Id,
         amount,
         purpose
      );

      // 5) Withdraw sender
      var withdrawResult = fromAccount.Withdraw(amount);
      if (!withdrawResult.IsSuccess)
         return Result<Transfer>.Fail(withdrawResult.Error!);

      // 6) Deposit receiver
      var depositResult = toAccount.Deposit(amount);
      if (!depositResult.IsSuccess)
         return Result<Transfer>.Fail(depositResult.Error!);

      // 7) Create transactions
      var debit = new Transaction(
         fromAccount.Id,
         transfer.Id,
         -amount,
         purpose
      );

      var credit = new Transaction(
         toAccount.Id,
         transfer.Id,
         amount,
         purpose
      );

      // 8) Persist everything atomically
      await _transferRepository.AddAsync(transfer);
      await _transactionRepository.AddAsync(debit);
      await _transactionRepository.AddAsync(credit);

      await _unitOfWork.SaveChangesAsync();

      _logger.LogDebug(
         "Transfer executed ({Id}) {From} -> {To}, Amount={Amount}",
         transfer.Id.To8(), fromAccount.Id.To8(), toAccount.Id.To8(), amount);

      return Result<Transfer>.Success(transfer);
   }
}
