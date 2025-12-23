namespace BankingApi.Domain.UseCases;

public interface IAccountUcGetTransactions {

   Task<Result<IReadOnlyList<Entities.Transaction>>> ExecuteAsync(
      Guid accountId,
      DateOnly fromDate,
      DateOnly toDate,
      CancellationToken ct = default
   );
}
