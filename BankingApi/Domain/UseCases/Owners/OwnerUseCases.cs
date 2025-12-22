namespace BankingApi.Domain.UseCases.Owners;

public class OwnerUseCases(
   IOwnerUcCreatePerson createPerson,
   IOwnerUcCreateCompany createCompany,
   IOwnerUcAddAccount addAccount,
   IOwnerUcUpdate update,
   IOwnerUcRemove remove
): IOwnerUseCases {
   public IOwnerUcCreatePerson CreatePerson { get; } = createPerson;
   public IOwnerUcCreateCompany CreateCompany { get; } = createCompany;
   public IOwnerUcAddAccount AddAccount { get; } = addAccount;
   public IOwnerUcUpdate Update { get; } = update;
   public IOwnerUcRemove Remove { get; } = remove;
}