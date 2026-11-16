using Core.Models;
using Core.Repositories;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetMovieReviewsWithUsersAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .AsNoTracking()
                .Include(r => r.User)
                .Where(r => r.MovieId == movieId)
                .OrderByDescending(r => r.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<double?> GetMovieAverageRatingAsync(int movieId, CancellationToken cancellationToken = default)
        {
            return await _context.Reviews
                .Where(r => r.MovieId == movieId)
                .AverageAsync(r => (double?)r.Rating, cancellationToken);
        }
    }
}
