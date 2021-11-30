using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Models;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;
using WebAPI.UploadImageUtils;

namespace WebAPI.RepositoryService.Service
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IUploadImage _uploadImage;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper, IUploadImage uploadImageUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadImage = uploadImageUtils;
        }

        public async Task<ProductDTO> CreateProductAsync(ProductModel productModel)
        {
            if (await IsProductNameExist(productModel.Name))
                return null;
            var brand = await _unitOfWork.Brands.FindByCondition(brand => brand.Name.ToLower() == productModel.BrandName.ToLower()).FirstOrDefaultAsync();
            Product product = new Product();
            product = _mapper.Map<Product>(productModel);
            product.BrandId = brand.Id;
            product.Price = 0;
            product.CurrentPrice = 0;
            product.IsSale = false;
            product.Stock = 0;
            product.IsDeleted = false;
            product.IsFeatured = false;

            if (productModel.Image != null)
            {
                string folder = "product/";
                string name = productModel.Name.Replace(" ", "");
                ImageUploadResult result = await _uploadImage.UploadImage(productModel.Image, name, folder) as ImageUploadResult;
                product.ImageUrl = result.Url.ToString();
            }

            _unitOfWork.Products.CreateProduct(product);
            await _unitOfWork.SaveAsync();

            var prod = await _unitOfWork.Products.FindByCondition(index => index.Name == productModel.Name).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(prod);
        }

        private async Task<bool> IsProductNameExist(string name)
        {
            var checkProductNameExist = await _unitOfWork.Products.FindByCondition(product => product.Name == name)
                .FirstOrDefaultAsync();
            if (checkProductNameExist == null || checkProductNameExist.IsDeleted == true)
                return false;
            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllSellingProductsAsync()
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.IsDeleted == false
                && (!index.Status.Equals("Ngừng kinh doanh")))
                .Include(index=>index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(mapToProductDTO(product));
            }
            return list;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true) return null;
            return mapToProductDTO(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductUpdateModel productModel)
        {
            if (await IsProductNameExist(productModel.Name)) return null;
            var product = await _unitOfWork.Products.GetProductByIdAsync(productModel.Id);
            if (product == null || product.IsDeleted == true) return null;

            if (!string.IsNullOrEmpty(productModel.Battery))
                product.Battery = productModel.Battery;

            if (!string.IsNullOrEmpty(productModel.BrandName))
            {
                var brand = await _unitOfWork.Brands
                    .FindByCondition(brand => brand.Name.ToLower() == productModel.BrandName.ToLower())
                    .FirstOrDefaultAsync();
                product.BrandId = brand.Id;
            }

            if (!string.IsNullOrEmpty(productModel.Color))
                product.Color = productModel.Color;

            if (!string.IsNullOrEmpty(productModel.BackCamera))
                product.BackCamera = productModel.BackCamera;

            if (!string.IsNullOrEmpty(productModel.Cpu))
                product.Cpu = productModel.Cpu;

            if (!string.IsNullOrEmpty(productModel.FrontCamera))
                product.FrontCamera = productModel.FrontCamera;

            if (!string.IsNullOrEmpty(productModel.Gps))
                product.Gps = productModel.Gps;

            if (!string.IsNullOrEmpty(productModel.Gpu))
                product.Gpu = productModel.Gpu;

            if (!string.IsNullOrEmpty(productModel.Name))
                product.Name = productModel.Name;


            if (!string.IsNullOrEmpty(productModel.Os))
                product.Os = productModel.Os;

            if (!string.IsNullOrEmpty(productModel.Ram))
                product.Ram = productModel.Ram;

            if (!string.IsNullOrEmpty(productModel.Rom))
                product.Rom = productModel.Rom;

            if (!string.IsNullOrEmpty(productModel.ScreenResolution))
                product.ScreenResolution = productModel.ScreenResolution;

            if (!string.IsNullOrEmpty(productModel.ScreenSize))
                product.ScreenSize = productModel.ScreenSize;

            if (!string.IsNullOrEmpty(productModel.ScreenTech))
                product.ScreenTech = productModel.ScreenTech;

            if (!string.IsNullOrEmpty(productModel.Sim))
                product.Sim = productModel.Sim;

            if (!string.IsNullOrEmpty(productModel.Wifi))
                product.Wifi = productModel.Wifi;

            if (productModel.Image != null)
            {
                string folder = "product/";
                string name = productModel.Name.Replace(" ", "");
                ImageUploadResult result = await _uploadImage.UploadImage(productModel.Image, name, folder) as ImageUploadResult;
                product.ImageUrl = result.Url.ToString();
            }
            _unitOfWork.Products.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.Products.GetProductByIdAsync(productModel.Id);
            return mapToProductDTO(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true) return false;
            product.IsFeatured = false;
            product.IsDeleted = true;
            _unitOfWork.Products.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProductStockManager>> GetAllProductsWithStockAsync()
        {
            List<ProductStockManager> list = new List<ProductStockManager>();
            var products = await _unitOfWork.Products.FindByCondition(index => index.IsDeleted == false).ToListAsync();
            foreach(var product in products)
            {
                ProductStockManager pr = new ProductStockManager();
                pr = _mapper.Map<ProductStockManager>(product);
                list.Add(pr);
            }
            return list;
        }

        public async Task<ProductStockManager> GetProductWithStockByIdAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true)
                return null;
            return _mapper.Map<ProductStockManager>(product);
        }

        public async Task<ProductStockManager> UpdateProductWithStockAsync(ProductStockManager productStock)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productStock.Id);
            if (product == null || product.IsDeleted == true)
                return null;

            if (!string.IsNullOrEmpty(productStock.Status))
                product.Status = productStock.Status;

            if (productStock.Stock >= 0)
                product.Stock = productStock.Stock;

            _unitOfWork.Products.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.Products.GetProductByIdAsync(productStock.Id);
            return _mapper.Map<ProductStockManager>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllFeatureProductsAsync()
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.IsDeleted == false && index.IsFeatured == true).Take(8)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<bool> AddFeatureProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted == true)
                return false;

            product.IsFeatured = true;
            _unitOfWork.Products.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }
        
        public async Task<bool> RemoveFeatureProductByIdAsync(int id)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted == true)
                return false;

            product.IsFeatured = false;
            _unitOfWork.Products.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProductPriceManager>> GetAllProductsWithPriceAsync()
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.IsDeleted == false)
                .ToListAsync();
            List<ProductPriceManager> list = new List<ProductPriceManager>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductPriceManager>(product));
            }
            return list;
        }

        public async Task<ProductPriceManager> GetProductWithPriceByIdAsync(int productId)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true)
                return null;
            return _mapper.Map<ProductPriceManager>(product);
        }

        public async Task<ProductPriceManager> UpdateProductWithPriceAsync(ProductPriceManager productPrice)
        {
            var product = await _unitOfWork.Products.GetProductByIdAsync(productPrice.Id);
            if (product == null || product.IsDeleted == true)
                return null;

            if (productPrice.Price > 0)
                product.Price = productPrice.Price;

            if (productPrice.CurrentPrice > 0 && productPrice.CurrentPrice < productPrice.Price)
            {
                product.IsSale = true;
                product.CurrentPrice = productPrice.CurrentPrice;
            }
            else
            {
                product.CurrentPrice = productPrice.Price;
                product.IsSale = false;
            }

            _unitOfWork.Products.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.Products.GetProductByIdAsync(productPrice.Id);
            return _mapper.Map<ProductPriceManager>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllSaleProductsAsync()
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.IsDeleted == false && index.IsSale == true)
                .Include(index => index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<IEnumerable<ProductDTO>> FindProductsByBrandNameAsync(string brandName)
        {
            var brand = await _unitOfWork.Brands
                .FindByCondition(brand => brand.Name.ToLower() == brandName.ToLower())
                .FirstOrDefaultAsync();
            if (brand == null) return null;
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.BrandId == brand.Id)
                .Include(index => index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<IEnumerable<ProductDTO>> FindProductsByProductNameAsync(string productName)
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.Name.ToLower().Contains(productName.ToLower()))
                .Include(index => index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.Products
                .FindByCondition(index => index.IsDeleted == false)
                .Include(index => index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        private ProductDTO mapToProductDTO(Product product) => new ProductDTO
        {
            Id = product.Id,
            Name = product.Name,
            CurrentPrice = product.CurrentPrice,
            ImageUrl = product.ImageUrl,
            Status = product.Status,
            Price = product.Price,
            Color = product.Color,
            ScreenTech = product.ScreenTech,
            Ram = product.Ram,
            Rom = product.Rom,
            Cpu = product.Cpu,
            ScreenResolution = product.ScreenResolution,
            ScreenSize = product.ScreenSize,
            BackCamera = product.BackCamera,
            FrontCamera = product.FrontCamera,
            BrandName = product.Brand.Name,
            Os = product.Os,
            Gpu = product.Gpu,
            Battery = product.Battery,
            Sim = product.Sim,
            Wifi = product.Wifi,
            Gps = product.Gps,
            IsFeatured = product.IsFeatured.Value,
            IsSale = product.IsSale.Value,
            Stock = product.Stock.GetValueOrDefault()
            
        };

        public async Task<IEnumerable<ProductDTO>> GetSimilarProductsAsync(int id)
        {
            var products = await _unitOfWork.Products.FindByCondition(product => product.IsDeleted == false
            && product.Stock > 0).Include(index => index.Brand).OrderBy(product => product.Price).ToListAsync();
            int index = products.FindIndex(product => product.Id == id);
            List<ProductDTO> list = new List<ProductDTO>();
            if(index == 0)
            {
                for(int i = 1; i<5; i++)
                {
                    list.Add(_mapper.Map<ProductDTO>(products[i]));
                }
            }
            else
            if(index == products.Count - 1)
            {
                for (int i = index - 1; i > index - 5; i--)
                {
                    list.Add(_mapper.Map<ProductDTO>(products[i]));
                }
            }
            else
            {
                list.Add(_mapper.Map<ProductDTO>(products[index - 2]));
                list.Add(_mapper.Map<ProductDTO>(products[index - 1]));
                list.Add(_mapper.Map<ProductDTO>(products[index + 1]));
                list.Add(_mapper.Map<ProductDTO>(products[index + 2]));
            }

            return list;
            
        }

        public async Task<IEnumerable<ProductDTO>> GetActiveProductsAsync(SortModel sortModel)
        {
            var products = _unitOfWork.Products.FindByCondition(product => product.IsDeleted == false
            && product.Stock > 0);
            List<ProductDTO> list = new List<ProductDTO>();
            if(!string.IsNullOrEmpty(sortModel.BrandName))
            {
                var brand = await _unitOfWork.Brands.FindByCondition(brand => brand.Name.ToLower().Contains(sortModel.BrandName.ToLower())).FirstOrDefaultAsync();
                products = products.Where(product => product.BrandId == brand.Id);
            }
            if(!string.IsNullOrEmpty(sortModel.OrderType))
            {
                if (sortModel.OrderType == "a")
                    products = products.OrderBy(product => product.Price);
                else
                    products = products.OrderByDescending(product => product.Price);
            }
            List<Product> productList = await products.Include(product => product.Brand).ToListAsync();
            if(productList.Count < 12)
            {
                foreach(var item in productList)
                {
                    list.Add(_mapper.Map<ProductDTO>(item));
                }
                return list;
            }
            if (sortModel.Page != 0)
            {
                for (int i = (sortModel.Page - 1) * 12; i < sortModel.Page * 12 || i > products.Count(); i++)
                    list.Add(_mapper.Map<ProductDTO>(productList[i]));
            }
            return list;
        }

        public async Task<IEnumerable<ReviewDTO>> GetAllReviewsByProductIdAsync(int productId)
        {
            var reviews = await _unitOfWork.Reviews.GetAllReviewsByProductId(productId);
            if (reviews == null)
                return null;
            List<ReviewDTO> rev = new List<ReviewDTO>();
            foreach(var item in reviews)
            {
                rev.Add(_mapper.Map<ReviewDTO>(item));
            }
            return rev;
        }

        public async Task<bool> CheckUserIdIfBuyProductId(int userId, int productId)
        {
            var orderByProductId = await _unitOfWork.OrderDetails.GetOrderDetailByProductId(productId);
            var check = orderByProductId.Where(or => or.Order.UserId == userId && or.Order.IsCompleted == true);
            if (check == null || check.Count() == 0)
                return false;
            return true;
        }

        public async Task<bool> CreateReview(ReviewModel model)
        {
            try
            {
                var check = await CheckUserIdIfBuyProductId(model.UserId, model.ProductId);
                if (check == false)
                    return false;
                var r = await _unitOfWork.Reviews.GetAllOwnReviews(model.UserId);
                r = r.Where(va => va.ProductId == model.ProductId);
                if (r.Count() != 0)
                    return false;
                Review rv = new Review()
                {
                    ProductId = model.ProductId,
                    UserId = model.UserId,
                    Content = model.Content,
                    ReviewTime = DateTime.Now
                };
                _unitOfWork.Reviews.CreateReview(rv);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        //public async Task<string> Modify(IFormFile file)
        //{
        //    string folder = "product/";
        //    string name = "Xiaomi11T5G";
        //    ImageUploadResult result = await _uploadImage.UploadImage(file, name, folder) as ImageUploadResult;
        //    return result.Url.ToString();
        //}


    }
}
