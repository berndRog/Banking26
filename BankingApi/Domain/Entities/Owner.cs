using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Entities;

public sealed class Owner {

   // Properties
   public Guid Id { get; private set; }
   public string FirstName { get; private set; } = string.Empty;
   public string LastName  { get; private set; } = string.Empty;
   public string Email     { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;

   public string DisplayName =>
      CompanyName ?? $"{FirstName} {LastName}";

   private readonly List<Account> _accounts = new();
   public IReadOnlyCollection<Account> Accounts => _accounts.AsReadOnly();
   public bool HasAccounts() => _accounts.Count > 0;

   // ctor for EF Core
   private Owner() { } 

   // static factory method to create a private person owner
   public static Result<Owner> CreatePerson(
      string firstName,
      string lastName,
      string email
   ) {

      if (string.IsNullOrWhiteSpace(firstName))
         return Result<Owner>.Fail(OwnerErrors.InvalidFirstName);

      if (string.IsNullOrWhiteSpace(lastName))
         return Result<Owner>.Fail(OwnerErrors.InvalidLastName);

      if (!email.Contains('@'))
         return Result<Owner>.Fail(OwnerErrors.InvalidEmail);

      return Result<Owner>.Success(new Owner {
         Id        = Guid.NewGuid(),
         FirstName = firstName.Trim(),
         LastName  = lastName.Trim(),
         Email     = email.Trim()
      });
   }

   // static factory method to create a company owner
   public static Result<Owner> CreateCompany(
      string firstName,
      string lastName,
      string companyName,
      string email
   ) {

      if (string.IsNullOrWhiteSpace(firstName))
         return Result<Owner>.Fail(OwnerErrors.InvalidFirstName);

      if (string.IsNullOrWhiteSpace(lastName))
         return Result<Owner>.Fail(OwnerErrors.InvalidLastName);

      if (!email.Contains('@'))
         return Result<Owner>.Fail(OwnerErrors.InvalidEmail);

      if (string.IsNullOrWhiteSpace(companyName))
         return Result<Owner>.Fail(OwnerErrors.InvalidCompanyName);

      return Result<Owner>.Success(new Owner {
         Id          = Guid.NewGuid(),
         FirstName   = firstName.Trim(),
         LastName    = lastName.Trim(),
         Email       = email.Trim(),
         CompanyName = companyName.Trim()
      });
   }


   // Mutations
   public Result<Owner> ChangeEmail(string email) {

      if (!email.Contains('@'))
         return Result<Owner>.Fail(OwnerErrors.InvalidEmail);

      Email = email.Trim();
      return Result<Owner>.Success(this);
   }

   public Result<Owner> ChangeCompanyName(string companyName) {

      if (string.IsNullOrWhiteSpace(companyName))
         return Result<Owner>.Fail(OwnerErrors.InvalidCompanyName);

      CompanyName = companyName.Trim();
      return Result<Owner>.Success(this);
   }

   internal void AddAccount(Account account) =>
      _accounts.Add(account);
}