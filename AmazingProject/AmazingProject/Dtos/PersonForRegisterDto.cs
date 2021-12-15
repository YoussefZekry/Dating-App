using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Dtos
{
    public class PersonForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8,MinimumLength = 4,ErrorMessage ="Password must be between 4 and 8 characters")]
        public string Password { get; set; }
    }
}
