using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Models;
using WebAPI.ModelDTO;

namespace WebAPI.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            //CreateMap<Product, ProductDTO>()
            //    .ForMember(destination => destination.BrandName,
            //    options => options.MapFrom(source => source))
            //    .ReverseMap();

            //CreateMap<ProductModel, Product>();

            //CreateMap<Product, ProductStockManager>();

            //CreateMap<Product, ProductPriceManager>();
        }
    }
}
