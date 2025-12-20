namespace BankingApi.Domain.Errors;

public static class OwnerErrors {

   public static readonly DomainErrors NotFound =
      new("owner.not_found", "Owner not found.");

   public static readonly DomainErrors InvalidFirstName =
      new("owner.invalid_first_name", "First name is invalid.");

   public static readonly DomainErrors InvalidLastName =
      new("owner.invalid_last_name", "Last name is invalid.");

   public static readonly DomainErrors InvalidEmail =
      new("owner.invalid_email", "Email address is invalid.");

   public static readonly DomainErrors HasAccounts =
      new("owner.has_accounts", "Owner still has accounts.");
}
