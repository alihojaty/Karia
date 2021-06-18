using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.JsonPatch;
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

        [HttpPatch("{employerId}")]
        public async Task<ActionResult> PartiallyUpdateEmployer(int employerId,
            JsonPatchDocument<EmployerForUpdateDto> patchDocument)
        {
            var employerFromRepo = await _kariaRepository.GetEmployerAsync(employerId);
            if (employerFromRepo is null)
            {
                return NotFound();
            }
            
            var employerToPatch = _mapper.Map<EmployerForUpdateDto>(employerFromRepo);
            
            patchDocument.ApplyTo(employerToPatch);

            if (!TryValidateModel(employerToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(employerToPatch, employerFromRepo);
            
            _kariaRepository.UpdateEmployerAsync(employerFromRepo);
            await _kariaRepository.Save();

            return NoContent();

        }
    }
}