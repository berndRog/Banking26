namespace BankingApi.Domain.Errors;

public static class AddressErrors {
   public static readonly DomainErrors street_required =
      new("address.street_required", "Street must not be empty");

   public static readonly DomainErrors zip_code_required =
      new("address.zip_code_required", "Zip code must not be empty");

   public static readonly DomainErrors city_required =
      new("address.city_required", "City must not be empty");
}