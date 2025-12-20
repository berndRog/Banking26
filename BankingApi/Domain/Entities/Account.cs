using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Entities;

public sealed class Account
{
   public Guid Id { get; }
   public Guid OwnerId { get; }
   public string Iban { get; }
   public decimal Balance { get; private set; }

   private Account(Guid id, Guid ownerId, string iban)
   {
      Id = id;
      OwnerId = ownerId;
      Iban = iban;
      Balance = 0m;
   }

   public static Result<Account> Create(Guid ownerId, string iban)
   {
      if (string.IsNullOrWhiteSpace(iban))
         return Result<Account>.Fail(AccountErrors.InvalidIban);

      return Result<Account>.Success(
         new Account(Guid.NewGuid(), ownerId, iban)
      );
   }
}
