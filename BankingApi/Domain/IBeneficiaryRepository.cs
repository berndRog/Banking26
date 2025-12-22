using BankingApi.Domain.Entities;
namespace BankingApi.Domain;

public interface IBeneficiaryRepository {
   Task<Beneficiary?> FindByIdAsync(Guid beneficiaryId);
   Task AddAsync(Beneficiary beneficiary);
   Task RemoveAsync(Beneficiary beneficiary);
}
