using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebScrapingData.Extensions;
using WebScrapingWorker.Config;
using WebScrapingWorker.Extensions;
using WebScrapingWorker.Service;

namespace WebScrapingWorker
{
    public class Program
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

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFromConfiguration<AppConfig>(Configuration);
                    services.AddDatabase();
                    services.AddHostedService<ScrapingService>();
                });
    }
}
