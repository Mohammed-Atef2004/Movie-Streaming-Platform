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
    public class MovieRepository:GenericRepository<Movie>, IMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public MovieRepository(ApplicationDbContext context):base(context)
        {
            _context= context;
        }
        public bool Update(Movie movie)
        { 
            var MoviewFromDb = _context.Movies.FirstOrDefault(u => u.Id == movie.Id);
            if(MoviewFromDb != null)
            {
                MoviewFromDb.Update(movie.Title, movie.Description,movie.IsFree, movie.ImageUrl, movie.UpdatedBy);
                _context.Movies.Update(MoviewFromDb);

                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else return false;
            }
            return false;
        }
    }
}
