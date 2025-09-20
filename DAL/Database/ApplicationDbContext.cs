using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Database
{
    public class ApplicationDbContext:DbContext
    {
        public DbSet<Movie>Movies{ get; set; }
        public DbSet<Category>Categories{ get; set; }
        public DbSet<Series>Series{ get; set; } 
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {
        }
    }
}
