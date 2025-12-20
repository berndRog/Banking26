using BankingApi.Domain.UseCases;
using BankingApi.Domain.UseCases.Accounts;
using BankingApi.Domain.UseCases.Owners;
namespace BankingApi.Domain.Extensions;

public static class DiAddDomainExtensions {
   
   public static IServiceCollection AddDomain(
      this IServiceCollection services
   ) {
      // ----------------------------
      // Owner UseCases
      // ----------------------------
      services.AddScoped<IOwnerUcCreate, OwnerUcCreate>();
      services.AddScoped<IOwnerUcUpdate, OwnerUcUpdate>();
      services.AddScoped<IOwnerUcRemove, OwnerUcRemove>();
      services.AddScoped<IOwnerUseCases, OwnerUseCases>();

      // ----------------------------
      // Account UseCases
      // ----------------------------
      services.AddScoped<IOwnerUcAddAccount, OwnerUcAddAccount>();

      return services;
   }
}