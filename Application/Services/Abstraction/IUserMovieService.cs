namespace Application.Services.Abstraction
{
    public interface IUserMovieService
    {
        Task<bool> AddToFavoritesAsync(string userId, int movieId);
        Task<bool> RemoveFromFavoritesAsync(string userId, int movieId);
        Task<bool> AddToWatchListAsync(string userId, int movieId);
        Task<bool> RemoveFromWatchListAsync(string userId, int movieId);
        Task<IEnumerable<Core.Models.UserMovie>> GetFavoritesAsync(string userId);
        Task<IEnumerable<Core.Models.UserMovie>> GetWatchListAsync(string userId);
        Task<bool> IsFavoriteAsync(string userId, int movieId);
        Task<bool> IsInWatchListAsync(string userId, int movieId);
    }
}
