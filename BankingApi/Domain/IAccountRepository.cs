using BankingApi.Domain.Entities;
namespace BankingApi.Domain.Repositories;

public interface IAccountRepository {
   Task AddAsync(Account account);
}