using AmazingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
   public interface IAuthRepository
    {
        Task<Person> Register(Person person, string password);
        Task<Person> Login(String username, string password);
        Task<bool> PersonExists(string username);
    }
}
