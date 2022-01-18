using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(PTStoreContext context) : base(context) { }

        public void CreateReview(Review review)
        {
            Create(review);
        }

        public async Task<IEnumerable<Review>> GetAllOwnReviewsAsync(int userId)
        {
            return await FindByCondition(review => review.UserId == userId)
                .Include(rv => rv.Product)
                .Include(rv => rv.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReviews()
        {
            return await FindAll().Include(rv => rv.Product)
                .Include(rv => rv.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReviewsByProductIdAsync(int productId)
        {
            return await FindByCondition(review => review.ProductId == productId)
                .OrderByDescending(rv => rv.ReviewTime)
                .Include(rv => rv.Product)
                .Include(rv => rv.User)
                .ToListAsync();
        }

        public async Task<Review> GetReviewByUserIdAndProductIdAsync(int userId, int productId)
        {
            return await FindByCondition(review => review.ProductId == productId && review.UserId == userId)
                .Include(rv => rv.Product)
                .Include(rv => rv.User)
                .FirstOrDefaultAsync();
        }

        public void UpdateReview(Review review)
        {
            Update(review);
        }
    }
}
