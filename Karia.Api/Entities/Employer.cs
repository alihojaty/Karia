using System;
using System.Collections.Generic;

#nullable disable

namespace Karia.Api.Entities
{
    public partial class Employer
    {
        public Employer()
        {
            Commentings = new HashSet<Commenting>();
            Critics = new HashSet<Critic>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string ProfileImage { get; set; }
        public DateTime? RegisterData { get; set; }

        public virtual ICollection<Commenting> Commentings { get; set; }
        public virtual ICollection<Critic> Critics { get; set; }
    }
}
