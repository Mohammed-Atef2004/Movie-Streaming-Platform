using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using DAL.Repositories.Abstraction;

namespace Application.Services.Implementation
{
    public class UserSeriesService : IUserSeriesService
    {
        private readonly IUserSeriesRepository _userSeriesRepository;

        public UserSeriesService(IUserSeriesRepository userSeriesRepository)
        {
            _userSeriesRepository = userSeriesRepository;
        }

        public bool AddToFavorites(string userId, int seriesId)
        {
            var item = _userSeriesRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.SeriesId == seriesId);

            if (item == null)
            {
                item = new UserSeries
                {
                    UserId = userId,
                    SeriesId = seriesId,
                    IsFavorite = true
                };

                return _userSeriesRepository.Add(item);
            }

            item.IsFavorite = true;
            return _userSeriesRepository.Update(item);
        }

        public bool RemoveFromFavorites(string userId, int seriesId)
        {
            var item = _userSeriesRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.SeriesId == seriesId);

            if (item == null)
                return false;

            item.IsFavorite = false;

            if (!item.IsInWatchList)
                return _userSeriesRepository.Remove(item);

            return _userSeriesRepository.Update(item);
        }

        public bool AddToWatchList(string userId, int seriesId)
        {
            var item = _userSeriesRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.SeriesId == seriesId);

            if (item == null)
            {
                item = new UserSeries
                {
                    UserId = userId,
                    SeriesId = seriesId,
                    IsInWatchList = true
                };

                return _userSeriesRepository.Add(item);
            }

            item.IsInWatchList = true;
            return _userSeriesRepository.Update(item);
        }

        public bool RemoveFromWatchList(string userId, int seriesId)
        {
            var item = _userSeriesRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.SeriesId == seriesId);

            if (item == null)
                return false;

            item.IsInWatchList = false;

            if (!item.IsFavorite)
                return _userSeriesRepository.Remove(item);

            return _userSeriesRepository.Update(item);
        }

        public IEnumerable<UserSeries> GetFavorites(string userId)
        {
            return _userSeriesRepository.GetAll(
                x => x.UserId == userId && x.IsFavorite,
                "Series");
        }

        public IEnumerable<UserSeries> GetWatchList(string userId)
        {
            return _userSeriesRepository.GetAll(
                x => x.UserId == userId && x.IsInWatchList,
                "Series");
        }
    }
}