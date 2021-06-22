using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Karia.Api.Models
{
    public class EmployerForCreationDto
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        public IFormFile Profile { get; set; }
        [Required]
        [MaxLength(11)]
        public string PhoneNumber { get; set; }
    }
}