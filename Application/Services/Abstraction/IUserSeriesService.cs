namespace Application.Services.Abstraction
{
    public interface IUserSeriesService
    {
        Task<bool> AddToFavoritesAsync(string userId, int seriesId);
        Task<bool> RemoveFromFavoritesAsync(string userId, int seriesId);
        Task<bool> AddToWatchListAsync(string userId, int seriesId);
        Task<bool> RemoveFromWatchListAsync(string userId, int seriesId);
        Task<IEnumerable<Core.Models.UserSeries>> GetFavoritesAsync(string userId);
        Task<IEnumerable<Core.Models.UserSeries>> GetWatchListAsync(string userId);
        Task<bool> IsFavoriteAsync(string userId, int seriesId);
        Task<bool> IsInWatchListAsync(string userId, int seriesId);
    }
}
