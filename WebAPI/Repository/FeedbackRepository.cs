using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class FeedbackRepository : GenericRepository<Feedback>, IFeedbackRepository
    {
        public FeedbackRepository(PTStoreContext context) : base(context) { }

        public void CreateFeedback(Feedback feedback)
        {
            Create(feedback);
        }

        public async Task<IEnumerable<Feedback>> GetAllFeedbacksAsync()
        {
            return await FindAll()
                .Include(feedback => feedback.RepliedByNavigation).ToListAsync();
        }

        public async Task<Feedback> GetFeedbackByIdAsync(int id)
        {
            return await FindByCondition(index => index.Id == id)
                .Include(feedback => feedback.RepliedByNavigation).FirstOrDefaultAsync();
        }

        public void UpdateFeedback(Feedback feedback)
        {
            Update(feedback);
        }
    }
}
