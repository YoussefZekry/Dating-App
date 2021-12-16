using AmazingProject.Data;
using AmazingProject.Dtos;
using AmazingProject.Models;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AmazingProject.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase   
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _repo = repo;
            _config = config;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(PersonForRegisterDto personForRegisterDto)
        {
            personForRegisterDto.Username = personForRegisterDto.Username.ToLower();
            if (await _repo.PersonExists(personForRegisterDto.Username))
                return BadRequest("Username already exists");

            var personToCreate = _mapper.Map<Person>(personForRegisterDto);

            var createdPerson = await _repo.Register(personToCreate, personForRegisterDto.Password);
            var personToReturn = _mapper.Map<PersonForDetailedDto>(createdPerson);

            return CreatedAtAction("GetPerson", new { Controller = "Peope1", id = createdPerson.Id },personToReturn);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(PersonForLoginDto personForLoginDto)
        {
            var personFromRepo = await _repo.Login(personForLoginDto.Username.ToLower(), personForLoginDto.Password);

            if (personFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,personFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, personFromRepo.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var person = _mapper.Map<PersonForListDto>(personFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                person
            });
        }

    }
}
