using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Question
    {
        public Question()
        {
            Surveys = new HashSet<Survey>();
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }

        public virtual ICollection<Survey> Surveys { get; set; }
    }
}
