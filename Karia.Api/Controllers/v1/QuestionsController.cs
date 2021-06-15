using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class QuestionsController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public QuestionsController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<QuestionDto>>> GetQuestions()
        {
            var questionFromRepo = await _kariaRepository.GetQuestionsAsync();

            return Ok(_mapper.Map<IEnumerable<QuestionDto>>(questionFromRepo));
        }
    }
}