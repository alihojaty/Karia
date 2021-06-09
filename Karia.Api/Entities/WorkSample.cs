using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class WorkSample
    {
        public int Id { get; set; }
        public int? ExpertId { get; set; }
        public string SamplePhoto { get; set; }

        public virtual Expert Expert { get; set; }
    }
}
