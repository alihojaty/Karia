using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class CommentsProfiles:Profile
    {
        public CommentsProfiles()
        {
            CreateMap<Commenting, CommentsDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src
                        => $"{src.Employer.FirstName} {src.Employer.LastName}"))
                .ForMember(dest => dest.Profile,
                    opt => opt.MapFrom(src
                        => src.Employer.ProfileImage))
                .ForMember(dest=>dest.Text,
                    opt=>opt.MapFrom(src
                        =>src.Comment));

        }
    }
}