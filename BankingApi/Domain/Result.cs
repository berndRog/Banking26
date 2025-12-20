using BankingApi.Domain.Errors;
namespace BankingApi.Domain;

public readonly struct Result<T> {
   
   public bool IsSuccess { get; }
   public T? Value { get; }
   public DomainErrors? Errors { get; }

   private Result(T value) {
      IsSuccess = true;
      Value = value;
      Errors = null;
   }

   private Result(DomainErrors errors) {
      IsSuccess = false;
      Value = default;
      Errors = errors;
   }

   public static Result<T> Success(T value) => new(value);
   public static Result<T> Fail(DomainErrors errors) => new(errors);

   // Kotlin-style fluent API
   public Result<T> OnSuccess(Action<T> action) {
      if (IsSuccess && Value is not null)
         action(Value);
      return this;
   }

   public Result<T> OnFailure(Action<DomainErrors> action) {
      if (!IsSuccess && Errors is not null)
         action(Errors);
      return this;
   }
}