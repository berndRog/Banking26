namespace BankingApi.Domain.Errors;

public static class OwnerErrors {

   public static readonly DomainErrors InvalidFirstName =
      new("owner.invalid_first_name", "First name is required.");

   public static readonly DomainErrors InvalidLastName =
      new("owner.invalid_last_name", "Last name is required.");

   public static readonly DomainErrors InvalidEmail =
      new("owner.invalid_email", "Email is invalid.");

   public static readonly DomainErrors InvalidCompanyName =
      new("owner.invalid_company_name", "Company name is required.");

   public static readonly DomainErrors NotFound =
      new("owner.not_found", "Owner not found.");

   public static readonly DomainErrors HasAccounts =
      new("owner.has_accounts", "Owner cannot be deleted because accounts exist.");
}

