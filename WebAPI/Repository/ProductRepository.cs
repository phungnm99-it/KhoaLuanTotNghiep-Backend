using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.Repository.Interface;

namespace WebAPI.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(PTStoreContext context) : base(context) { }

        public void CreateProduct(Product product)
        {
            Create(product);
        }

        public void DeleteProduct(Product product)
        {
            Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await FindAll()
                .Include(product => product.Brand)
                .OrderBy(product => product.Id)
                .ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await FindByCondition(product => product.Id == productId)
                .Include(product => product.Brand)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetProductWithDetailsAsync(int productId)
        {
            return await FindByCondition(product => product.Id == productId)
                .Include(product => product.Brand)
                .Include(product => product.OrderDetails)
                .Include(product => product.Reviews)
                .FirstOrDefaultAsync();
        }

        public void UpdateProduct(Product product)
        {
            Update(product);
        }
    }
}
