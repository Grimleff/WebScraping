using System.Collections.Generic;
using System.Threading.Tasks;
using WebScrapingData.Model;

namespace WebScrapingData.Repository.Interfaces
{
    public interface IScrapingRepository
    {
        Task<Product> GetProductAsync(string idProduct);
        Task<IEnumerable<Product>> GetProductsAsync();
        
        void AddProductAsync(Product product);
        void AddProductsAsync(IEnumerable<Product> products);

        Task<Review> GetReviewAsync(long idReview);
        Task<IEnumerable<Review>> GetReviewAsync();

        void AddReviewAsync(Review review);
        void AddReviewsAsync(IEnumerable<Review> reviews);


    }
}