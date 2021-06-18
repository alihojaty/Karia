using System.ComponentModel.DataAnnotations;

namespace Karia.Api.Models
{
    public class CommentForCreationDto
    {
        
        [Required(ErrorMessage = "The EmployerId field is required.")]
        public int?  EmployerId { get; set; }
        
        [Required]
        [MaxLength(500)]
        public string Comment { get; set; }
    }
}