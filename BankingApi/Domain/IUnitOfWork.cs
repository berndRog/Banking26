namespace BankingApi.Domain;

public interface IUnitOfWork {
   Task SaveChangesAsync();
}