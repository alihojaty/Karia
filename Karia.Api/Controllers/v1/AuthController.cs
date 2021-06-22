using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Karia.Api.Helpers;
using Karia.Api.Models;
using Karia.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Karia.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class AuthController:ControllerBase
    {
        private readonly IKariaServices _kariaServices;
        private readonly IKariaRepository _kariaRepository;

        public AuthController(IKariaServices kariaServices, IKariaRepository kariaRepository)
        {
            _kariaServices = kariaServices;
            _kariaRepository = kariaRepository;
        }

        [HttpPost("sendPhoneNumber")]
        public async Task<IActionResult> ConfirmPhoneNumber([FromBody] ConfirmPhoneNumberDto phoneNumberDto)
        {
            var code = GenerateRandomNumber.GetCode();
            if (!await SendSms.Send(phoneNumberDto.PhoneNumber, code.ToString()))
            {
                return BadRequest();
            }

            if (!await _kariaServices.SetCode(phoneNumberDto.PhoneNumber, code))
            {
                return BadRequest();
            }

            
            
            return NoContent();
        }

        [HttpPost("confirmCode")]
        public async Task<IActionResult> ConfirmVerificationCode(ConfirmVerificationCodeDto confirmVerificationCodeDto)
        {
            // var code = await _kariaServices.GetCode(confirmVerificationCodeDto.PhoneNumber);
            // if (code.Equals(confirmVerificationCodeDto.Code))
            // {
            //     return Unauthorized();
            // }





            var claims = new List<Claim>
            {
                new Claim ("UserId", confirmVerificationCodeDto.PhoneNumber),

            };
            string key = "79322475-2517-4695-b8f7-e570169ce17d";
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "Karia.ir",
                audience: "Karia.ir",

                notBefore: DateTime.Now,
                claims: claims,
                signingCredentials: credentials
            );
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);



                


            if (await _kariaRepository.ExistsEmployerByPhoneNumber(confirmVerificationCodeDto.PhoneNumber))
            {
                return Ok(new AuthenticationResultDto()
                {
                    Token = jwtToken,
                    Type = Enum.GetName(typeof(AuthType), 1)
                });
            }
            return Ok(new AuthenticationResultDto()
            {
                Token = jwtToken,
                Type = Enum.GetName(typeof(AuthType), 0)
            });
        }
        
        
    }
}