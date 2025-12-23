namespace BankingApi.Domain.UseCases;

public interface IAccountUcRemoveBeneficiary {
   Task<Result<Guid>> ExecuteAsync(
      Guid beneficiaryId,
      CancellationToken ct = default
   );
}
