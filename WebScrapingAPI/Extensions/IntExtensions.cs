namespace WebScrapingAPI.Extensions
{
    public static class IntExtensions
    {
        private const int MaxPageSize = 500;
        private const int DefaultPageSize = 50;
        private const int DefaultPage = 1;

        public static int LimitPageSize(this int? pageSize)
        {
            if (pageSize is >= 0)
            {
                return pageSize.Value <= MaxPageSize ? pageSize.Value : MaxPageSize;
            }
            return DefaultPageSize;
        }

        public static int LimitPage(this int? page)
        {
            return page is > 0 ? page.Value : DefaultPage;
        }
    }
}