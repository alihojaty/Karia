using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Models;
using Karia.Api.ResourceParameters;
using Karia.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CommentsController:ControllerBase
    {
        private readonly IKariaRepository _kariaRepository;
        private readonly IMapper _mapper;

        public CommentsController(IKariaRepository kariaRepository, IMapper mapper)
        {
            _kariaRepository = kariaRepository;
            _mapper = mapper;
        }

        [HttpGet("{expertId}")]
        public async Task<ActionResult<IEnumerable<CommentsDto>>> GetCommentsForExpert(int expertId,
            [FromQuery]CommentsResourceParameters commentsResourceParameters)
        {
            if (!await _kariaRepository.ExistsExpertAsync(expertId))
            {
                return NotFound();
            }
            var commentsFromRepo = await _kariaRepository.GetCommentsAsync(expertId,
                commentsResourceParameters);
            var paginationMetaData = new
            {
                currentPage=commentsFromRepo.CurrentPage,
                totalPages=commentsFromRepo.TotalPages
            };
            
            HttpContext.Response.Headers.Add("x-pagination",
                JsonSerializer.Serialize(paginationMetaData));
            return Ok(_mapper.Map<IEnumerable<CommentsDto>>(commentsFromRepo));
        }
    }
}