namespace BankingApi.Domain.UseCases;

public interface IOwnerUcAddCompanyAddress {
   Task<Result> ExecuteAsync(
      Guid ownerId,
      string street,
      string zipCode,
      string city,
      CancellationToken ct = default
   );
}