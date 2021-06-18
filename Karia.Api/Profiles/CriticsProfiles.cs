using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class CriticsProfiles:Profile
    {
        public CriticsProfiles()
        {
            CreateMap<CriticForCreationDto, Critic>();
        }
    }
}