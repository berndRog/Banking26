using BankingApi.Domain.Errors;
namespace BankingApi.Domain.ValueObjects;

public sealed record Address {
   public string Street  { get; private set; } = "";
   public string ZipCode { get; private set; } = "";
   public string City    { get; private set; } = "";

   private Address() { } // EF

   // Factory method
   public static Result<Address> Create(string street, string zipCode, string city) {
      if (string.IsNullOrWhiteSpace(street))
         return Result<Address>.Failure(AddressErrors.street_required);

      if (string.IsNullOrWhiteSpace(zipCode))
         return Result<Address>.Failure(AddressErrors.zip_code_required);

      if (string.IsNullOrWhiteSpace(city))
         return Result<Address>.Failure(AddressErrors.city_required);

      var address = new Address
      {
         Street  = street.Trim(),
         ZipCode = zipCode.Trim(),
         City    = city.Trim()
      };

      return Result<Address>.Success(address);
   }
}
