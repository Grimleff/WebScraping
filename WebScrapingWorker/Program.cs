using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebScrapingData.Extensions;
using WebScrapingData.Repository.Implementation;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Config;
using WebScrapingWorker.Extensions;
using WebScrapingWorker.BgService;
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

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFromConfiguration<AppConfig>(Configuration);
                    services.AddDatabase();
                    services.AddTransient<IScrapingRepository, ScrapingRepository>();
                    services.AddTransient<IScrapingService, ScrapingService>();
                    services.AddHttpClient<IScrapingService, ScrapingService>();
                    services.AddHostedService<WebScrapingService>();
                });
    }
}
