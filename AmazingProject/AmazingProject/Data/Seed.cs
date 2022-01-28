using AmazingProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
    public class Seed
    {
        public static void SeedPeople(DataContext context)
        {
            if (!context.People.Any())
            {
                var personData = System.IO.File.ReadAllText("Data/PersonSeedData.json");
                var people = JsonConvert.DeserializeObject<List<Person>>(personData);
                foreach (var person in people)
                {
                    byte[] passwordhash, passwordSalt;
                    CreatePasswordHash("password", out passwordhash, out passwordSalt);

                    person.PasswordHash = passwordhash;
                    person.PasswordSalt = passwordSalt;
                    person.Username = person.Username.ToLower();
                    context.People.Add(person);
                }

                context.SaveChanges();
            }
        }
        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
