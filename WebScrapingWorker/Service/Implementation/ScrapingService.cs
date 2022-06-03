using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
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

        public async Task GetProductsDataFromAmazonWebPage()
        {
            var products = await _scrapingRepository.GetProductsAsync();
            foreach (var product in products)
            {
                var url = $"{_appConfig.AmazonBaseUrl}/{product.IdProduct}";
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
                
                var str = await streamReader.ReadToEndAsync();
                Console.WriteLine(str);
                
                await using (var outputFile = new StreamWriter(Path.Combine(@"C:\Users\rpoir", "WriteTextAsync.txt")))
                {
                    await outputFile.WriteAsync(str);
                }

                
                /*var browser = new Browser();
                browser.HttpClient.DefaultRequestHeaders.Add("User-Agent", 
                                    "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.4; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2");

                var page = await browser.Open(amazonAddress);

                //var myIp = page.Select("#ip").Text();




                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();

                web.UserAgent = "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.4; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2";

                HtmlAgilityPack.HtmlDocument doc = web.Load(amazonAddress);
                var agilityNodes = doc.DocumentNode.SelectNodes("//a[@class='a-section review aok-relative']");

                var handler = new HttpClientHandler
                {
                    UseCookies = true,
                };

                var httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri(amazonAddress),
                };
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip, deflate");

                httpClient.DefaultRequestHeaders.Add("User-Agent","Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10.4; en-US; rv:1.9.2.2) Gecko/20100316 Firefox/3.6.2");


                var result = httpClient.GetAsync("").Result;

                string strData = "";


                strData = result.Content.ReadAsStringAsync().Result;



                var response = await httpClient.GetByteArrayAsync(amazonAddress);
                var responseString = Encoding.UTF8.GetString(response, 0, response.Length - 1);

                var response2 = await httpClient.GetAsync(amazonAddress);
                var contenttype = response2.Content.Headers.First(h => h.Key.Equals("Content-Type"));
                var rawencoding = contenttype.Value.First();

                if (rawencoding.Contains("utf8") || rawencoding.Contains("UTF-8"))
                {
                    var bytes = await response2.Content.ReadAsByteArrayAsync();
                    var finalRes =  Encoding.UTF8.GetString(bytes);
                }



                HtmlDocument htmlDocument = new();
                htmlDocument.LoadHtml(strData);

                var nodes = htmlDocument.DocumentNode.SelectNodes("//div[@class='a-section review aok-relative']");
                foreach (var node in nodes)
                {
                    System.Console.WriteLine(node.InnerHtml);
                }*/
            }
            await Task.CompletedTask;
        }
    }
}