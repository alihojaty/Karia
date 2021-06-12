using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class EmployersProfiles:Profile
    {
        public EmployersProfiles()
        {
            CreateMap<Employer, EmployerDto>()
                .ForMember(dest => dest.Profile,
                    opt => opt.MapFrom(src
                        => src.ProfileImage));
        }
    }
}