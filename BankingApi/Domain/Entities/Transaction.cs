namespace BankingApi.Domain.Entities;

/*
   Transfer (Überweisung)
      → Geschäftsvorgang, vom Nutzer ausgelöst
   Transaction (Buchung)
      → Kontobewegung, technisch/fachlich abgeleitet
   Ein Transfer erzeugt immer zwei Transactions
   
   Konto	            Transaction	   Betrag
   Senderkonto	      Lastschrift	   −X
   Empfängerkonto	   Gutschrift	   +X
 */

public sealed class Transaction {

   public Guid Id { get; private set; }
   public DateTimeOffset BookingDate { get; private set; }
   public decimal Amount { get; private set; }
   public string Purpose { get; private set; } = string.Empty;

   public Guid AccountId { get; private set; }
   public Guid TransferId { get; private set; }

   // ctor for EF Core
   private Transaction() { }

   // domain ctor
   public Transaction(
      Guid accountId,
      Guid transferId,
      DateTimeOffset bookingDate,
      decimal amount,
      string purpose
   ) {
      Id          = Guid.NewGuid();
      AccountId   = accountId;
      TransferId  = transferId;
      BookingDate = bookingDate;
      Amount      = amount;
      Purpose     = purpose;
   }
}
