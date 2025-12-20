namespace BankingApi.Domain.UseCases.Owners;

public class OwnerUseCases(
   IOwnerUcCreate create,
   IOwnerUcUpdate update,
   IOwnerUcRemove remove
): IOwnerUseCases {
   public IOwnerUcCreate Create { get; } = create;
   public IOwnerUcUpdate Update { get; } = update;
   public IOwnerUcRemove Remove { get; } = remove;
}