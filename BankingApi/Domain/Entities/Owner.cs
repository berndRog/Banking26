using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Entities;

using BankingApi.Domain;

public sealed class Owner {

   // Owner Properties
   public Guid Id { get; }
   public string FirstName { get; }
   public string LastName { get; }
   public string Email { get; }
   // Owner <-> Account [1] : [0..*]
   private readonly List<Account> _accounts = new();
   public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
   public bool HasAccounts() => _accounts.Count > 0;

   #region Owner 
   private Owner(Guid id, string firstName, string lastName, string email) {
      Id = id;
      FirstName = firstName;
      LastName = lastName;
      Email = email;
   }

   public static Result<Owner> Create(
      string firstName,
      string lastName,
      string email
   ) {

      if(string.IsNullOrWhiteSpace(firstName))
         return Result<Owner>.Fail(OwnerErrors.InvalidFirstName);

      if(string.IsNullOrWhiteSpace(lastName))
         return Result<Owner>.Fail(OwnerErrors.InvalidLastName);

      if(!email.Contains('@'))
         return Result<Owner>.Fail(OwnerErrors.InvalidEmail);

      return Result<Owner>.Success(
         new Owner(Guid.NewGuid(), firstName, lastName, email)
      );
   }

   public Result<Owner> ChangeEmail(string email) {
      if(!email.Contains('@'))
         return Result<Owner>.Fail(OwnerErrors.InvalidEmail);

      return Result<Owner>.Success(
         new Owner(Id, FirstName, LastName, email)
      );
   }
   #endregion Owner
   
   
   internal void AddAccount(Account account) =>
      _accounts.Add(account);

}

