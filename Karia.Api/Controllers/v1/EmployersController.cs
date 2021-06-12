using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployersController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public EmployersController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }
        
        [HttpGet("{employerId}")]
        public async Task<ActionResult<EmployerDto>> GetEmployer(int employerId)
        {
            var employerFromRepo = await _kariaRepository.GetEmployerAsync(employerId);
            if (employerFromRepo is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EmployerDto>(employerFromRepo));
        }
    }
}