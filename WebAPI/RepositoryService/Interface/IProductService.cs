using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Models;
using WebAPI.ModelDTO;

namespace WebAPI.RepositoryService.Interface
{
    public interface IProductService
    {
        public Task<IEnumerable<ProductDTO>> GetActiveProductsAsync(SortModel sortModel);
        public Task<ProductDTO> GetProductByIdAsync(int productId);

        public Task<IEnumerable<ProductDTO>> GetAllSellingProductsAsync();

        public Task<IEnumerable<ProductDTO>> GetAllProductsAsync();

        public Task<ProductDTO> CreateProductAsync(ProductModel productModel);

        //public Task<string> Modify(IFormFile file);

        public Task<ProductDTO> UpdateProductAsync(ProductUpdateModel productModel);

        public Task<bool> DeleteProductAsync(int productId);

        public Task<IEnumerable<ProductStockManager>> GetAllProductsWithStockAsync();

        public Task<ProductStockManager> GetProductWithStockByIdAsync(int productId);

        public Task<ProductStockManager> UpdateProductWithStockAsync(ProductStockManager productStock);

        public Task<IEnumerable<ProductDTO>> GetAllFeatureProductsAsync();

        public Task<bool> AddFeatureProductByIdAsync(int id);

        public Task<bool> RemoveFeatureProductByIdAsync(int id);

        public Task<IEnumerable<ProductPriceManager>> GetAllProductsWithPriceAsync();

        public Task<ProductPriceManager> GetProductWithPriceByIdAsync(int productId);

        public Task<ProductPriceManager> UpdateProductWithPriceAsync(ProductPriceManager productPrice);

        public Task<IEnumerable<ProductDTO>> GetAllSaleProductsAsync();

        public Task<IEnumerable<ProductDTO>> FindProductsByBrandNameAsync(string brandName);

        public Task<IEnumerable<ProductDTO>> FindProductsByProductNameAsync(string productName);

        public Task<IEnumerable<ProductDTO>> GetSimilarProductsAsync(int id);

        public Task<IEnumerable<ReviewDTO>> GetAllReviewsByProductIdAsync(int productId);

        public Task<bool> CreateReview(ReviewModel model);
        public Task<bool> CheckUserIdIfBuyProductId(int userId, int productId);

        public Task<bool> CheckUserBuyProductButNotReview(int userId, int productId);

        public Task<List<ReviewDTO>> GetAllReview();
    }
}
