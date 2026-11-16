using BLL.ViewModels;
using Core.Models;

namespace BLL.Services.Abstraction
{
    public interface IMovieService
    {
        Task<(bool Success, string Message)> CreateMovieAsync(MovieVM movieVM, string? actor = null);
        Task<(bool Success, string Message)> UpdateMovieAsync(MovieVM movieVM, int id, string? actor = null);
        Task<(bool Success, string Message)> DeleteMovieAsync(int id, string? actor = null);
        Task<IEnumerable<MovieVM>> GetAllMoviesAsync();
        Task<MovieVM?> GetMovieByIdAsync(int id);
        Task<IEnumerable<Movie>> GetRecentMoviesAsync(int count);
        Task<int> GetMoviesCountAsync();
        Task<(IEnumerable<MovieVM> Movies, int TotalCount)> SearchMoviesAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12);
        Task IncrementViewsAsync(int id);
        Task IncrementDownloadsAsync(int id);
        Task<bool> IsMovieFreeAsync(int id);
    }
}
