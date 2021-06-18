using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/{expertId}")]
    public class StatisticsController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public StatisticsController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatisticsDto>>> GetStatisticsForExpert(int expertId)
        {
            var statisticsFromRepo = await _kariaRepository.GetPollStatisticsAsync(expertId);

            if (statisticsFromRepo is null)
            {
                return NotFound();
            }

            var statisticsDto = _mapper.Map<IEnumerable<StatisticsDto>>(statisticsFromRepo);
            
            
            
            return Ok(statisticsDto);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateStatistics(int expertId,
            [FromBody] List<StatisticsForUpdateDto> statisticsForUpdateDto)
        {
            if (!await _kariaRepository.ExistsExpertAsync(expertId))
            {
                return NotFound();
            }
            
            foreach (var stat in statisticsForUpdateDto)
            {
                await _kariaRepository.UpdateStatisticsAsync(expertId,stat);
            }

            return StatusCode(204);
        }
        
    }
}