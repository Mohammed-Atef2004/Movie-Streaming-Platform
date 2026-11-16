using Core.Models;

namespace Core.Repositories
{
    public interface IReviewRepository : IGenericRepository<Review>
    {
        Task<IEnumerable<Review>> GetMovieReviewsWithUsersAsync(int movieId, CancellationToken cancellationToken = default);

        /// <summary>Average star rating of a movie's reviews, or null when the movie has no reviews.</summary>
        Task<double?> GetMovieAverageRatingAsync(int movieId, CancellationToken cancellationToken = default);
    }
}
