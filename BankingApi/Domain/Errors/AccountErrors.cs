namespace BankingApi.Domain.Errors;

public static class AccountErrors {
   public static readonly DomainErrors InvalidIban =
      new("invalid_iban", "The IBAN is invalid.");

   public static readonly DomainErrors OwnerNotFound =
      new("owner_not_found", "Owner not found.");
}