using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace Application.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IReviewRepository reviewRepository,
            IMovieRepository movieRepository,
            ILogger<ReviewService> logger)
        {
            _reviewRepository = reviewRepository;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> CreateReviewAsync(string userId, int movieId, string comment, int rating = 5)
        {
            try
            {
                var review = Review.Create(userId, movieId, comment, rating);
                if (await _reviewRepository.AddAsync(review))
                {
                    await RecalculateMovieRatingAsync(movieId);
                    return (true, "Review Added Successfully");
                }
                return (false, "Failed to Add Review");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to add review for movie {MovieId}", movieId);
                return (false, "Failed to Add Review");
            }
        }

        public async Task<(bool Success, string Message)> DeleteReviewAsync(string userId, int movieId)
        {
            var review = await _reviewRepository.GetFirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
            if (review == null)
            {
                return (false, "Failed to find the review to delete");
            }

            if (await _reviewRepository.RemoveAsync(review))
            {
                await RecalculateMovieRatingAsync(movieId);
                return (true, "Review Deleted Successfully");
            }
            return (false, "Failed to Delete Review");
        }

        public async Task<(bool Success, string Message)> UpdateReviewAsync(string userId, int movieId, string comment, int rating = 5)
        {
            // Bug fix: the old copy-pasted messages said "Deleted" on the update path.
            var review = await _reviewRepository.GetFirstOrDefaultAsync(x => x.UserId == userId && x.MovieId == movieId);
            if (review == null)
            {
                return (false, "Failed to find the review to update");
            }

            review.Update(comment, rating);
            if (await _reviewRepository.UpdateAsync(review))
            {
                await RecalculateMovieRatingAsync(movieId);
                return (true, "Review Updated Successfully");
            }
            return (false, "Failed to Update Review");
        }

        public async Task<IEnumerable<Review>> GetUserReviewsAsync(string userId)
        {
            return await _reviewRepository.GetAllAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<Review>> GetFilmReviewsAsync(int movieId)
        {
            return await _reviewRepository.GetMovieReviewsWithUsersAsync(movieId);
        }

        /// <summary>
        /// Keeps Movie.Rating in sync with its reviews: the average of the review
        /// stars, or the neutral 5 when the movie has no reviews left.
        /// </summary>
        private async Task RecalculateMovieRatingAsync(int movieId)
        {
            var average = await _reviewRepository.GetMovieAverageRatingAsync(movieId);
            await _movieRepository.UpdateRatingAsync(movieId, average ?? 5);
        }
    }
}
