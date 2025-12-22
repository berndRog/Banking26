namespace BankingApi.Domain.UseCases.Owners;

public class AccountUseCases(
   IAccountUcAddBeneficiary addBeneficiary,
   IAccountUcRemoveBeneficiary removeBeneficiary,
   IAccountUcExecuteTransfer executeTransfer,
   IAccountUcReverseTransfer reverseTransfer,
   IAccountUcGetTransactions getTransactions
) : IAccountUseCases {
   public IAccountUcAddBeneficiary AddBeneficiary { get; } = addBeneficiary;
   public IAccountUcRemoveBeneficiary RemoveBeneficiary { get; } = removeBeneficiary;
   public IAccountUcExecuteTransfer ExecuteTransfer { get; } = executeTransfer;
   public IAccountUcReverseTransfer ReverseTransfer { get; } = reverseTransfer;
   public IAccountUcGetTransactions GetTransactions { get; } = getTransactions;
}