namespace BankingApi.Domain.UseCases;

public interface IOwnerUseCases {
   IOwnerUcCreatePerson CreatePerson { get; }
   IOwnerUcCreateCompany CreateCompany { get; }
   IOwnerUcAddAccount AddAccount { get; }
   IOwnerUcUpdateEmail UpdateEmail { get; }
   IOwnerUcRemove Remove { get; }
}