using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class SeriesService : ISeriesService
    {
        private readonly IMapper _mapper;
        private readonly ISeriesRepository _seriesRepository;
        private readonly ILogger<SeriesService> _logger;

        public SeriesService(IMapper mapper, ISeriesRepository seriesRepository, ILogger<SeriesService> logger)
        {
            _mapper = mapper;
            _seriesRepository = seriesRepository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> CreateSeriesAsync(SeriesVM seriesVM, string? actor = null)
        {
            try
            {
                // Bug fix: the old implementation called UploadFile without a null
                // check and crashed with a NullReferenceException when no image was picked.
                if (seriesVM.Image != null)
                {
                    seriesVM.ImageUrl = BLL.Helpers.Load.UploadImage(seriesVM.Image);
                }

                var series = _mapper.Map<Series>(seriesVM);
                series.Create(
                    seriesVM.Title,
                    viewCount: 0,
                    downloadCount: 0,
                    categoryId: seriesVM.CategoryId,
                    description: seriesVM.Description,
                    isFree: seriesVM.IsFree,
                    imageUrl: seriesVM.ImageUrl,
                    creator: actor);

                if (await _seriesRepository.AddAsync(series))
                {
                    return (true, "Series Created Successfully");
                }
                return (false, "Failed to Create Series");
            }
            catch (InvalidDataException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create series {Title}", seriesVM.Title);
                return (false, "An unexpected error occurred while creating the series. Please check the category exists and try again.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteSeriesAsync(int id, string? actor = null)
        {
            try
            {
                var series = await _seriesRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (series == null)
                {
                    return (false, "Series Not Found");
                }

                // Remove the DB row first (soft delete) and only then the image file.
                if (await _seriesRepository.RemoveAsync(series))
                {
                    if (!string.IsNullOrEmpty(series.ImageUrl))
                    {
                        BLL.Helpers.Load.RemoveFile("Images", series.ImageUrl);
                    }
                    return (true, "Series Deleted Successfully");
                }
                return (false, "Failed to Delete Series");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete series {SeriesId}", id);
                return (false, "An unexpected error occurred while deleting the series.");
            }
        }

        public async Task<IEnumerable<SeriesVM>> GetAllSeriesAsync()
        {
            var series = await _seriesRepository.GetAllAsync(includeWords: "Category", orderBy: q => q.OrderByDescending(s => s.CreatedAt));
            return _mapper.Map<IEnumerable<SeriesVM>>(series);
        }

        public async Task<SeriesVM?> GetSeriesByIdAsync(int id)
        {
            var series = await _seriesRepository.GetFirstOrDefaultAsync(x => x.Id == id, includeWords: "Category");
            return series == null ? null : _mapper.Map<SeriesVM>(series);
        }

        public async Task<int> GetSeriesCountAsync()
        {
            return await _seriesRepository.CountAsync();
        }

        public async Task<(IEnumerable<SeriesVM> Series, int TotalCount)> SearchSeriesAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12)
        {
            var (items, totalCount) = await _seriesRepository.SearchAsync(searchTerm, categoryId, sortBy, page, pageSize);
            return (_mapper.Map<IEnumerable<SeriesVM>>(items), totalCount);
        }

        public async Task IncrementViewsAsync(int id)
        {
            await _seriesRepository.IncrementViewsAsync(id);
        }

        public async Task IncrementDownloadsAsync(int id)
        {
            await _seriesRepository.IncrementDownloadsAsync(id);
        }

        public async Task<bool> IsSeriesFreeAsync(int id)
        {
            var series = await _seriesRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return series?.IsFree ?? false;
        }

        public async Task<(bool Success, string Message)> UpdateSeriesAsync(SeriesVM seriesVM, int id, string? actor = null)
        {
            try
            {
                var series = await _seriesRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (series == null)
                {
                    return (false, "Series Not Found");
                }

                if (seriesVM.Image != null)
                {
                    var oldImage = series.ImageUrl;
                    var newImage = BLL.Helpers.Load.UploadImage(seriesVM.Image);
                    series.ImageUrl = newImage;

                    if (await _seriesRepository.UpdateAsync(series) && !string.IsNullOrEmpty(oldImage))
                    {
                        BLL.Helpers.Load.RemoveFile("Images", oldImage);
                    }
                }

                // Bug fix: the old repository update re-fetched the entity and had a
                // positional-argument bug that silently dropped IsFree. Views/Downloads
                // are preserved (they are incremented by watch/download actions).
                series.Update(
                    seriesVM.Title,
                    seriesVM.CategoryId,
                    description: seriesVM.Description,
                    isFree: seriesVM.IsFree,
                    imageUrl: series.ImageUrl,
                    updater: actor);

                if (await _seriesRepository.UpdateAsync(series))
                {
                    return (true, "Series Updated Successfully");
                }
                return (false, "Failed to Update Series");
            }
            catch (InvalidDataException ex)
            {
                return (false, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update series {SeriesId}", id);
                return (false, "An unexpected error occurred while updating the series.");
            }
        }
    }
}
