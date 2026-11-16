using Core.Models;
using Core.Repositories;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class UserSeriesRepository : GenericRepository<UserSeries>, IUserSeriesRepository
    {
        public UserSeriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public Task<UserSeries?> GetByKeyAsync(string userId, int seriesId, CancellationToken cancellationToken = default)
        {
            return _context.UserSeries
                .FirstOrDefaultAsync(x => x.UserId == userId && x.SeriesId == seriesId, cancellationToken);
        }

        public async Task<IEnumerable<UserSeries>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserSeries
                .AsNoTracking()
                .Include(x => x.Series)
                .Where(x => x.UserId == userId && x.IsFavorite)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserSeries>> GetWatchListAsync(string userId, CancellationToken cancellationToken = default)
        {
            return await _context.UserSeries
                .AsNoTracking()
                .Include(x => x.Series)
                .Where(x => x.UserId == userId && x.IsInWatchList)
                .OrderByDescending(x => x.AddedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
