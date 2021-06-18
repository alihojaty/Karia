

using System.ComponentModel.DataAnnotations;

namespace Karia.Api.Models
{
    public class StatisticsForUpdateDto
    {
        [Required]
        public int QuestionId { get; set; }
        
        [Required]
        public int Answer { get; set; }
    }
}