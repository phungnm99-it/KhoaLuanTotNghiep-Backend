using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPI.Models;
using WebAPI.ModelDTO;

namespace WebAPI.AutoMapper
{
    public class FeedbackProfile : Profile
    {
        public FeedbackProfile()
        {
            CreateMap<Feedback, FeedbackDTO>()
                .ForMember(des => des.ReplierName, opt => opt.MapFrom(src => src.RepliedByNavigation.FullName));
        }
    }
}
