using Core.Models;
using DAL.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Presistance.Database
{
    public class ApplicationDbContext:IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie>Movies{ get; set; }
        public DbSet<Category>Categories{ get; set; }
        public DbSet<Series>Series{ get; set; } 
        public DbSet<Episode> Episodes{ get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<UserSeries> UserSeries{ get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<UserMovie>()
               .HasKey(x => new { x.UserId, x.MovieId });

            builder.Entity<UserSeries>()
                .HasKey(x => new { x.UserId, x.SeriesId });
            builder.Entity<Review>()
                .HasKey(x=>new {x.UserId, x.MovieId});
        }
    }
}
