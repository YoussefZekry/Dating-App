using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Dtos
{
    public class PersonForListDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public String Gender { get; set; }
        public int Age { get; set; }
        public DateTime LastActive { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhotoUrl { get; set; }

    }
}
