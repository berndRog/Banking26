using Microsoft.Extensions.DependencyInjection;

namespace BankingApi.Ui;

public static class DiAddUiExtensions {
   
   public static IServiceCollection AddUi(
      this IServiceCollection services
   ) {
      services.AddControllers();

      services.AddEndpointsApiExplorer();
      services.AddSwaggerGen();

      return services;
   }
}
