using AmazingProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AmazingProject.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options){}
        public DbSet<Person> People { get; set; }
        public DbSet<photo> Photos { get; set; }
        public DbSet<Like> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Like>()
                .HasKey(k => new { k.LikerId, k.LikeeId });

            builder.Entity<Like>()
                .HasOne(p => p.Likee)
                .WithMany(p => p.Likers)
                .HasForeignKey(p => p.LikeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Like>()
                .HasOne(p => p.Liker)
                .WithMany(p => p.Likees)
                .HasForeignKey(p => p.LikerId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
