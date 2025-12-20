namespace BankingApi.Domain.UseCases;

public interface IOwnerUseCases {
   IOwnerUcCreate Create { get; }
   IOwnerUcUpdate Update { get; }
   IOwnerUcRemove Remove { get; }
}