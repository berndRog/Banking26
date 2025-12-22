using BankingApi.Domain.Entities;
using BankingApi.Domain.Errors;
using BankingApi.Domain.Repositories;
namespace BankingApi.Domain.UseCases.Accounts;

public sealed class AccountUcReverseTransfer(
   ITransferRepository transferRepository,
   IAccountRepository accountRepository,
   ITransactionRepository transactionRepository,
   IUnitOfWork unitOfWork
) : IAccountUcReverseTransfer {

   private readonly ITransferRepository _transferRepository = transferRepository;
   private readonly IAccountRepository _accountRepository = accountRepository;
   private readonly ITransactionRepository _transactionRepository = transactionRepository;
   private readonly IUnitOfWork _unitOfWork = unitOfWork;

   public async Task<Result<Transfer>> ExecuteAsync(
      Guid accountId,
      Guid originalTransferId,
      string reason
   ) {

      // 1️ Originaltransfer laden
      var original = await _transferRepository.FindByIdAsync(originalTransferId);
      if (original is null)
         return Result<Transfer>.Fail(TransferErrors.NotFound);

      // 2 Sicherheits-/Ownership-Check
      if (original.FromAccountId != accountId)
         return Result<Transfer>.Fail(DomainErrors.Forbidden);

      // 3 Bereits storniert?
      if (await _transferRepository.ExistsReversalForAsync(original.Id))
         return Result<Transfer>.Fail(TransferErrors.AlreadyReversed);

      // 4️⃣ Konten laden
      var sender = await _accountRepository.FindByIdAsync(original.FromAccountId);
      var receiver = await _accountRepository.FindByIdAsync(original.ToAccountId);

      if (sender is null || receiver is null)
         return Result<Transfer>.Fail(DomainErrors.NotFound);

      // 5️⃣ Salden prüfen & ändern (Domain!)
      var withdraw = receiver.Withdraw(original.Amount);
      if (!withdraw.IsSuccess)
         return Result<Transfer>.Fail(TransferErrors.InsufficientFunds);

      sender.Deposit(original.Amount);

      // 6️⃣ Neuer Transfer (Storno!)
      var reversal = new Transfer(
         fromAccountId: receiver.Id,
         toAccountId: sender.Id,
         amount: original.Amount,
         purpose: $"REVERSAL: {reason}",
         reversalOfTransferId: original.Id
      );

      await _transferRepository.AddAsync(reversal);

      // 7 Zwei neue Buchungen
      await _transactionRepository.AddAsync(
         new Transaction(receiver.Id, reversal.Id, -original.Amount, reversal.Purpose)
      );
      await _transactionRepository.AddAsync(
         new Transaction(sender.Id, reversal.Id, original.Amount, reversal.Purpose)
      );

      await _unitOfWork.SaveChangesAsync();

      return Result<Transfer>.Success(reversal);
   }
}
