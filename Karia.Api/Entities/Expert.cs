using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Expert
    {
        public Expert()
        {
            Commentings = new HashSet<Commenting>();
            Groupings = new HashSet<Grouping>();
            Surveys = new HashSet<Survey>();
            WorkSamples = new HashSet<WorkSample>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public string Description { get; set; }
        public string Orientation { get; set; }
        public DateTime Birthyear { get; set; }
        public DateTime? RegisterDate { get; set; }
        public int? Offers { get; set; }
        public decimal? Rate { get; set; }
        public decimal? Scores { get; set; }
        public int? Count { get; set; }
        public bool? IsValid { get; set; }
        public bool? IsMaster { get; set; }
        public bool? IsHasVehicle { get; set; }

        public virtual ICollection<Commenting> Commentings { get; set; }
        public virtual ICollection<Grouping> Groupings { get; set; }
        public virtual ICollection<Survey> Surveys { get; set; }
        public virtual ICollection<WorkSample> WorkSamples { get; set; }
    }
}
