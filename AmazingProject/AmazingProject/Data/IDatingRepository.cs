using AmazingProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
    public interface IDatingRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<IEnumerable<Person>> GetPeople();
        Task<Person> GetPerson(int id);
        Task<photo> GetPhoto(int id);
        Task<photo> GetMainPhotoForPerson(int personId);
    }
}
