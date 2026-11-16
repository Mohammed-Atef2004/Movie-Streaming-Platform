using Core.Models;

namespace Core.Repositories
{
    public interface ISeriesRepository : IGenericRepository<Series>
    {
        /// <summary>Paged, server-side search over series (title search + category filter + sorting).</summary>
        Task<(IEnumerable<Series> Items, int TotalCount)> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default);

        /// <summary>Atomically increments the ViewCount counter in the database.</summary>
        Task IncrementViewsAsync(int seriesId, CancellationToken cancellationToken = default);

        /// <summary>Atomically increments the DownloadCount counter in the database.</summary>
        Task IncrementDownloadsAsync(int seriesId, CancellationToken cancellationToken = default);
    }
}
