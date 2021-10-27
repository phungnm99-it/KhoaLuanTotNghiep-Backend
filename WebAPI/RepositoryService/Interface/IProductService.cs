using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Model;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IProductService
    {
        public Task<ProductDTO> GetProductByIdAsync(int productId);

        public Task<IEnumerable<ProductDTO>> GetAllProductAsync();

        public Task<ProductDTO> CreateProductAsync(ProductModel productModel);

        //public Task<string> Modify(IFormFile file);

        public Task<ProductDTO> UpdateProductAsync();
    }
}
