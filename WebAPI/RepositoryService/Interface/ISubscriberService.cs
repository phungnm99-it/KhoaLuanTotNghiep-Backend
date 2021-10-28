using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface ISubscriberService
    {
        Task<SubscriberDTO> GetSubscriberByIdAsync(int id);
        Task<IEnumerable<SubscriberDTO>> GetAllSubscribersAsync();
        Task<SubscriberDTO> AddSubscriberAsync(string email);
        Task<bool> RemoveSubscriberAsync(int id);
    }
}
