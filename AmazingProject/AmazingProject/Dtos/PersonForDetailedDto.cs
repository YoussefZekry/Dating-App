using AmazingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Dtos
{
    public class PersonForDetailedDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public String Gender { get; set; }
        public int Age { get; set; }
        public DateTime LastActive { get; set; }
        public String Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }
        public ICollection<PhotoForDetailedDto> photos { get; set; }
    }
}
