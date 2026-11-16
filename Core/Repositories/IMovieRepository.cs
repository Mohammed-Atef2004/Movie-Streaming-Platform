using Core.Models;

namespace Core.Repositories
{
    public interface IMovieRepository : IGenericRepository<Movie>
    {
        /// <summary>Paged, server-side search over movies (title search + category filter + sorting).</summary>
        Task<(IEnumerable<Movie> Items, int TotalCount)> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default);

        /// <summary>Atomically increments the Views counter in the database.</summary>
        Task IncrementViewsAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>Atomically increments the Downloads counter in the database.</summary>
        Task IncrementDownloadsAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>Sets the aggregated rating of a movie (from its reviews).</summary>
        Task UpdateRatingAsync(int movieId, double rating, CancellationToken cancellationToken = default);
    }
}
