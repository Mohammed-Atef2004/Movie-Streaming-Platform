using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using DAL.Repositories.Abstraction;

namespace Application.Services.Implementation
{
    public class UserMovieService : IUserMovieService
    {
        private readonly IUserMovieRepository _userMovieRepository;

        public UserMovieService(IUserMovieRepository userMovieRepository)
        {
            _userMovieRepository = userMovieRepository;
        }

        public bool AddToFavorites(string userId, int movieId)
        {
            var item = _userMovieRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.MovieId == movieId);

            if (item == null)
            {
                item = new UserMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    IsFavorite = true
                };

                return _userMovieRepository.Add(item);
            }

            item.IsFavorite = true;
            return _userMovieRepository.Update(item);
        }

        public bool RemoveFromFavorites(string userId, int movieId)
        {
            var item = _userMovieRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.MovieId == movieId);

            if (item == null)
                return false;

            item.IsFavorite = false;

            if (!item.IsInWatchList)
                return _userMovieRepository.Remove(item);

            return _userMovieRepository.Update(item);
        }

        public bool AddToWatchList(string userId, int movieId)
        {
            var item = _userMovieRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.MovieId == movieId);

            if (item == null)
            {
                item = new UserMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    IsInWatchList = true
                };

                return _userMovieRepository.Add(item);
            }

            item.IsInWatchList = true;
            return _userMovieRepository.Update(item);
        }

        public bool RemoveFromWatchList(string userId, int movieId)
        {
            var item = _userMovieRepository.GetFirstOrDefault(
                x => x.UserId == userId && x.MovieId == movieId);

            if (item == null)
                return false;

            item.IsInWatchList = false;

            if (!item.IsFavorite)
                return _userMovieRepository.Remove(item);

            return _userMovieRepository.Update(item);
        }

        public IEnumerable<UserMovie> GetFavorites(string userId)
        {
            return _userMovieRepository.GetAll(
                x => x.UserId == userId && x.IsFavorite,
                "Movie");
        }

        public IEnumerable<UserMovie> GetWatchList(string userId)
        {
            return _userMovieRepository.GetAll(
                x => x.UserId == userId && x.IsInWatchList,
                "Movie");
        }
        public bool IsFavorite(string userId, int movieId)
        {
            return _userMovieRepository.GetFirstOrDefault(
            x => x.UserId == userId &&
         x.MovieId == movieId &&
         x.IsFavorite) != null;
        }
        public bool IsInWatchList(string userId, int movieId)
        {
            return _userMovieRepository.GetFirstOrDefault(
           x => x.UserId == userId &&
         x.MovieId == movieId &&
         x.IsInWatchList) != null;
        }
    }
}