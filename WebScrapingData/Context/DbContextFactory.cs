namespace WebScrapingData.Context
{
    public class DbContextFactory
    {
        public ScrapingContext CreateContext()
        {
            return new ScrapingContext();
        }
    }
}