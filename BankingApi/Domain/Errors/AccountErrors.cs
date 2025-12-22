namespace BankingApi.Domain.Errors;

public static class AccountErrors {

   public static readonly DomainErrors InvalidOwnerId =
      new("account.invalid_owner_id", "OwnerId must not be empty.");

   public static readonly DomainErrors InvalidIban =
      new("account.invalid_iban", "IBAN must not be empty.");

   public static readonly DomainErrors InvalidAmount =
      new("account.invalid_amount", "Amount must be greater than 0.");

   public static readonly DomainErrors InsufficientFunds =
      new("account.insufficient_funds", "Insufficient funds for this operation.");

 
   public static readonly DomainErrors NotFound =
      new("account.not_found", "Account not found.");
}
