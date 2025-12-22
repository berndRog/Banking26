using BankingApi.Domain.UseCases;
namespace BankingApi.Domain;

public interface IAccountUseCases {
   IAccountUcAddBeneficiary  AddBeneficiary  { get; }
   IAccountUcRemoveBeneficiary RemoveBeneficiary { get; }
   IAccountUcExecuteTransfer ExecuteTransfer { get; }
   IAccountUcReverseTransfer ReverseTransfer { get; }
   IAccountUcGetTransactions GetTransactions { get; }
}