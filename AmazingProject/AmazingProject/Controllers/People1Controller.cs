using AmazingProject.Data;
using AmazingProject.Dtos;
using AmazingProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace AmazingProject.Controllers
{
    [Authorize]
    [Route("[Controller]")]
    [ApiController]
    public class People1Controller : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public People1Controller(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPeople()
        {
            var people = await _repo.GetPeople();
            var peopleToReturn = _mapper.Map<IEnumerable<PersonForListDto>>(people);
            return Ok(peopleToReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerson(int id)
        {
            var person = await _repo.GetPerson(id);
            var personToReturn = _mapper.Map<PersonForDetailedDto>(person);
            return Ok(personToReturn);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePerson(int id, PersonForUpdateDto personForUpdateDto)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var personFromRepo = await _repo.GetPerson(id);
            _mapper.Map(personForUpdateDto, personFromRepo);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception($"Updating Person {id} failed on save");
        }
    }
}
