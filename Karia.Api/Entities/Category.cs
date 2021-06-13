using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Category
    {
        public Category()
        {
            Groupings = new HashSet<Grouping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }

        public virtual ICollection<Grouping> Groupings { get; set; }
    }
}
