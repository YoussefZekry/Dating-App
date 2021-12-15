using AmazingProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
    public class DatingRepository : IDatingRepository
    {
        private readonly DataContext _context;

        public DatingRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<photo> GetMainPhotoForPerson(int personId)
        {
            return await _context.Photos.Where(p => p.personId == personId).FirstOrDefaultAsync(p => p.IsMain);
        }

        public async Task<IEnumerable<Person>> GetPeople()
        {
            var people = await _context.people.Include(p => p.Photos).ToListAsync();
            return people;
        }

        public async Task<Person> GetPerson(int id)
        {
            var person = await _context.people.Include(p => p.Photos).FirstOrDefaultAsync(per => per.Id == id);
            return person;
        }

        public async Task<photo> GetPhoto(int id)
        {
            var photo = await _context.Photos.FirstOrDefaultAsync(p => p.Id == id);
            return photo;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
