using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Karia.Api.Entities;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Http;
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

        [HttpPost]
        public async Task<IActionResult> InsertEmployer(EmployerForCreationDto employerForCreationDto)
        {
            var employer = _mapper.Map<Employer>(employerForCreationDto);
            _kariaRepository.InsertEmployer(employer);
            
            if (employerForCreationDto.Profile.Length > 0)
            {
                var file = employerForCreationDto.Profile;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "Images/Employer", employerForCreationDto.Profile.FileName);
                using (var fileStream=new FileStream(filePath,FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                employer.ProfileImage = file.FileName;
            }
            await _kariaRepository.Save();

            return StatusCode(201);
        }

        [HttpPut("{employerId}")]
        public async Task<IActionResult> UpdateProfileImage(int employerId,
            [FromBody]IFormFile profile)
        {
            if (!await _kariaRepository.ExistsExpertAsync(employerId))
            {
                return NotFound();
            }
            
            if (profile.Length > 0)
            {
                
                var file = profile;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                    "Images/Employer", profile.FileName);
                // System.IO.File.Move("oldfilename", "newfilename");
                using (var fileStream=new FileStream(filePath,FileMode.CreateNew))
                {
                    await file.CopyToAsync(fileStream);
                }
                var employer=await _kariaRepository.GetEmployerAsync(employerId);
                employer.ProfileImage = file.FileName;
                await _kariaRepository.Save();

            }

            
            

            return NoContent();
        }
    }
}