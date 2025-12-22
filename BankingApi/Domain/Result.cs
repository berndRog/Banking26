using BankingApi.Domain.Errors;
namespace BankingApi.Domain;

public readonly struct Result<T> {
   
   public bool IsSuccess { get; }
   public T? Value { get; }
   public DomainErrors? Error { get; }

   private Result(T value) {
      IsSuccess = true;
      Value = value;
      Error = null;
   }

   private Result(DomainErrors error) {
      IsSuccess = false;
      Value = default;
      Error = error;
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
      if (!IsSuccess && Error is not null)
         action(Error);
      return this;
   }
}