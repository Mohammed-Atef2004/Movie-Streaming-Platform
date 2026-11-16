using Core.Models;
using Core.Repositories;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class EpisodeRepository : GenericRepository<Episode>, IEpisodeRepository
    {
        public EpisodeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Episode>> GetBySeriesAsync(int seriesId, CancellationToken cancellationToken = default)
        {
            return await _context.Episodes
                .AsNoTracking()
                .Where(e => e.SeriesId == seriesId)
                .OrderBy(e => e.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task IncrementViewsAsync(int episodeId, CancellationToken cancellationToken = default)
        {
            await _context.Episodes
                .Where(e => e.Id == episodeId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.ViewCount, x => (x.ViewCount ?? 0) + 1), cancellationToken);
        }

        public async Task IncrementDownloadsAsync(int episodeId, CancellationToken cancellationToken = default)
        {
            await _context.Episodes
                .Where(e => e.Id == episodeId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.DownloadCount, x => x.DownloadCount + 1), cancellationToken);
        }
    }
}
