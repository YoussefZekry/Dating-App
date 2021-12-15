using System;

namespace AmazingProject.Models
{
    public class photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime DateAdded { get; set; }
        public bool IsMain { get; set; }
        public String PublicId { get; set; }
        public Person person { get; set; }
        public int personId { get; set; }
    }
}