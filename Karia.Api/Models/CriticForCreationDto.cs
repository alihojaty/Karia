using System.ComponentModel.DataAnnotations;

namespace Karia.Api.Models
{
    public class CriticForCreationDto
    {
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        [Required]
        [MaxLength(500)]
        public string Text { get; set; }
    }
}