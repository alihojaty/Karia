using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Grouping
    {
        public int Id { get; set; }
        public int? ExpertId { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual Expert Expert { get; set; }
    }
}
