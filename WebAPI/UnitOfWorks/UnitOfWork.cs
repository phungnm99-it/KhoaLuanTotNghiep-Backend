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

        public UnitOfWork(PTStoreContext context,
            IProductRepository productRepository,
            IUserRepository userRepository,
            IBrandRepository brandRepository)
        {
            RepositoryContext = context;
            ProductRepository = productRepository;
            UserRepository = userRepository;
            BrandRepository = brandRepository;
        }

        public async Task SaveAsync()
        {
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
