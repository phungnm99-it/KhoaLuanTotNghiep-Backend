using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(int productId);
        Task<IEnumerable<Review>> GetAllOwnReviewsAsync(int userId);
        void CreateReview(Review review);
        void UpdateReview(Review review);
        Task<Review> GetReviewByUserIdAndProductIdAsync(int userId, int productId);
    }
}
