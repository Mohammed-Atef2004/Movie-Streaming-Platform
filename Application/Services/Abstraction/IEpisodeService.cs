using BLL.ViewModels;

namespace BLL.Services.Abstraction
{
    public interface IEpisodeService
    {
        Task<(bool Success, string Message)> CreateEpisodeAsync(EpisodeVM episodeVM, string? actor = null);
        Task<(bool Success, string Message)> UpdateEpisodeAsync(EpisodeVM episodeVM, int id, string? actor = null);
        Task<(bool Success, string Message)> DeleteEpisodeAsync(int id, string? actor = null);
        Task<IEnumerable<EpisodeVM>> GetAllEpisodesAsync();
        Task<EpisodeVM?> GetEpisodeByIdAsync(int id);
        Task<IEnumerable<EpisodeVM>> GetEpisodesBySeriesAsync(int seriesId);
        Task IncrementEpisodeViewsAsync(int id);
        Task IncrementEpisodeDownloadsAsync(int id);
    }
}
