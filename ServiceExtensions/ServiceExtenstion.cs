using RandomApp1.Data;
using RandomApp1.Services;
using RandomApp1.Utils;

namespace RandomApp1.ServiceExtensions
{
    public static class ServiceExtenstion
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddExceptionHandler<GlobalExceptionHandler>();
            services.AddProblemDetails();
            services.AddSingleton<ITokenService, TokenService>();
            services.AddSingleton<DataContext>();
            services.AddSingleton<IIdentityService, IdentityService>();
            return services;
        }
    }
}