using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class FeedbacksController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public FeedbacksController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }
        
        [HttpPost("{employerId}")]
        public async Task<IActionResult> InsertFeedback(int employerId,
            CriticForCreationDto criticForCreationDto)
        {
            if (!await _kariaRepository.ExistsEmployerAsync(employerId))
            {
                return NotFound();
            }

            var critic = _mapper.Map<Critic>(criticForCreationDto);
            critic.EmployerId = employerId;
            
            _kariaRepository.InsertFeedback(critic);

            if (!await _kariaRepository.Save())
            {
                return BadRequest();
            }

            return StatusCode(201);
        }
    }
}