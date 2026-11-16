using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class MovieService : IMovieService
    {
        private readonly IMapper _mapper;
        private readonly IMovieRepository _movieRepository;
        private readonly ILogger<MovieService> _logger;

        public MovieService(IMapper mapper, IMovieRepository movieRepository, ILogger<MovieService> logger)
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> CreateMovieAsync(MovieVM movieVM, string? actor = null)
        {
            try
            {
                if (movieVM.Image != null)
                {
                    movieVM.ImageUrl = BLL.Helpers.Load.UploadImage(movieVM.Image);
                }

                var movie = _mapper.Map<Movie>(movieVM);
                movie.Create(
                    movieVM.Title,
                    movieVM.Description,
                    movieVM.ImageUrl,
                    movieVM.IsFree,
                    movieVM.CategoryId,
                    movieVM.TrailerUrl,
                    movieVM.MovieUrl,
                    movieVM.Rating,
                    movieVM.Year,
                    creator: actor);

                if (await _movieRepository.AddAsync(movie))
                {
                    return (true, "Movie Created Successfully");
                }
                return (false, "Failed to Create Movie");
            }
            catch (InvalidDataException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create movie {Title}", movieVM.Title);
                return (false, "An unexpected error occurred while creating the movie. Please check the category exists and try again.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteMovieAsync(int id, string? actor = null)
        {
            try
            {
                var movie = await _movieRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (movie == null)
                {
                    return (false, "Movie Not Found");
                }

                // Bug fix: remove the DB row first (soft delete) and only then the image file.
                if (await _movieRepository.RemoveAsync(movie))
                {
                    if (!string.IsNullOrEmpty(movie.ImageUrl))
                    {
                        BLL.Helpers.Load.RemoveFile("Images", movie.ImageUrl);
                    }
                    return (true, "Movie Deleted Successfully");
                }
                return (false, "Failed to Delete Movie");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete movie {MovieId}", id);
                return (false, "An unexpected error occurred while deleting the movie.");
            }
        }

        public async Task<IEnumerable<MovieVM>> GetAllMoviesAsync()
        {
            var movies = await _movieRepository.GetAllAsync(includeWords: "Category", orderBy: q => q.OrderByDescending(m => m.CreatedAt));
            return _mapper.Map<IEnumerable<MovieVM>>(movies);
        }

        public async Task<MovieVM?> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetFirstOrDefaultAsync(x => x.Id == id, includeWords: "Category");
            return movie == null ? null : _mapper.Map<MovieVM>(movie);
        }

        public async Task<IEnumerable<Movie>> GetRecentMoviesAsync(int count)
        {
            return await _movieRepository.GetAllAsync(
                orderBy: q => q.OrderByDescending(m => m.CreatedAt),
                includeWords: "Category")
                .ContinueWith(t => t.Result.Take(count).ToList());
        }

        public async Task<int> GetMoviesCountAsync()
        {
            return await _movieRepository.CountAsync();
        }

        public async Task<(IEnumerable<MovieVM> Movies, int TotalCount)> SearchMoviesAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12)
        {
            var (items, totalCount) = await _movieRepository.SearchAsync(searchTerm, categoryId, sortBy, page, pageSize);
            return (_mapper.Map<IEnumerable<MovieVM>>(items), totalCount);
        }

        public async Task IncrementViewsAsync(int id)
        {
            await _movieRepository.IncrementViewsAsync(id);
        }

        public async Task IncrementDownloadsAsync(int id)
        {
            await _movieRepository.IncrementDownloadsAsync(id);
        }

        public async Task<bool> IsMovieFreeAsync(int id)
        {
            var movie = await _movieRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return movie?.IsFree ?? false;
        }

        public async Task<(bool Success, string Message)> UpdateMovieAsync(MovieVM movieVM, int id, string? actor = null)
        {
            try
            {
                var movie = await _movieRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (movie == null)
                {
                    return (false, "Movie Not Found");
                }

                if (movieVM.Image != null)
                {
                    var oldImage = movie.ImageUrl;
                    var newImage = BLL.Helpers.Load.UploadImage(movieVM.Image);
                    movie.ImageUrl = newImage;

                    if (await _movieRepository.UpdateAsync(movie) && !string.IsNullOrEmpty(oldImage))
                    {
                        BLL.Helpers.Load.RemoveFile("Images", oldImage);
                    }
                }
                else
                {
                    // Preserve the existing image when the form doesn't carry one.
                    movieVM.ImageUrl = movie.ImageUrl;
                }

                // Bug fix: the old implementation never updated TrailerUrl, MovieUrl,
                // Rating or Year. Views/Downloads are preserved (they are incremented
                // by watch/download actions, never edited from the admin form).
                movie.Update(
                    movieVM.Title,
                    movieVM.Description,
                    movie.ImageUrl,
                    movieVM.IsFree,
                    movieVM.CategoryId,
                    movieVM.TrailerUrl,
                    movieVM.MovieUrl,
                    movieVM.Rating,
                    movieVM.Year,
                    views: movie.Views,
                    downloads: movie.Downloads,
                    updater: actor);

                var result = await _movieRepository.UpdateAsync(movie);

                return result
                    ? (true, "Movie Updated Successfully")
                    : (false, "Failed to Update Movie");
            }
            catch (InvalidDataException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update movie {MovieId}", id);
                return (false, "An unexpected error occurred while updating the movie.");
            }
        }
    }
}
