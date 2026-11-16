using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class EpisodeService : IEpisodeService
    {
        private readonly IMapper _mapper;
        private readonly IEpisodeRepository _episodeRepository;
        private readonly ILogger<EpisodeService> _logger;

        public EpisodeService(IMapper mapper, IEpisodeRepository episodeRepository, ILogger<EpisodeService> logger)
        {
            _mapper = mapper;
            _episodeRepository = episodeRepository;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> CreateEpisodeAsync(EpisodeVM episodeVM, string? actor = null)
        {
            try
            {
                var episode = new Episode();
                episode.Create(
                    episodeVM.Title,
                    episodeVM.SeriesId,
                    description: episodeVM.Description,
                    videoUrl: episodeVM.VideoUrl,
                    creator: actor);

                if (await _episodeRepository.AddAsync(episode))
                {
                    return (true, "Episode Created Successfully");
                }
                return (false, "Failed to Create Episode");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create episode {Title}", episodeVM.Title);
                return (false, "An unexpected error occurred while creating the episode. Please check the series exists and try again.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteEpisodeAsync(int id, string? actor = null)
        {
            try
            {
                var episode = await _episodeRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (episode == null)
                {
                    return (false, "Episode Not Found");
                }

                if (await _episodeRepository.RemoveAsync(episode))
                {
                    return (true, "Episode Deleted Successfully");
                }
                return (false, "Failed to Delete Episode");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete episode {EpisodeId}", id);
                return (false, "An unexpected error occurred while deleting the episode.");
            }
        }

        public async Task<IEnumerable<EpisodeVM>> GetAllEpisodesAsync()
        {
            var episodes = await _episodeRepository.GetAllAsync(includeWords: "Series", orderBy: q => q.OrderByDescending(e => e.SeriesId).ThenBy(e => e.Id));
            return _mapper.Map<IEnumerable<EpisodeVM>>(episodes);
        }

        public async Task<EpisodeVM?> GetEpisodeByIdAsync(int id)
        {
            var episode = await _episodeRepository.GetFirstOrDefaultAsync(x => x.Id == id, includeWords: "Series");
            return episode == null ? null : _mapper.Map<EpisodeVM>(episode);
        }

        public async Task<IEnumerable<EpisodeVM>> GetEpisodesBySeriesAsync(int seriesId)
        {
            var episodes = await _episodeRepository.GetBySeriesAsync(seriesId);
            return _mapper.Map<IEnumerable<EpisodeVM>>(episodes);
        }

        public async Task IncrementEpisodeViewsAsync(int id)
        {
            await _episodeRepository.IncrementViewsAsync(id);
        }

        public async Task IncrementEpisodeDownloadsAsync(int id)
        {
            await _episodeRepository.IncrementDownloadsAsync(id);
        }

        public async Task<(bool Success, string Message)> UpdateEpisodeAsync(EpisodeVM episodeVM, int id, string? actor = null)
        {
            try
            {
                var episode = await _episodeRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (episode == null)
                {
                    return (false, "Episode Not Found");
                }

                // Bug fix: the old implementation never updated VideoUrl or SeriesId.
                episode.Update(
                    episodeVM.Title,
                    description: episodeVM.Description,
                    videoUrl: episodeVM.VideoUrl,
                    seriesId: episodeVM.SeriesId,
                    updater: actor);

                if (await _episodeRepository.UpdateAsync(episode))
                {
                    return (true, "Episode Updated Successfully");
                }
                return (false, "Failed to Update Episode");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update episode {EpisodeId}", id);
                return (false, "An unexpected error occurred while updating the episode.");
            }
        }
    }
}
