using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.Repository.Interface;

namespace WebAPI.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private PTStoreContext RepositoryContext { get; set; }
        public IProductRepository ProductRepository { get; set; }
        public IUserRepository UserRepository { get; set; }
        public IBrandRepository BrandRepository { get; set; }
        public ISubscriberRepository SubscriberRepository { get; set; }

        public UnitOfWork(PTStoreContext context,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IBrandRepository brandRepository,
            ISubscriberRepository subscriberRepository)
        {
            RepositoryContext = context;
            ProductRepository = productRepository;
            UserRepository = userRepository;
            BrandRepository = brandRepository;
            SubscriberRepository = subscriberRepository;
        }

        public async Task SaveAsync()
        {
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
