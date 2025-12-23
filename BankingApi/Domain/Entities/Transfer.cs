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
public sealed class Transfer {

   public Guid Id { get; private set; }
   public DateTimeOffset DtOffSet { get; private set; }
   public Guid FromAccountId { get; private set; }
   public Guid ToAccountId   { get; private set; }
   public decimal Amount     { get; private set; }
   public string Purpose     { get; private set; } = string.Empty;
   // optional reference for reversal (Storno)
   public Guid? ReversalOfTransferId { get; private set; }
   public bool IsReversed => ReversalOfTransferId.HasValue;

   // ctor for EF Core
   private Transfer() { }

   // domain ctor
   public Transfer(
      Guid fromAccountId,
      Guid toAccountId,
      DateTimeOffset dtOffset,
      decimal amount,
      string purpose,
      Guid? reversalOfTransferId = null
   ) {
      Id = Guid.NewGuid();
      FromAccountId = fromAccountId;
      ToAccountId   = toAccountId;
      DtOffSet      = dtOffset;
      Amount        = amount;
      Purpose       = purpose;
      ReversalOfTransferId = reversalOfTransferId;
   }
}

