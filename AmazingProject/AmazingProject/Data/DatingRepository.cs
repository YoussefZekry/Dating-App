﻿using AmazingProject.Helpers;
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

        public async Task<PagedList<Person>> GetPeople(PersonParams personParams)
        {
            var people = _context.people.Include(p => p.Photos).OrderByDescending(p =>p.LastActive).AsQueryable();
            people = people.Where(p => p.Id != personParams.PersonId);
            people = people.Where(p => p.Gender == personParams.Gender);
            if (personParams.MinAge != 18 || personParams.MaxAge != 99)
            {
                var minDob = DateTime.Today.AddYears(-personParams.MaxAge - 1);
                var maxDob = DateTime.Today.AddYears(-personParams.MinAge);
                people = people.Where(p => p.DateOfBirth >= minDob && p.DateOfBirth <= maxDob);
            }

            if (!string.IsNullOrEmpty(personParams.OrderBy))
            {
                switch (personParams.OrderBy)
                {
                    case "created":
                        people = people.OrderByDescending(p => p.Created);
                        break;
                    default:
                        people = people.OrderByDescending(p => p.LastActive);
                        break;
                }
            }

            return await PagedList<Person>.CreateAsync(people, personParams.PageNumber, personParams.PageSize);
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
