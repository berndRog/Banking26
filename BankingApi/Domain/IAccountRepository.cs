using BankingApi.Domain.Entities;
namespace BankingApi.Domain.Repositories;

public interface IAccountRepository {
   
   Task<Account?> FindByIdAsync(Guid accountId);
   Task<Account?> FindByIbanAsync(string iban);
   Task AddAsync(Account account);
   Task UpdateAsync(Account account);
}