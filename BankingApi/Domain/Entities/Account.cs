using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Entities;

public sealed class Account {

   // Properties
   public Guid Id { get; private set; }
   public string Iban { get; private set; } = string.Empty;
   public decimal Balance { get; private set; } = 0m;

   // Account -> Owner [0..*] : [1] 
   public Guid OwnerId { get; private set; }
   
   // Empfänger: Account -> Beneficiaries [1] : [0..*]
   private readonly List<Beneficiary> _beneficiaries = new();
   public IReadOnlyCollection<Beneficiary> Beneficiaries => _beneficiaries.AsReadOnly();
   // Überweisungen: Account -> Transfer [1] : [0..*]
   private readonly List<Transfer> _transfers = new();
   public IReadOnlyCollection<Transfer> Transfers => _transfers.AsReadOnly();
   // Buchungen: Account -> Transaction [1] : [0..*]
   private readonly List<Transaction> _transactions = new();
   public IReadOnlyCollection<Transaction> Transactions => _transactions.AsReadOnly();
   
   // ctor for EF Core
   private Account() { }

   // static factory method to create a new account for an existing owner
   public static Result<Account> Create(
      Guid ownerId,
      string iban
   ) {
      if (ownerId == Guid.Empty)
         return Result<Account>.Failure(AccountErrors.InvalidOwnerId);

      if (string.IsNullOrWhiteSpace(iban))
         return Result<Account>.Failure(AccountErrors.InvalidIban);

      return Result<Account>.Success(new Account {
         Id      = Guid.NewGuid(),
         OwnerId = ownerId,
         Iban    = iban.Trim(),
         Balance = 0m
      });
   }
   
   public bool HasSufficientFunds(decimal amount) =>
      amount > 0m && Balance >= amount;


   // Domain operation used later by transfers/transactions
   public Result<Account> Deposit(decimal amount) {
      if (amount <= 0m)
         return Result<Account>.Failure(AccountErrors.InvalidAmount);

      Balance += amount;
      return Result<Account>.Success(this);
   }
   
   public Result<Account> Withdraw(decimal amount) {
      if (amount <= 0m)
         return Result<Account>.Failure(AccountErrors.InvalidAmount);

      if (Balance < amount)
         return Result<Account>.Failure(AccountErrors.InsufficientFunds);

      Balance -= amount;
      return Result<Account>.Success(this);
   }

   // Story 3.1: add a beneficiary to THIS account
   public Result<Beneficiary> AddBeneficiary(
      string firstName,
      string lastName,
      string? companyName,
      string iban
   ) {
      var result = Beneficiary.Create(Id, firstName, lastName, companyName, iban);
      if (!result.IsSuccess)
         return result;

      //if (_beneficiaries.Any(b => string.Equals(b.Iban, result.Value!.Iban, StringComparison.OrdinalIgnoreCase)))
      //    return Result<Beneficiary>.Fail(BeneficiaryErrors.DuplicateIban);

      _beneficiaries.Add(result.Value!);
      return result;
   }
   
   // Story 3.2: delete beneficiary only
   public Result<Guid> RemoveBeneficiary(Guid beneficiaryId) {
      if (beneficiaryId == Guid.Empty)
         return Result<Guid>.Failure(BeneficiaryErrors.InvalidBeneficiaryId);

      var found = _beneficiaries.FirstOrDefault(b => b.Id == beneficiaryId);
      if (found is null)
         return Result<Guid>.Failure(BeneficiaryErrors.NotFound);

      _beneficiaries.Remove(found);
      return Result<Guid>.Success(beneficiaryId);
   }
}
