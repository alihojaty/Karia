using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;

namespace Karia.Api.Profiles
{
    public class QuestionsProfiles:Profile
    {
        public QuestionsProfiles()
        {
            CreateMap<Question, QuestionDto>();
        }
    }
}