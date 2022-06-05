using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Product> GetProductAsync(string productAsin)
        {
            return await Db.Products.FirstOrDefaultAsync(x => x.ProductAsin.Equals(productAsin));
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Db.Products.ToListAsync();
        }

        public async Task<int> AddProductAsync(Product product)
        {
            await Db.AddAsync(product);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> AddProductsAsync(IEnumerable<Product> products)
        {
            await Db.AddRangeAsync(products);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> UpdateProductAsync(Product product)
        {
            var dbProduct = await Db.Products.FirstOrDefaultAsync(x => x.ProductAsin.Equals(product.ProductAsin));
            product.IdProduct = dbProduct.IdProduct;
            Db.Entry(dbProduct).CurrentValues.SetValues(product);
            return await Db.SaveChangesAsync();
        }

        public async Task<Review> GetReviewAsync(string reviewCard)
        {
            return await Db.Reviews.FirstOrDefaultAsync(x=>x.Card.Equals(reviewCard));
        }

        public async Task<IEnumerable<Review>> GetReviewAsync()
        {
            return await Db.Reviews.ToListAsync();
        }

        public async Task<int> AddReviewAsync(Review review)
        {
            await Db.AddAsync(review);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> AddReviewsAsync(IEnumerable<Review> reviews)
        {
            await Db.AddRangeAsync(reviews);
            return await Db.SaveChangesAsync();
        }

        public async Task<int> UpdateReviewAsync(Review review)
        {
            var dbReview = await Db.Reviews.FirstOrDefaultAsync(x => x.Card.Equals(review.Card));
            review.IdReview = dbReview.IdReview;
            Db.Entry(dbReview).CurrentValues.SetValues(review);
            return await Db.SaveChangesAsync();
        }
        
        public async Task<int> AddOrUpdateProduct(Product product)
        {
            var dbProduct = await GetProductAsync(product.ProductAsin);
            if (dbProduct == null)
            {
                return await AddProductAsync(product);
                
            }
            return await UpdateProductAsync(product);
        }
        
        public async Task<int> AddOrUpdateReview(Review review)
        {
            var dbReview = await GetReviewAsync(review.Card);
            if (dbReview == null)
            {
                return await AddReviewAsync(review);
                
            }
            return await UpdateReviewAsync(review);
        }
    }
}