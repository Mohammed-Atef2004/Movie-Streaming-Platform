using Core.Models;
using Core.Repositories;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class UserMovieRepository : GenericRepository<UserMovie>, IUserMovieRepository
    {
        public UserMovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<UserMovie?> GetByKeyAsync(string userId, int movieId, CancellationToken cancellationToken = default)
        {
            return _context.UserMovies
                .FirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId, cancellationToken);
        }

        public async Task<IEnumerable<UserMovie>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserMovies
                .AsNoTracking()
                .Include(x => x.Movie)
                .Where(x => x.UserId == userId && x.IsFavorite)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserMovie>> GetWatchListAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserMovies
                .AsNoTracking()
                .Include(x => x.Movie)
                .Where(x => x.UserId == userId && x.IsInWatchList)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
