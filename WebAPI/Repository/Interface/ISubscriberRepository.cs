using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.ModelDTO;

namespace WebAPI.Repository.Interface
{
    public interface ISubscriberRepository : IGenericRepository<Subscriber>
    {
        void AddSubscriber(Subscriber subscriber);
        void RemoveSubscriber(Subscriber subscriber);
        Task<Subscriber> GetSubscriberByIdAsync(int id);
        Task<Subscriber> GetSubscriberByEmailAsync(string email);
        Task<IEnumerable<Subscriber>> GetAllSubscribersAsync();
    }
}
