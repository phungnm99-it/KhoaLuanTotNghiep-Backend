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
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDTO>()
                .ReverseMap();

            CreateMap<BrandModel, Brand>();
        }
    }
}
