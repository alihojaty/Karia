using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
        public async Task<ActionResult<IEnumerable<ExpertsDto>>> GetExpertsForCategories(int categoryId,
            [FromQuery] ExpertsResourceParameters expertsResourceParameters)
        {
            if (!await _kariaRepository.ExistsCategoryAsync(categoryId))
            {
                return NotFound();
            }
            var expertsFromRepo =await _kariaRepository.GetExpertsAsync(categoryId, expertsResourceParameters);
            
            if (expertsFromRepo is null)
            {
                return NotFound();
            }

            
            var paginationMetaData = new
            {
                currentPage=expertsFromRepo.CurrentPage,
                totalPages=expertsFromRepo.TotalPages
            };
            
            HttpContext.Response.Headers.Add("x-pagination",
                JsonSerializer.Serialize(paginationMetaData));
                
            return Ok(_mapper.Map<IEnumerable<ExpertsDto>>(expertsFromRepo));

        }

        [HttpGet("{expertId}")]
        public async Task<ActionResult<ExpertDto>> GetExpertForCategories(int categoryId,
            int expertId)
        {
            if (!await _kariaRepository.ExistsCategoryAsync(categoryId))
            {
                return NotFound();
            }

            var expertFormRepo = await _kariaRepository.GetExpertAsync(categoryId, expertId);
            if (expertFormRepo is null)
            {
                return NotFound();
            }
            var expertDto = _mapper.Map<ExpertDto>(expertFormRepo);
            expertDto.TotalComments = await _kariaRepository.GetTotalCommentsForExperts(expertId);

            return Ok(expertDto);
        }
        
        
        
        
        
    }
}