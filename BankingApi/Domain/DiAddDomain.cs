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
      services.AddScoped<IOwnerUcCreatePerson, OwnerUcCreatePerson>();
      services.AddScoped<IOwnerUcCreateCompany, OwnerUcCreateCompany>();
      services.AddScoped<IOwnerUcUpdateEmail, OwnerUcUpdateEmail>();
      services.AddScoped<IOwnerUcRemove, OwnerUcRemove>();
      services.AddScoped<IOwnerUcAddAccount, OwnerUcAddAccount>();
      services.AddScoped<IOwnerUseCases, OwnerUseCases>();

      // ----------------------------
      // Account UseCases
      // ----------------------------
      
      services.AddScoped<IAccountUcGetTransactions, AccountUcGetTransactions>();
      services.AddScoped<IAccountUcAddBeneficiary, AccountUcAddBeneficiary>();
      services.AddScoped<IAccountUcRemoveBeneficiary, AccountUcRemoveBeneficiary>();
      services.AddScoped<IAccountUcExecuteTransfer, AccountUcExecuteTransfer>();
      services.AddScoped<IAccountUcReverseTransfer, AccountUcReverseTransfer>();

      services.AddScoped<IAccountUseCases, AccountUseCases>();

      return services;
   }
}