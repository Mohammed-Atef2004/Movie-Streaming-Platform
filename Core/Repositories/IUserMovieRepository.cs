using Core.Models;

namespace Core.Repositories
{
    public interface IUserMovieRepository : IGenericRepository<UserMovie>
    {
        Task<UserMovie?> GetByKeyAsync(string userId, int movieId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserMovie>> GetFavoritesAsync(string userId, CancellationToken cancellationToken = default);

        Task<IEnumerable<UserMovie>> GetWatchListAsync(string userId, CancellationToken cancellationToken = default);
    }
}
