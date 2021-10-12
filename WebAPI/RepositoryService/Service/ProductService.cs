﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Model;
using WebAPI.ModelDTO;
using WebAPI.RepositoryService.Interface;
using WebAPI.UnitOfWorks;

namespace WebAPI.RepositoryService.Service
{
    public class ProductService : IProductService
    {
        private IUnitOfWork _unitOfWork;
        private IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CreateProductAsync(ProductDTO product)
        {
            Product prd = new Product();
            prd = _mapper.Map<Product>(product);
            prd.Id = 1;
            prd.Stock = 0;
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



    }
}