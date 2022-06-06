using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using HtmlAgilityPack;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;
using WebScrapingWorker.Config;
using WebScrapingWorker.Extensions;
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
                //https://www.amazon.com/product-reviews/B082XY23D5/ref=cm_cr_arp_d_paging_btm_next_2?pageNumber=1

                var pageNumber = 22;
                var pageExist = true;
                while (pageExist)
                {
                    var url = $"{_appConfig.AmazonBaseUrl}/{product.ProductAsin}?pageNumber={pageNumber}";
                    _logger.LogInformation($"url to scrap : {url}");
                    
                    //https://stackoverflow.com/questions/30899113/httpclient-returning-special-characters-but-nothing-readable
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "text/html,application/xhtml+xml,application/xml");
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
                    _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
                    var response = await _httpClient.GetAsync(url);
                    if (!response.IsSuccessStatusCode)
                    {
                        pageExist = false;
                        continue;
                    }
  
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
                            .SelectNodes("//div[@data-hook='review']/div")
                            .Elements();

                    foreach (var webReview in webReviews)
                    {
                        var reviewTitleElement = webReview.SelectSingleNode(
                            ".//*[@data-hook='review-title']/span");
                        var reviewTitle = reviewTitleElement == null
                            ? string.Empty
                            : reviewTitleElement.InnerText;

                        var reviewContent = webReview.SelectSingleNode(".//span[@class='a-size-base review-text review-text-content']")
                            .InnerText
                            .Trim()
                            .Replace(@"\n","");
             
                        var reviewCardElement = webReview.Id;
                        var reviewCard= reviewCardElement.Split("-").ToList().Last();
               
                        //1.0 out of 5 stars
                        var reviewStarElement = webReview.SelectSingleNode(".//i[@data-hook='%review-star-rating']/span");
                        var reviewStar = reviewStarElement == null
                            ? 0
                            : reviewStarElement.InnerText.ExtractStars();
                        
                        //Reviewed in the United States on March 8, 2020
                        var reviewCountryElement = webReview.SelectSingleNode(".//span[@data-hook='review-date']");
                        var reviewCountry = reviewCountryElement == null
                            ? string.Empty
                            : reviewCountryElement.InnerText.ExtractCountry();
                        
                        var reviewDate = webReview.SelectSingleNode(".//span[@data-hook='review-date']")
                            .InnerText
                            .ExtractDateTime();
                        
           
                        //Verified Purchase
                        var reviewVerifiedElement = webReview.SelectSingleNode(".//span[@data-hook='avp-badge']");
                        var reviewVerified = reviewVerifiedElement != null && reviewVerifiedElement
                            .InnerText
                            .CheckVerified();
                        
                        //271 people found this helpful
                        var reviewValidationElement = webReview.SelectSingleNode(".//span[@data-hook='helpful-vote-statement']");
                        var reviewValidation = reviewValidationElement ==null 
                            ? 0
                            : reviewValidationElement
                                .InnerText
                                .ExtractReviewValidations();

                        var reviewProfileElement = webReview.SelectSingleNode(".//span[@class='a-profile-name']");
                        var reviewProfile = reviewProfileElement == null
                            ? string.Empty
                            : reviewProfileElement
                            . InnerText;
                        

                        var review = new Review
                        {
                            Card = reviewCard,
                            ReviewComment = reviewContent,
                            ReviewCountry = reviewCountry,
                            ReviewDate = reviewDate,
                            ReviewStars = reviewStar,
                            ReviewTitle = reviewTitle,
                            ReviewValidation = reviewValidation,
                            ReviewVerified = reviewVerified,
                            ReviewProfile = reviewProfile
                        };
                        await _scrapingRepository.AddOrUpdateReview(review);

                    }
                    pageNumber++;
                }
                
                
     
            }
            await Task.CompletedTask;
        }
    }
}