using AmazingProject.Data;
using AmazingProject.Dtos;
using AmazingProject.Helpers;
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
    [ServiceFilter(typeof(LogPersonActivity))]
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
        public async Task<IActionResult> GetPeople([FromQuery]PersonParams personParams)
        {
            var currentPersonId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var personFromRepo = await _repo.GetPerson(currentPersonId);
            personParams.PersonId = currentPersonId;
            if (string.IsNullOrEmpty(personParams.Gender))
            {
                personParams.Gender = personFromRepo.Gender == "male" ? "female" : "male";
            }
            var people = await _repo.GetPeople(personParams);
            var peopleToReturn = _mapper.Map<IEnumerable<PersonForListDto>>(people);
            Response.AddPagination(people.CurrentPage, people.PageSize, people.TotalCount, people.TotalPages);
            return Ok(peopleToReturn);
        }

        [HttpGet("{id}", Name ="GetPerson")]
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

        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikePerson(int id, int recipientId)
        {
            if (id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();

            var like = await _repo.GetLike(id, recipientId);
            if (like != null)
                return BadRequest("You Already Like This Person");

            if(await _repo.GetPerson(recipientId)==null)
            {
                return NotFound();
            }

            like = new Like
            {
                LikerId = id,
                LikeeId = recipientId
            };

            _repo.Add<Like>(like);
            if (await _repo.SaveAll())
                return Ok();

            return BadRequest("Failed to like this person");

        }
    }
}
