using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebScrapingWorker.Config;

namespace WebScrapingWorker.Service
{
    public class ScrapingService : BackgroundService
    {
        private readonly AppConfig _appConfig;
        private readonly ILogger<ScrapingService> _logger;
        
        public ScrapingService(AppConfig appConfig, ILogger<ScrapingService> logger)
        {
            _appConfig = appConfig;
            _logger = logger;
        }
        
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
                try
                {
                    
                    _logger.LogInformation(
                        $"Success running background service {typeof(ScrapingService).FullName} at {DateTime.UtcNow}");
                }
                catch (Exception e)
                {

                    
                    _logger.LogError(
                        $"Failed running background service {typeof(ScrapingService).FullName} at {DateTime.UtcNow} : {e.Message}");
                }
                finally
                {
                    await Task.Delay(TimeSpan.FromSeconds(_appConfig.BackgroundServiceCycleInSecond), stoppingToken);
                }
        }
    }
}
