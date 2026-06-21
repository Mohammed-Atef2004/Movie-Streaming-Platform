using Core.Models;

namespace Application.Services.Abstraction
{
    public interface IUserMovieService
    {
        bool AddToWatchList(string userId, int movieId);

        bool RemoveFromWatchList(string userId, int movieId);

        bool AddToFavorites(string userId, int movieId);

        bool RemoveFromFavorites(string userId, int movieId);

        IEnumerable<UserMovie> GetWatchList(string userId);

        bool IsFavorite(string userId, int movieId);
        bool IsInWatchList(string userId, int movieId);
        IEnumerable<UserMovie> GetFavorites(string userId);
    }
}