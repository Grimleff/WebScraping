using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebScrapingData.Context;
using WebScrapingData.Model;
using WebScrapingData.Repository.Interfaces;

namespace WebScrapingData.Repository.Implementation
{
    public class ScrapingRepository : RepositoryBase, IScrapingRepository
    {
        public ScrapingRepository(DbContextFactory contextFactory) : base (contextFactory)
        {
        }

        public async Task<Product> GetProductAsync(string idProduct)
        {
            return await Db.FindAsync<Product>(idProduct);
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Db.Products.ToListAsync();
        }

        public async void AddProductAsync(Product product)
        {
            await Db.AddAsync(product);
            await Db.SaveChangesAsync();
        }

        public async void AddProductsAsync(IEnumerable<Product> products)
        {
            foreach (var product in products)
            {
                await Db.AddAsync(product);
            }
            await Db.SaveChangesAsync();
        }

        public async Task<Review> GetReviewAsync(long idReview)
        {
            return await Db.FindAsync<Review>(idReview);
        }

        public async Task<IEnumerable<Review>> GetReviewAsync()
        {
            return await Db.Reviews.ToListAsync();
        }

        public async void AddReviewAsync(Review review)
        {
            await Db.AddAsync(review);
            await Db.SaveChangesAsync();
        }

        public async void AddReviewsAsync(IEnumerable<Review> reviews)
        {
            foreach (var review in reviews)
            {
                await Db.AddAsync(review);
            }
            await Db.SaveChangesAsync();
        }
    }
}