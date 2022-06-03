using System;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebScrapingWorker.Attributes;

namespace WebScrapingWorker.Extensions
{
    public static class ConfigurationExtensions
    {
        public static void AddFromConfiguration<T>(this IServiceCollection services, IConfiguration configuration) where T : class
        {
            var attribute = typeof(T).GetCustomAttribute<ConfigurationAttribute>();
            if (attribute is null)
            {
                throw new InvalidOperationException($"Missing Configuration attribute on {typeof(T).Name}");
            }

            services.AddSingleton(configuration.GetSection(attribute.SectionName).Get<T>());
        }
    }
}