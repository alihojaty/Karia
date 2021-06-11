using System;
using System.Linq;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Helpers;
using Karia.Api.Models;
using Newtonsoft.Json.Serialization;

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
                .ForMember(dest => dest.Orientations,
                    opt => opt.MapFrom(src =>
                        src.Orientation.Split(',', StringSplitOptions.None).ToList()));

            CreateMap<Expert, ExpertDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src =>
                        $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Age,
                    opt => opt.MapFrom(src => src.Birthyear.GetCurrentAge()))
                .ForMember(dest => dest.Orientations,
                    opt => opt.MapFrom(src => src.Orientation.Split(',', StringSplitOptions.None).ToList()))
                .ForMember(dest => dest.NumberOfOffers,
                    opt => opt.MapFrom(src => src.Offers))
                .ForMember(dest => dest.IsHasVehicle,
                    opt => opt.MapFrom(src => src.IsHaveVehicle));
        }
    }
}