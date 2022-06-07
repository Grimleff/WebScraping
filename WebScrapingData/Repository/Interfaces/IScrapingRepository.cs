using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebScrapingData.Model;

namespace WebScrapingData.Repository.Interfaces
{
    public interface IScrapingRepository
    {
        Task<Product> GetProductAsync(string productAsin);
        Task<IEnumerable<Product>> GetProductsAsync();
        Task<IEnumerable<Product>> GetEnableProductsAsync();
        
        Task<int> AddProductAsync(Product product);
        Task<int> AddProductsAsync(IEnumerable<Product> products);
        Task<int> UpdateProductAsync(Product product);

        Task<Review> GetReviewAsync(string reviewCard);
        Task<IEnumerable<Review>> GetReviewsAsync();
        Task<IEnumerable<Review>> GetReviewsFromAsinProductAsync(string productAsin);
        Task<IEnumerable<Review>> GetReviewsFromAsinsProductAsync(string[] productAsin, DateTime reviewMaxLastDate);

        Task<int> AddReviewAsync(Review review);
        Task<int> AddReviewsAsync(IEnumerable<Review> reviews);

        Task<int> UpdateReviewAsync(Review review);

        Task<int> AddOrUpdateProduct(Product product);

        Task<int> AddOrUpdateReview(Review review);
    }
}