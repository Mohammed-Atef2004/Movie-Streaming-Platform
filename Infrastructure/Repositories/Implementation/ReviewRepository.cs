using Core.Models;
using Core.Repositories;
using DAL.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories.Implementation
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;

        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public IEnumerable<Review> GetMovieReviewsWithUsers(int movieId)
        {
            return _context.Reviews
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .ToList();
        }
    }
}
