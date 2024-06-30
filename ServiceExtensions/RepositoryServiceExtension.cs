using RandomApp1.Repositories;

namespace RandomApp1.ServiceExtensions
{
    public static class RepositoryServiceExtension
    {
        public static IServiceCollection AddRepositoryServices(this IServiceCollection services)
        {
            services.AddSingleton<IIdentityRepository, IdentityRepository>();
            services.AddSingleton<IRefreshTokenRepository, RefreshTokenRepository>();
            return services;
        }
    }
}