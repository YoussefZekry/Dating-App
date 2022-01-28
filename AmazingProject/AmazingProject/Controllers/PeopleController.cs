using AmazingProject.Data;
using AmazingProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class PeopleController : ControllerBase
    {
        private readonly DataContext _context;

        public PeopleController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            var People = await _context.People.ToListAsync();
            return Ok(People);
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var Person = await _context.People.FirstOrDefaultAsync(x => x.Id == id);
            return Ok(Person);
        }
    }
}
