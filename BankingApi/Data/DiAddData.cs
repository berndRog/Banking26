using BankingApi.Data.Database;
using BankingApi.Data.Repositories;
using BankingApi.Domain;
using BankingApi.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
namespace BankingApi.Data.Extensions;

public static class DiAddDataExtensions {
   public static IServiceCollection AddData(
      this IServiceCollection services,
      IConfiguration configuration
   ) {
      
      services.AddDbContext<BankingDbContext>(options =>
         options.UseSqlite(
            configuration.GetConnectionString("BankingDb"))
      );

      // Unit of Work
      services.AddScoped<IUnitOfWork, UnitOfWork>();

      // Repositories
      services.AddScoped<IOwnerRepository, OwnerRepository>();
      services.AddScoped<IAccountRepository, AccountRepository>();

      return services;
   }
}