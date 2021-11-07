using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.ModelDTO;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class SubsriberRepository : GenericRepository<Subscriber>, ISubscriberRepository
    {
        public SubsriberRepository(PTStoreContext context) : base(context) { }

        public void AddSubscriber(Subscriber subscriber)
        {
            Create(subscriber);
        }

        public async Task<IEnumerable<Subscriber>> GetAllSubscribersAsync()
        {
            return await FindAll().ToListAsync();
        }

        public async Task<Subscriber> GetSubscriberByEmailAsync(string email)
        {
            return await FindByCondition(index => index.Email == email).FirstOrDefaultAsync();
        }

        public async Task<Subscriber> GetSubscriberByIdAsync(int id)
        {
            return await FindByCondition(index => index.Id == id).FirstOrDefaultAsync();
        }

        public void RemoveSubscriber(Subscriber subscriber)
        {
            Update(subscriber);
        }
    }
}
