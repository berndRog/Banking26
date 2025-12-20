using BankingApi.Domain.Entities;
namespace BankingApi.Domain.UseCases.Owners;

public sealed class OwnerUcCreate(
   IOwnerRepository repository,
   IUnitOfWork unitOfWork,
   ILogger<OwnerUcCreate> logger
) : IOwnerUcCreate {
   
   private readonly IOwnerRepository _repository = repository;
   private readonly IUnitOfWork _unitOfWork = unitOfWork;
   private readonly ILogger<OwnerUcCreate> _logger = logger;

   public async Task<Result<Owner>> ExecuteAsync(
      string firstName,
      string lastName,
      string email
   ) {
      
      var result = Owner.Create(firstName, lastName, email);
      if (!result.IsSuccess) {
         _logger.LogWarning("Create Owner failed: {ErrorCode}", result.Errors!.Code);
         return result;
      }

      // The UseCase represents one business transaction:
      // added changes are tracked via repositories and committed once - as unit of Work
      await _repository.AddAsync(result.Value!);
      // save all changes to database using a transaction
      await _unitOfWork.SaveChangesAsync();

      _logger.LogInformation("Owner created successfully: {OwnerId}", result.Value!.Id);

      return result;
   }
}