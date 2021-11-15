using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;

namespace WebAPI.Repository.Interface
{
    public interface IFeedbackRepository : IGenericRepository<Feedback>
    {
        Task<Feedback> GetFeedbackByIdAsync(int id);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync();
        void CreateFeedback(Feedback feedback);
        void UpdateFeedback(Feedback feedback);
    }
}
