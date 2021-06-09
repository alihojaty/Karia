using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;
using Karia.Api.ResourceParameters;
using Karia.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/categories/{categoryId}/[controller]")]
    public class ExpertsController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public ExpertsController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExpertsDto>>> GetExpertsForCategories(Guid categoryId,
            [FromQuery] ExpertsResourceParameters expertsResourceParameters)
        {
            if (!await _kariaRepository.ExistsCategory(categoryId))
            {
                return NotFound();
            }
            var expertsFromRepo =await _kariaRepository.GetExperts(categoryId, expertsResourceParameters);

            return Ok(_mapper.Map<IEnumerable<ExpertsDto>>(expertsFromRepo));

        }
        
    }
}