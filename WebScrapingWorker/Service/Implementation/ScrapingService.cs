using System.Threading.Tasks;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker.Service.Implementation
{
    public class ScrapingService : IScrapingService
    {
        private readonly IScrapingRepository _scrapingRepository;

        public ScrapingService(IScrapingRepository scrapingRepository)
        {
            _scrapingRepository = scrapingRepository;
        }
        public Task AddNewProduct(Product product)
        {
            _scrapingRepository.AddProductAsync(product);
            return Task.CompletedTask;
        }
    }
}