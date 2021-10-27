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
            if (checkProductNameExist == null)
                return false;
            return true;
        }

        public async Task<IEnumerable<ProductDTO>> GetAllProductAsync()
        {
            var products = await _unitOfWork.ProductRepository.GetAllProductsAsync();
            List<ProductDTO> list = new List<ProductDTO>();
            foreach(var product in products)
            {
                list.Add(_mapper.Map<ProductDTO>(product));
            }
            return list;
        }

        public async Task<ProductDTO> GetProductByIdAsync(int productId)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            return _mapper.Map<ProductDTO>(product);
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
