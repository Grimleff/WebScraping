using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebScrapingData.Extensions;
using WebScrapingData.Repository.Implementation;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.BgService;
using WebScrapingWorker.Config;
using WebScrapingWorker.Extensions;
using WebScrapingWorker.Service.Implementation;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker
{
    public static class Program
    {
        private static readonly IConfiguration Configuration;

        static Program()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
        }

        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(options =>
                    {
                        options.Listen(IPAddress.Any, 5005);
                    });
                    webBuilder.UseStartup<Startup>();
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSignalR();
                    services.AddFromConfiguration<AppConfig>(Configuration);
                    services.AddDatabase();
                    services.AddTransient<IScrapingRepository, ScrapingRepository>();
                    services.AddTransient<IScrapingService, ScrapingService>();
                    services.AddHttpClient<IScrapingService, ScrapingService>();
                    services.AddCors(options => options.AddPolicy("CorsPolicy",
                        builder =>
                        {
                            builder.AllowAnyHeader()
                                .AllowAnyMethod()
                                .SetIsOriginAllowed((host) => true)
                                .AllowCredentials();
                        }));
                    
                    services.AddHostedService<WebScrapingService>();
                });
        }
    }
}