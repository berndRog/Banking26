using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases;

public interface IOwnerUcAddPrivateAddress {
   Task<Result> ExecuteAsync(
      Guid ownerId,
      string street,
      string zipCode,
      string city,
      CancellationToken ct = default
   );
}