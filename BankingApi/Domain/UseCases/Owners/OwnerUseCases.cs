namespace BankingApi.Domain.UseCases.Owners;

public class OwnerUseCases(
   IOwnerUcCreatePerson createPerson,
   IOwnerUcCreateCompany createCompany,
   IOwnerUcAddAccount addAccount,
   IOwnerUcUpdateEmail updateEmail,
   IOwnerUcRemove remove
): IOwnerUseCases {
   public IOwnerUcCreatePerson CreatePerson { get; } = createPerson;
   public IOwnerUcCreateCompany CreateCompany { get; } = createCompany;
   public IOwnerUcAddAccount AddAccount { get; } = addAccount;
   public IOwnerUcUpdateEmail UpdateEmail { get; } = updateEmail;
   public IOwnerUcRemove Remove { get; } = remove;
}