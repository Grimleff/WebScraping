using System;
using WebScrapingData.Context;

namespace WebScrapingData.Repository
{
    public abstract class RepositoryBase : IDisposable
    {
        private readonly DbContextFactory _contextFactory;
        private ScrapingContext _scrapingContext;

        protected RepositoryBase(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        protected ScrapingContext Db => _scrapingContext ??= _contextFactory.CreateContext();

        public void Dispose()
        {
            _scrapingContext?.Dispose();
        }
    }
}