using Core.Models;
using Core.Repositories;
using DAL.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class UserMovieRepository:GenericRepository<UserMovie>,IUserMovieRepository
    {
        private readonly ApplicationDbContext _context;
        public UserMovieRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public UserMovie GetById(string userId,int movieId)
        {
            var result=_context.UserMovies.Where(x=>x.UserId==userId&&x.MovieId==movieId)
                .FirstOrDefault();
            return result;
        }

    }
}
