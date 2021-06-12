using System;

namespace Karia.Api.Models
{
    public class CommentsDto
    {
        public string Name { get; set; }
        public string Profile { get; set; }
        public string Text { get; set; }
        public DateTime DateOfRegistration { get; set; }
    }
}