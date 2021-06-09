using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Critic
    {
        public int Id { get; set; }
        public int? EmployerId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public virtual Employer Employer { get; set; }
    }
}
