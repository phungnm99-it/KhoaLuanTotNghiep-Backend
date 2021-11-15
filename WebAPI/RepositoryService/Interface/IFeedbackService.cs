using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IFeedbackService
    {
        Task<FeedbackDTO> GetFeedbackByIdAsync(int id);
        Task<IEnumerable<FeedbackDTO>> GetAllFeedbacksAsync();
        Task<FeedbackDTO> CreateFeedbackAsync(FeedbackModel feedback);
        Task<FeedbackDTO> ReplyFeedbackAsync(ReplyFeedbackModel model, int adminId);
    }
}
