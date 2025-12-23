using BankingApi.Domain.Errors;
using BankingApi.Domain.ValueObjects;
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

   // Value Objects (0..1), embedded in Owner
   public Address? PrivateAddress { get; private set; }
   public Address? CompanyAddress { get; private set; }

   // Relation Owner -> Accounts [1] : [0..*]
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
         return Result<Owner>.Failure(OwnerErrors.InvalidFirstName);

      if (string.IsNullOrWhiteSpace(lastName))
         return Result<Owner>.Failure(OwnerErrors.InvalidLastName);

      if (!email.Contains('@'))
         return Result<Owner>.Failure(OwnerErrors.InvalidEmail);

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
         return Result<Owner>.Failure(OwnerErrors.InvalidFirstName);

      if (string.IsNullOrWhiteSpace(lastName))
         return Result<Owner>.Failure(OwnerErrors.InvalidLastName);

      if (!email.Contains('@'))
         return Result<Owner>.Failure(OwnerErrors.InvalidEmail);

      if (string.IsNullOrWhiteSpace(companyName))
         return Result<Owner>.Failure(OwnerErrors.InvalidCompanyName);

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
         return Result<Owner>.Failure(OwnerErrors.InvalidEmail);

      Email = email.Trim();
      return Result<Owner>.Success(this);
   }

   // Use Case operation: Add an account to the owner
   public void AddAccount(Account account) =>
      _accounts.Add(account);
   
   // Use Case operation: Set private address
   public Result SetPrivateAddress(string street, string zipCode, string city) {
      var addressResult = Address.Create(street, zipCode, city);
      if (addressResult.IsFailure)
         return Result.Failure(addressResult.Error!);

      PrivateAddress = addressResult.Value!;
      return Result.Success();
   }

   public Result SetCompanyAddress(string street, string zipCode, string city) {
      var addressResult = Address.Create(street, zipCode, city);
      if (addressResult.IsFailure)
         return Result.Failure(addressResult.Error!);

      CompanyAddress = addressResult.Value!;
      return Result.Success();
   }

}
