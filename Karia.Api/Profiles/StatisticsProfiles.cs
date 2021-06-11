using System;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class StatisticsProfiles:Profile
    {
        public StatisticsProfiles()
        {
            CreateMap<Survey, StatisticsDto>()
                .ForMember(dest=>dest.Title,
                    opt=>opt.MapFrom(src=>src.Question.Title))
                .ForMember(dest => dest.Percent,
                    opt => opt.MapFrom(src=>
                        GetPercent(src.Positive.Value,src.Negative.Value)));
        }

        private double GetPercent(int value1, int value2)
        {
            try
            {
                var result = (double)((100 / (value1+value2)) * value1);
                return result ;
            }
            catch (Exception e)
            {
                return 0;
            }
        }
    }
}