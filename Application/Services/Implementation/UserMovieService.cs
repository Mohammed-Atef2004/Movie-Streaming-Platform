using Application.Services.Abstraction;
using Core.Models;
using Core.Repositories;
using Core.Repositories;

namespace Application.Services.Implementation
{
    public class UserMovieService : IUserMovieService
    {
        private readonly IUserMovieRepository _userMovieRepository;

        public UserMovieService(IUserMovieRepository userMovieRepository)
        {
            _userMovieRepository = userMovieRepository;
        }

        public async Task<bool> AddToFavoritesAsync(string userId, int movieId)
        {
            var item = await _userMovieRepository.GetByKeyAsync(userId, movieId);

            if (item == null)
            {
                item = new UserMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    IsFavorite = true
                };

                return await _userMovieRepository.AddAsync(item);
            }

            item.IsFavorite = true;
            return await _userMovieRepository.UpdateAsync(item);
        }

        public async Task<bool> RemoveFromFavoritesAsync(string userId, int movieId)
        {
            var item = await _userMovieRepository.GetByKeyAsync(userId, movieId);

            if (item == null)
                return false;

            item.IsFavorite = false;

            if (!item.IsInWatchList)
                return await _userMovieRepository.RemoveAsync(item);

            return await _userMovieRepository.UpdateAsync(item);
        }

        public async Task<bool> AddToWatchListAsync(string userId, int movieId)
        {
            var item = await _userMovieRepository.GetByKeyAsync(userId, movieId);

            if (item == null)
            {
                item = new UserMovie
                {
                    UserId = userId,
                    MovieId = movieId,
                    IsInWatchList = true
                };

                return await _userMovieRepository.AddAsync(item);
            }

            item.IsInWatchList = true;
            return await _userMovieRepository.UpdateAsync(item);
        }

        public async Task<bool> RemoveFromWatchListAsync(string userId, int movieId)
        {
            var item = await _userMovieRepository.GetByKeyAsync(userId, movieId);

            if (item == null)
                return false;

            item.IsInWatchList = false;

            if (!item.IsFavorite)
                return await _userMovieRepository.RemoveAsync(item);

            return await _userMovieRepository.UpdateAsync(item);
        }

        public async Task<IEnumerable<UserMovie>> GetFavoritesAsync(string userId)
        {
            return await _userMovieRepository.GetFavoritesAsync(userId);
        }

        public async Task<IEnumerable<UserMovie>> GetWatchListAsync(string userId)
        {
            return await _userMovieRepository.GetWatchListAsync(userId);
        }

        public async Task<bool> IsFavoriteAsync(string userId, int movieId)
        {
            return await _userMovieRepository.ExistsAsync(
                x => x.UserId == userId && x.MovieId == movieId && x.IsFavorite);
        }

        public async Task<bool> IsInWatchListAsync(string userId, int movieId)
        {
            return await _userMovieRepository.ExistsAsync(
                x => x.UserId == userId && x.MovieId == movieId && x.IsInWatchList);
        }
    }
}
