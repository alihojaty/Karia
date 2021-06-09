using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Survey
    {
        public int Id { get; set; }
        public Guid? QuestionId { get; set; }
        public int? ExpertId { get; set; }
        public int? Positive { get; set; }
        public int? Negative { get; set; }

        public virtual Expert Expert { get; set; }
        public virtual Question Question { get; set; }
    }
}
