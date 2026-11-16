using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using Core.Repositories;

namespace Application.Services.Implementation
{
    public class UserSeriesService : IUserSeriesService
    {
        private readonly IUserSeriesRepository _userSeriesRepository;

        public UserSeriesService(IUserSeriesRepository userSeriesRepository)
        {
            _userSeriesRepository = userSeriesRepository;
        }

        public async Task<bool> AddToFavoritesAsync(string userId, int seriesId)
        {
            var item = await _userSeriesRepository.GetByKeyAsync(userId, seriesId);

            if (item == null)
            {
                item = new UserSeries
                {
                    UserId = userId,
                    SeriesId = seriesId,
                    IsFavorite = true
                };

                return await _userSeriesRepository.AddAsync(item);
            }

            item.IsFavorite = true;
            return await _userSeriesRepository.UpdateAsync(item);
        }

        public async Task<bool> RemoveFromFavoritesAsync(string userId, int seriesId)
        {
            var item = await _userSeriesRepository.GetByKeyAsync(userId, seriesId);

            if (item == null)
                return false;

            item.IsFavorite = false;

            if (!item.IsInWatchList)
                return await _userSeriesRepository.RemoveAsync(item);

            return await _userSeriesRepository.UpdateAsync(item);
        }

        public async Task<bool> AddToWatchListAsync(string userId, int seriesId)
        {
            var item = await _userSeriesRepository.GetByKeyAsync(userId, seriesId);

            if (item == null)
            {
                item = new UserSeries
                {
                    UserId = userId,
                    SeriesId = seriesId,
                    IsInWatchList = true
                };

                return await _userSeriesRepository.AddAsync(item);
            }

            item.IsInWatchList = true;
            return await _userSeriesRepository.UpdateAsync(item);
        }

        public async Task<bool> RemoveFromWatchListAsync(string userId, int seriesId)
        {
            var item = await _userSeriesRepository.GetByKeyAsync(userId, seriesId);

            if (item == null)
                return false;

            item.IsInWatchList = false;

            if (!item.IsFavorite)
                return await _userSeriesRepository.RemoveAsync(item);

            return await _userSeriesRepository.UpdateAsync(item);
        }

        public async Task<IEnumerable<UserSeries>> GetFavoritesAsync(string userId)
        {
            return await _userSeriesRepository.GetFavoritesAsync(userId);
        }

        public async Task<IEnumerable<UserSeries>> GetWatchListAsync(string userId)
        {
            return await _userSeriesRepository.GetWatchListAsync(userId);
        }

        public async Task<bool> IsFavoriteAsync(string userId, int seriesId)
        {
            return await _userSeriesRepository.ExistsAsync(
                x => x.UserId == userId && x.SeriesId == seriesId && x.IsFavorite);
        }

        public async Task<bool> IsInWatchListAsync(string userId, int seriesId)
        {
            return await _userSeriesRepository.ExistsAsync(
                x => x.UserId == userId && x.SeriesId == seriesId && x.IsInWatchList);
        }
    }
}
