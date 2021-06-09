using System;
using System.Linq;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class ExpertsProfiles:Profile
    {
        public ExpertsProfiles()
        {
            CreateMap<Expert, ExpertsDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src =>
                        $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Orientation,
                    opt => opt.MapFrom(src =>
                        src.Orientation.Split(',', StringSplitOptions.None).ToList()));
        }
    }
}