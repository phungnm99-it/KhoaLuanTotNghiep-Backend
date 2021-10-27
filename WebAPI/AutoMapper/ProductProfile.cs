using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.DataModel;
using WebAPI.Model;
using WebAPI.ModelDTO;

namespace WebAPI.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(destination => destination.BrandName,
                options => options.MapFrom(source => source.Brand.Name))
                .ReverseMap();

            CreateMap<ProductModel, Product>();
        }
    }
}
