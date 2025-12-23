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
   
   
   public T GetValueOrDefault(T defaultValue = default!) {
      return IsSuccess && Value is not null ? Value : defaultValue;
   }

   public T GetValueOrThrow() {
      if (!IsSuccess || Value is null)
         throw new InvalidOperationException($"Result failed: {Error}");
      return Value;
   }
   
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
   
   public TResult Fold<TResult>(
      Func<T, TResult> onSuccess,
      Func<DomainErrors, TResult> onFailure
   ) {
      return IsSuccess && Value is not null
         ? onSuccess(Value)
         : onFailure(Error!);
   }
   /*
   public Result<TResult> Map<TResult>(Func<T, TResult> mapper) {
      return IsSuccess && Value is not null
         ? Result<TResult>.Success(mapper(Value))
         : Result<TResult>.Fail(Error!);
   }
   
   public Result<TResult> Bind<TResult>(Func<T, Result<TResult>> binder) {
      return IsSuccess && Value is not null
         ? binder(Value)
         : Result<TResult>.Fail(Error!);
   }
   */
}