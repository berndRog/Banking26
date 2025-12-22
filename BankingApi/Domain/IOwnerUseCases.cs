namespace BankingApi.Domain.UseCases;

public interface IOwnerUseCases {
   IOwnerUcCreatePerson CreatePerson { get; }
   IOwnerUcCreateCompany CreateCompany { get; }
   IOwnerUcAddAccount AddAccount { get; }
   IOwnerUcUpdate Update { get; }
   IOwnerUcRemove Remove { get; }
}