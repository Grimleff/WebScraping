using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebScrapingData.Context;

namespace WebScrapingData.Service
{
    public class DatabaseInitializerService : IHostedService
    {
        private readonly DbContextFactory _contextFactory;
        public DatabaseInitializerService(DbContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await using var context = _contextFactory.CreateContext();
            await context.Database.EnsureCreatedAsync(cancellationToken);
            await context.Database.MigrateAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}