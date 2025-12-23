using BankingApi.Data.Database;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Repositories;

public sealed class AccountRepository(
   BankingDbContext _dbContext,
   ILogger<AccountRepository> _logger
) : IAccountRepository {

   
   public async Task<Account?> FindByIdAsync(
      Guid accountId,
      CancellationToken ct = default
   ){
      _logger.LogDebug("Load Account by Id ({Id})", accountId.To8());
      var account = await _dbContext.Accounts
         .FirstOrDefaultAsync(a => a.Id == accountId, ct);
      if (account is not null) return account;
      
      _logger.LogDebug("Account not found ({Id})", accountId.To8());
      return null;
   }
   
   public async Task<Account?> FindByIbanAsync(
      string iban,
      CancellationToken ct = default
   ) {
      _logger.LogDebug("Loading account by IBAN ({Iban})", iban);
      
      return await _dbContext.Accounts
         .FirstOrDefaultAsync(a => a.Iban == iban, ct);
   }
   
   
   public void Add(Account account) {
      _logger.LogDebug("Add Account ({Id}, {Iban})", account.Id.To8(), account.Iban);
      _dbContext.Accounts.Add(account);
   }
}