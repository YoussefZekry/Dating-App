using AmazingProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _Context;
        public AuthRepository(DataContext context)
        {
            _Context = context;
        }

        //Regiter method
        public async Task<Person> Register(Person person, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            person.PasswordHash = passwordHash;
            person.PasswordSalt = passwordSalt;

            await _Context.people.AddAsync(person);
            await _Context.SaveChangesAsync();

            return person;
        }

        //CreatePasswordHash method
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        //Login method
        public async Task<Person> Login(string username, string password)
        {
            var person = await _Context.people.Include(p => p.Photos).FirstOrDefaultAsync(x => x.Username == username);

            if (person == null)
                return null;

            if (!VerifyPasswordHash(password, person.PasswordHash, person.PasswordSalt))
                return null;

            return person;
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != passwordHash[i]) return false;
                }

                return true;
            }
        }

        //PeronExist method 
        //checks if peron already exist or not.
        public async Task<bool> PersonExists(string username)
        {
            if (await _Context.people.AnyAsync(x => x.Username == username))
                return true;

            return false;
        }

    }
}
