using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Entities;

public sealed class Beneficiary {

   // Properties
   public Guid Id { get; private set; }
   public string FirstName { get; private set; } = string.Empty;
   public string LastName  { get; private set; } = string.Empty;
   public string? CompanyName { get; private set; } = null;
   public string Iban      { get; private set; } = string.Empty;

   public Guid AccountId { get; private set; }
   
   public string DisplayName =>
      CompanyName ?? $"{FirstName} {LastName}";
   
   
   // ctor for EF Core
   private Beneficiary() { }

   // static factory method to create a beneficiary
   public static Result<Beneficiary> Create(
      Guid accountId,
      string firstName,
      string lastName,
      string? companyName, 
      string iban
   ) {

      if (string.IsNullOrWhiteSpace(firstName))
         return Result<Beneficiary>.Failure(BeneficiaryErrors.InvalidFirstName);

      if (string.IsNullOrWhiteSpace(lastName))
         return Result<Beneficiary>.Failure(BeneficiaryErrors.InvalidLastName);

      if( companyName != null && string.IsNullOrWhiteSpace(companyName))
         return Result<Beneficiary>.Failure(BeneficiaryErrors.InvalidCompanyName);

      if (string.IsNullOrWhiteSpace(iban))
         return Result<Beneficiary>.Failure(BeneficiaryErrors.InvalidIban);

      return Result<Beneficiary>.Success(new Beneficiary {
         Id        = Guid.NewGuid(),
         AccountId =  accountId,
         FirstName = firstName.Trim(),
         LastName  = lastName.Trim(),
         CompanyName = companyName?.Trim(),
         Iban      = iban.Trim()
      });
   }
}
