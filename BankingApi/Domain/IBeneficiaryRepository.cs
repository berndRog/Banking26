using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface IBeneficiaryRepository {
   Task<Beneficiary?> FindByIdAsync(Guid beneficiaryId, CancellationToken ct = default);
   void Add(Beneficiary beneficiary);
   void Remove(Beneficiary beneficiary);
}
