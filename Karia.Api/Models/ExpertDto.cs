using System;
using System.Collections.Generic;

namespace Karia.Api.Models
{
    public class ExpertDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Profile { get; set; }
        public int Age { get; set; }
        public List<string> Orientations { get; set; }
        public decimal Rate { get; set; }
        public int NumberOfOffers { get; set; }
        public DateTime RegisterDate { get; set; }
        public string Description { get; set; }
        public bool IsMaster { get; set; }
        public bool IsHasVehicle { get; set; }
        public int TotalComments { get; set; }
    }
}