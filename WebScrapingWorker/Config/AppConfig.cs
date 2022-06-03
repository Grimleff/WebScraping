using WebScrapingWorker.Attributes;

namespace WebScrapingWorker.Config
{
    [Configuration("App")]
    public class AppConfig
    {
        public int BackgroundServiceCycleInSecond { get; set; }
        public string AmazonBaseUrl { get; set; }
    }
}