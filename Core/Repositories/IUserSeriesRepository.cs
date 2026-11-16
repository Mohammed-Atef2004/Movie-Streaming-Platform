using Core.Models;

namespace Core.Repositories
{
    public interface IUserSeriesRepository : IGenericRepository<UserSeries>
    {
        Task<UserSeries?> GetByKeyAsync(string userId, int seriesId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserSeries>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserSeries>> GetWatchListAsync(string userId, CancellationToken cancellationToken = default);
    }
}
