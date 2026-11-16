using Core.Models;

namespace Core.Repositories
{
    public interface IEpisodeRepository : IGenericRepository<Episode>
    {
        Task<IEnumerable<Episode>> GetBySeriesAsync(int seriesId, CancellationToken cancellationToken = default);

        Task IncrementViewsAsync(int episodeId, CancellationToken cancellationToken = default);

        Task IncrementDownloadsAsync(int episodeId, CancellationToken cancellationToken = default);
    }
}
