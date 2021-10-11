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

        public UnitOfWork(PTStoreContext context,
            IProductRepository productRepository)
        {
            RepositoryContext = context;
            ProductRepository = productRepository;
        }

        public async Task SaveAsync()
        {
            await RepositoryContext.SaveChangesAsync();
        }
    }
}
