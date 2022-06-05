using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Config;
using WebScrapingWorker.Service.Interfaces;

namespace WebScrapingWorker.Service.Implementation
{
    public class ScrapingService : IScrapingService
    {
        private readonly IScrapingRepository _scrapingRepository;
        private readonly HttpClient _httpClient;
        private readonly AppConfig _appConfig;
        private readonly ILogger<ScrapingService> _logger;
        public ScrapingService(IScrapingRepository scrapingRepository, HttpClient httpClient, AppConfig appConfig, ILogger<ScrapingService> logger)
        {
            _scrapingRepository = scrapingRepository;
            _httpClient = httpClient;
            _appConfig = appConfig;
            _logger = logger;
        }

        public async Task AddNewProduct(Product product)
        {
            var result = await _scrapingRepository.AddOrUpdateProduct(product);
        }
        public async Task GetProductsDataFromAmazonWebPage()
        {
            var products = await _scrapingRepository.GetProductsAsync();
            foreach (var product in products)
            {
                var url = $"{_appConfig.AmazonBaseUrl}/{product.ProductAsin}";
                _logger.LogInformation($"url to scrap : {url}");
                
                //https://stackoverflow.com/questions/30899113/httpclient-returning-special-characters-but-nothing-readable
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                var response = await _httpClient.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                    continue;
                //response.EnsureSuccessStatusCode();
 
                await using var responseStream = await response.Content.ReadAsStreamAsync();
                await using var deflateStream = new GZipStream(responseStream, CompressionMode.Decompress);
                using var streamReader = new StreamReader(deflateStream);
                
                var html = await streamReader.ReadToEndAsync();
                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);
                var webReviews =
                    htmlDoc
                        .DocumentNode
                        .SelectSingleNode("//div[@id='cm_cr-review_list']")
                        .SelectNodes("//div[@data-hook='review']");

                foreach (var webReview in webReviews)
                {
                    var title = webReview.SelectSingleNode("//a[@class='a-size-base a-link-normal review-title a-color-base review-title-content a-text-bold']/span").InnerText;
                    var content = webReview.SelectSingleNode("//span[@class='a-size-base review-text review-text-content']")
                        .InnerText
                        .Trim()
                        .Replace(@"\n","");
                    //
                    var reviewCard = webReview.Id;
                    //review-star-rating
                    var reviewStar = webReview.SelectSingleNode("//i[@data-hook='review-star-rating']/span");
                    var reviewCountry = "";
                    var reviewDate = webReview.SelectSingleNode("//span[@data-hook='review-date']");
                    //avp-badge
                    var reviewVerified = webReview.SelectSingleNode("//span[@data-hook='avp-badge']");
                    //helpful-vote-statement
                    var reviewValidation = webReview.SelectSingleNode("//span[@data-hook='helpful-vote-statement']");

                    var review = new Review
                    {
                        Card = reviewCard,
                        ReviewComment = content,
                        ReviewCountry = reviewCountry,
                        ReviewDate = DateTime.UtcNow,
                        ReviewStars = 3,
                        ReviewTitle = title,
                        ReviewValidation = 0,
                        ReviewVerified = true
                    };
                    await _scrapingRepository.AddOrUpdateReview(review);

                }
                
     
            }
            await Task.CompletedTask;
        }
    }
}