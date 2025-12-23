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
      string purpose,
      CancellationToken ct = default
   ) {
      if (amount <= 0m)
         return Result<Transfer>.Failure(TransferErrors.InvalidAmount);

      // 1) Load sender account
      var fromAccount = await _accountRepository.FindByIdAsync(fromAccountId, ct);
      if (fromAccount is null)
         return Result<Transfer>.Failure(TransferErrors.AccountNotFound);

      // 2) Load beneficiary (must belong to sender account)
      var beneficiary = await _beneficiaryRepository.FindByIdAsync(beneficiaryId, ct);
      if (beneficiary is null || beneficiary.AccountId != fromAccountId)
         return Result<Transfer>.Failure(TransferErrors.BeneficiaryNotFound);

      // 3) Load receiver account via IBAN
      var toAccount = await _accountRepository.FindByIbanAsync(beneficiary.Iban, ct);
      if (toAccount is null)
         return Result<Transfer>.Failure(TransferErrors.AccountNotFound);

      if (toAccount.Id == fromAccount.Id)
         return Result<Transfer>.Failure(TransferErrors.SameAccount);

      // 4) Create transfer
      // unique timestamp for transfer and transactions
      var dtOffsetNow = DateTimeOffset.UtcNow;
      var transfer = new Transfer(
         fromAccount.Id,
         toAccount.Id,
         dtOffsetNow,
         amount,
         purpose
      );

      // 5) Withdraw sender
      var withdrawResult = fromAccount.Withdraw(amount);
      if (!withdrawResult.IsSuccess)
         return Result<Transfer>.Failure(withdrawResult.Error!);

      // 6) Deposit receiver
      var depositResult = toAccount.Deposit(amount);
      if (!depositResult.IsSuccess)
         return Result<Transfer>.Failure(depositResult.Error!);

      // 7) Create transactions
      // debit from sender (Lastschrift)
      var debit = new Transaction(
         fromAccount.Id,
         transfer.Id,
         dtOffsetNow,
         -amount,
         purpose
      );
      // credit to receiver (Gutschrift)
      var credit = new Transaction(
         toAccount.Id,
         transfer.Id,
         dtOffsetNow,
         amount,
         purpose
      );

      // 8) add changes to repositiroes
      _transferRepository.Add(transfer);
      _transactionRepository.Add(debit);
      _transactionRepository.Add(credit);

      await _unitOfWork.SaveAllChangesAsync("Execute transfer", ct);

      _logger.LogDebug(
         "Transfer executed ({Id}) {From} -> {To}, Amount={Amount}",
         transfer.Id.To8(), fromAccount.Id.To8(), toAccount.Id.To8(), amount);

      return Result<Transfer>.Success(transfer);
   }
}
