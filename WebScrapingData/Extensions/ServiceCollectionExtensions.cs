using Microsoft.Extensions.DependencyInjection;
using WebScrapingData.Context;
using WebScrapingData.Service;

namespace WebScrapingData.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            services.AddTransient<DbContextFactory>();
            services.AddHostedService<DatabaseInitializerService>();
            return services;
        }
    }
}