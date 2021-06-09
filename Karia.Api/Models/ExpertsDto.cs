using System.Collections.Generic;

namespace Karia.Api.Models
{
    public class ExpertsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public List<string> Orientation { get; set; }
        public decimal Rate { get; set; }
        public bool IsMaster { get; set; }
        public bool IsHaveVehicle { get; set; }
        
    }
}