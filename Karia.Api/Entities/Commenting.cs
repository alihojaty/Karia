using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Commenting
    {
        public int Id { get; set; }
        public int? EmployerId { get; set; }
        public int? ExpertId { get; set; }
        public string Comment { get; set; }
        public DateTime DateOfRegistration { get; set; }
        public bool? IsValid { get; set; }

        public virtual Employer Employer { get; set; }
        public virtual Expert Expert { get; set; }
    }
}
