using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Config;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker.Service.Implementation
{
    public class ScrapingService : IScrapingService
    {
        private readonly IScrapingRepository _scrapingRepository;
        private readonly AppConfig _appConfig;
        private readonly ILogger<ScrapingService> _logger;
        public ScrapingService(IScrapingRepository scrapingRepository, AppConfig appConfig, ILogger<ScrapingService> logger)
        {
            _scrapingRepository = scrapingRepository;
            _appConfig = appConfig;
            _logger = logger;
        }

        public async Task GetProductsDataFromAmazonWebPage()
        {
            var products = await _scrapingRepository.GetProductsAsync();
            foreach (var product in products)
            {
                var url = $"{_appConfig.AmazonBaseUrl}/{product.IdProduct}";
                _logger.LogInformation($"url to scrap : {url}");
            }

            await Task.CompletedTask;
        }
    }
}