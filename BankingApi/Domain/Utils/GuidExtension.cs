using BankingApi.Domain.Errors;
namespace BankingApi.Domain.Utils;

public static class GuidExtensions {
   
   /// <summary>
   /// Returns a short log-friendly representation of a Guid
   /// (first 8 hex characters).
   /// </summary>
   public static string To8(this Guid value) =>
      value.ToString("N")[..8];

   /// <summary>
   /// Tries to parse a Guid from a string.
   /// Returns Result.Fail(...) instead of throwing exceptions.
   /// </summary>
   public static Result<Guid> ToGuid(this string value) {
      if (Guid.TryParse(value, out var guid))
         return Result<Guid>.Success(guid);

      return Result<Guid>.Failure(DomainErrors.InvalidGuidFormat);
   }
}