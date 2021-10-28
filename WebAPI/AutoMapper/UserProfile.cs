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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(destionation => destionation.RoleName,
                option => option.MapFrom(source => source.Role.Name))
                .ReverseMap();

            CreateMap<RegisterModel, User>();
        }
    }
}
