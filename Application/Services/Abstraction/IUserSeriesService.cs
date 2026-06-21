using Core.Models;

namespace Application.Services.Abstraction
{
    public interface IUserSeriesService
    {
        bool AddToWatchList(string userId, int seriesId);

        bool RemoveFromWatchList(string userId, int seriesId);

        bool AddToFavorites(string userId, int seriesId);

        bool RemoveFromFavorites(string userId, int seriesId);

        IEnumerable<UserSeries> GetWatchList(string userId);

        IEnumerable<UserSeries> GetFavorites(string userId);
    }
}