using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool Update(Movie movie)
        {
            _context.Movies.Attach(movie);
            _context.Entry(movie).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            return _context.SaveChanges() > 0;
        }
    }
}