using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.ModelDTO;
using WebAPI.Models;

namespace WebAPI.AutoMapper
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<Review, ReviewDTO>()
                .ForMember(des => des.ProductName, opt => opt.MapFrom(value => value.Product.Name))
                .ForMember(des => des.ImageUrl, opt => opt.MapFrom(value => value.User.ImageUrl))
                .ForMember(des => des.UserName, opt => opt.MapFrom(value => value.User.FullName));
            
        }
    }
}
