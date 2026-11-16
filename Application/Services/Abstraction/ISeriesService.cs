using BLL.ViewModels;

namespace BLL.Services.Abstraction
{
    public interface ISeriesService
    {
        Task<(bool Success, string Message)> CreateSeriesAsync(SeriesVM seriesVM, string? actor = null);
        Task<(bool Success, string Message)> UpdateSeriesAsync(SeriesVM seriesVM, int id, string? actor = null);
        Task<(bool Success, string Message)> DeleteSeriesAsync(int id, string? actor = null);
        Task<IEnumerable<SeriesVM>> GetAllSeriesAsync();
        Task<SeriesVM?> GetSeriesByIdAsync(int id);
        Task<int> GetSeriesCountAsync();
        Task<(IEnumerable<SeriesVM> Series, int TotalCount)> SearchSeriesAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12);
        Task IncrementViewsAsync(int id);
        Task IncrementDownloadsAsync(int id);
        Task<bool> IsSeriesFreeAsync(int id);
    }
}
