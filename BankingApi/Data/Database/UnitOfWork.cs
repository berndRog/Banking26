using BankingApi.Domain;
namespace BankingApi.Data.Database;


public sealed class UnitOfWork(
   BankingDbContext context
) : IUnitOfWork {

   public Task SaveChangesAsync() =>
      context.SaveChangesAsync();
}
