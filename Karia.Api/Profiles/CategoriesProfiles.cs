using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class CategoriesProfiles:Profile
    {
        public CategoriesProfiles()
        {
            CreateMap<Category, CategoryDto>()
                .ForMember(dest=>dest.Title,
                    opt=>opt.MapFrom(src=>src.Name));
        }
    }
}