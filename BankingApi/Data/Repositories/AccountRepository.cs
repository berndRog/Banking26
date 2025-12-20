using BankingApi.Data.Database;
using BankingApi.Domain;
using BankingApi.Domain.Entities;
using BankingApi.Domain.Repositories;
using BankingApi.Domain.Utils;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Repositories;

public class AccountRepository(
   BankingDbContext dbContext,
   ILogger<AccountRepository> logger
) : IAccountRepository {
   
   private readonly ILogger<AccountRepository> _logger = logger;
   private readonly BankingDbContext _dbContext = dbContext;
   
   public Task AddAsync(Account account) {
      
      
      throw new NotImplementedException();
   }
}