using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Karia.Api.Models
{
    public class EmployerForUpdateDto
    {
        [MaxLength(50)] 
        public string FirstName { get; set; }
        [MaxLength(50)]     
        public string LastName { get; set; }
    }
}