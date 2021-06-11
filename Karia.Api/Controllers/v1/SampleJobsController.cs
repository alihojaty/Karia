using System.Collections.Generic;
using System.Linq;
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
    public class SampleJobsController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;


        public SampleJobsController(IKariaRepository kariaRepository)
        {
            _kariaRepository = kariaRepository;

        }

        [HttpGet("{expertId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetSampleJobsForExpert(int expertId)
        {
            if (! await _kariaRepository.ExistsExpertAsync(expertId))
            {
                return NotFound();
            }
            var sampleJobsFromRepo = await _kariaRepository.GetSampleJobsAsync(expertId);


            return Ok(new {sampleJobs=sampleJobsFromRepo.Select(s => s.SamplePhoto)});
        }
        
    }
}