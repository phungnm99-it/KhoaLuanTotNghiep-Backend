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

        public async Task<IEnumerable<Review>> GetAllOwnReviews(int userId)
        {
            return await FindByCondition(review => review.UserId == userId)
                .Include(rv => rv.Product)
                .Include(rv => rv.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetAllReviewsByProductId(int productId)
        {
            return await FindByCondition(review => review.ProductId == productId)
                .Include(rv => rv.Product)
                .Include(rv => rv.User)
                .ToListAsync();
        }

        public void UpdateReview(Review review)
        {
            Update(review);
        }
    }
}
