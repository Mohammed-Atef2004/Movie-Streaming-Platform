namespace Application.Services.Abstraction
{
    public interface IReviewService
    {
        Task<(bool Success, string Message)> CreateReviewAsync(string userId, int movieId, string comment, int rating = 5);
        Task<(bool Success, string Message)> DeleteReviewAsync(string userId, int movieId);
        Task<(bool Success, string Message)> UpdateReviewAsync(string userId, int movieId, string comment, int rating = 5);
        Task<IEnumerable<Core.Models.Review>> GetUserReviewsAsync(string userId);
        Task<IEnumerable<Core.Models.Review>> GetFilmReviewsAsync(int movieId);
    }
}
