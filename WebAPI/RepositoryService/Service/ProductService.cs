using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Model;
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
            var brand = await _unitOfWork.BrandRepository.FindByCondition(brand => brand.Name.ToLower() == productModel.BrandName.ToLower()).FirstOrDefaultAsync();
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

            _unitOfWork.ProductRepository.CreateProduct(product);
            await _unitOfWork.SaveAsync();

            var prod = await _unitOfWork.ProductRepository.FindByCondition(index => index.Name == productModel.Name).FirstOrDefaultAsync();
            return _mapper.Map<ProductDTO>(prod);
        }

        private async Task<bool> IsProductNameExist(string name)
        {
            var checkProductNameExist = await _unitOfWork.ProductRepository.FindByCondition(product => product.Name == name)
                .FirstOrDefaultAsync();
            if (checkProductNameExist == null || checkProductNameExist.IsDeleted == true)
                return false;
            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository
                .FindByCondition(index => index.IsDeleted == false)
                .Include(index=>index.Brand)
                .ToListAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach (var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true) return null;
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<ProductDTO> UpdateProductAsync(ProductUpdateModel productModel)
        {
            if (await IsProductNameExist(productModel.Name)) return null;
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productModel.Id);
            if (product == null || product.IsDeleted == true) return null;

            if (!string.IsNullOrEmpty(productModel.Battery))
                product.Battery = productModel.Battery;

            if (!string.IsNullOrEmpty(productModel.BrandName))
            {
                var brand = await _unitOfWork.BrandRepository
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
            _unitOfWork.ProductRepository.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productModel.Id);
            return _mapper.Map<ProductDTO>(product);
        }

        public async Task<bool> DeleteProductAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true) return false;
            product.IsFeatured = false;
            product.IsDeleted = true;
            _unitOfWork.ProductRepository.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProductStockManager>> GetAllProductsWithStockAsync()
        {
            List<ProductStockManager> list = new List<ProductStockManager>();
            var products = await _unitOfWork.ProductRepository.FindByCondition(index => index.IsDeleted == false).ToListAsync();
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
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true)
                return null;
            return _mapper.Map<ProductStockManager>(product);
        }

        public async Task<ProductStockManager> UpdateProductWithStockAsync(ProductStockManager productStock)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productStock.Id);
            if (product == null || product.IsDeleted == true)
                return null;

            if (!string.IsNullOrEmpty(productStock.Status))
                product.Status = productStock.Status;

            if (productStock.Stock >= 0)
                product.Stock = productStock.Stock;

            _unitOfWork.ProductRepository.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productStock.Id);
            return _mapper.Map<ProductStockManager>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllFeatureProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository
                .FindByCondition(index => index.IsDeleted == false && index.IsFeatured == true)
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
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted == true)
                return false;

            product.IsFeatured = true;
            _unitOfWork.ProductRepository.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }
        
        public async Task<bool> RemoveFeatureProductByIdAsync(int id)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(id);
            if (product == null || product.IsDeleted == true)
                return false;

            product.IsFeatured = false;
            _unitOfWork.ProductRepository.DeleteProduct(product);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<IEnumerable<ProductPriceManager>> GetAllProductsWithPriceAsync()
        {
            var products = await _unitOfWork.ProductRepository
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
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null || product.IsDeleted == true)
                return null;
            return _mapper.Map<ProductPriceManager>(product);
        }

        public async Task<ProductPriceManager> UpdateProductWithPriceAsync(ProductPriceManager productPrice)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productPrice.Id);
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

            _unitOfWork.ProductRepository.UpdateProduct(product);
            await _unitOfWork.SaveAsync();
            product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productPrice.Id);
            return _mapper.Map<ProductPriceManager>(product);
        }

        public async Task<IEnumerable<ProductDTO>> GetAllSaleProductsAsync()
        {
            var products = await _unitOfWork.ProductRepository
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

        public async Task<IEnumerable<ProductDTO>> FindProductsByBrandName(string brandName)
        {
            var brand = await _unitOfWork.BrandRepository
                .FindByCondition(brand => brand.Name.ToLower() == brandName.ToLower())
                .FirstOrDefaultAsync();
            var products = await _unitOfWork.ProductRepository
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

        public async Task<IEnumerable<ProductDTO>> FindProductsByProductName(string productName)
        {
            var products = await _unitOfWork.ProductRepository
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



        //public async Task<string> Modify(IFormFile file)
        //{
        //    string folder = "product/";
        //    string name = "Xiaomi11T5G";
        //    ImageUploadResult result = await _uploadImage.UploadImage(file, name, folder) as ImageUploadResult;
        //    return result.Url.ToString();
        //}


    }
}
