using AutoMapper;
using CloudinaryDotNet.Actions;
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
    public class BrandService : IBrandService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;
        private IUploadImage _uploadImage;
        public BrandService(IUnitOfWork unitOfWork, IMapper mapper, IUploadImage uploadImageUtils)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _uploadImage = uploadImageUtils;
        }
        public async Task<BrandDTO> CreateBrandAsync(BrandModel brandModel)
        {
            if (await IsBrandNameExist(brandModel.Name))
                return null;
            Brand brand = new Brand();
            brand.Name = brandModel.Name;
            string folder = "brand/";
            ImageUploadResult result = await _uploadImage.UploadImage(brandModel.Image, brandModel.Name, folder) as ImageUploadResult;
            brand.ImageUrl = result.Url.ToString();
            brand.IsDeleted = false;
            _unitOfWork.Brands.CreateBrand(brand);
            await _unitOfWork.SaveAsync();
            brand = await _unitOfWork.Brands.FindByCondition(index => index.Name == brandModel.Name).FirstOrDefaultAsync();
            return _mapper.Map<BrandDTO>(brand);
        }

        public async Task<IEnumerable<BrandDTO>> GetAllBrandsAsync()
        {
            var listBrands = await _unitOfWork.Brands.FindByCondition(index => index.IsDeleted == false).ToListAsync();
            List<BrandDTO> list = new List<BrandDTO>();
            foreach(var brand in listBrands)
            {
                list.Add(_mapper.Map<BrandDTO>(brand));
            }

            return list;
        }

        public async Task<BrandDTO> GetBrandByIdAsync(int brandId)
        {
            var brand = await _unitOfWork.Brands.GetBrandByIdAsync(brandId);
            if (brand == null || brand.IsDeleted == true) return null;
            return _mapper.Map<BrandDTO>(brand);
        }

        public async Task<BrandDTO> UpdateBrandAsync(BrandUpdateModel brandModel)
        {
            if (await IsBrandNameExist(brandModel.Name))
                return null;
            var brand = await _unitOfWork.Brands.GetBrandByIdAsync(brandModel.Id);
            if (brand == null || brand.IsDeleted == true)
                return null;

            brand.Name = brandModel.Name;
            if(brandModel.Image != null)
            {
                string folder = "brand/";
                ImageUploadResult result = await _uploadImage.UploadImage(brandModel.Image, brandModel.Name, folder) as ImageUploadResult;
                brand.ImageUrl = result.Url.ToString();
            }
            _unitOfWork.Brands.UpdateBrand(brand);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<BrandDTO>(brand);
        }


        public async Task<bool> IsBrandNameExist(string brandName)
        {
            var checkBrandNameExist = await _unitOfWork.Brands.FindByCondition(index => index.Name == brandName)
                .FirstOrDefaultAsync();
            if (checkBrandNameExist == null)
                return false;
            return true;
        }

        //public async Task<bool> DeleteBrandAsync(int brandId)
        //{
        //    var brand = await _unitOfWork.Brands.GetBrandByIdAsync(brandId);
        //    if (brand == null || brand.IsDeleted == true)
        //        return false;
        //    brand.IsDeleted = true;
        //    _unitOfWork.Brands.DeleteBrand(brand);
        //    await _unitOfWork.SaveAsync();
        //    return true;
        //}

        //public async Task<bool> RestoreBrandAsync(int brandId)
        //{
        //    var brand = await _unitOfWork.Brands.GetBrandByIdAsync(brandId);
        //    if (brand == null || brand.IsDeleted == false)
        //        return false;
        //    brand.IsDeleted = false;
        //    _unitOfWork.Brands.DeleteBrand(brand);
        //    await _unitOfWork.SaveAsync();
        //    return true;
        //}

        public async Task<IEnumerable<BrandDTO>> GetActiveBrandAsync()
        {
            var brands = await _unitOfWork.Brands.FindByCondition(brand => brand.Products.Count > 0
            && brand.Products.Any(pr => pr.Stock < 1) == false).ToListAsync();
            List<BrandDTO> list = new List<BrandDTO>();
            foreach (var brand in brands)
            {
                list.Add(_mapper.Map<BrandDTO>(brand));
            }

            return list;
        }

        public async Task<List<BrandDTO>> SearchBrandAsync(string name)
        {
            var listBrands = await _unitOfWork.Brands.FindByCondition(index => index.IsDeleted == false
            && index.Name.ToLower().Contains(name.ToLower())).ToListAsync();
            List<BrandDTO> list = new List<BrandDTO>();
            foreach (var brand in listBrands)
            {
                list.Add(_mapper.Map<BrandDTO>(brand));
            }

            return list;
        }
    }
}
