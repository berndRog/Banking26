using BankingApi.Domain.Entities;
namespace BankingApi.Domain.Repositories;

public interface IAccountRepository {
   Task<Account?> FindByIdAsync(Guid accountId, CancellationToken ct = default);
   Task<Account?> FindByIbanAsync(string iban, CancellationToken ct = default);
   void Add(Account account);
}