namespace BankingApi.Domain.Errors;

public static class BeneficiaryErrors {

   public static readonly DomainErrors InvalidBeneficiaryId =
      new("beneficiary.invalid_id", "BeneficiaryId must not be empty.");

   public static readonly DomainErrors InvalidFirstName =
      new("beneficiary.invalid_first_name", "First name must not be empty.");

   public static readonly DomainErrors InvalidLastName =
      new("beneficiary.invalid_last_name", "Last name must not be empty.");

   public static readonly DomainErrors InvalidCompanyName =
      new("beneficiary.invalid_company_name", "Company name must not be empty.");

   public static readonly DomainErrors InvalidIban =
      new("beneficiary.invalid_iban", "IBAN must not be empty.");
   
   public static readonly DomainErrors NotFound =
      new("beneficiary.not_found", "Beneficiary not found.");
   
   public static readonly DomainErrors AccountNotFound =
      new("account.not.found", "Account not found.");
}

