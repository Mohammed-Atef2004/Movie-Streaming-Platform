using Core.Models;
using Core.Repositories;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class SeriesRepository : GenericRepository<Series>, ISeriesRepository
    {
        public SeriesRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Series> Items, int TotalCount)> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.Series.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(s => s.Title.ToLower().Contains(searchTerm.Trim().ToLower()));

            if (categoryId.HasValue)
                query = query.Where(s => s.CategoryId == categoryId.Value);

            query = sortBy?.ToLowerInvariant() switch
            {
                "views" => query.OrderByDescending(s => s.ViewCount),
                "downloads" => query.OrderByDescending(s => s.DownloadCount),
                "newest" => query.OrderByDescending(s => s.CreatedAt),
                _ => query.OrderByDescending(s => s.CreatedAt),
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Include(s => s.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task IncrementViewsAsync(int seriesId, CancellationToken cancellationToken = default)
        {
            await _context.Series
                .Where(s => s.Id == seriesId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.ViewCount, x => x.ViewCount + 1), cancellationToken);
        }

        public async Task IncrementDownloadsAsync(int seriesId, CancellationToken cancellationToken = default)
        {
            await _context.Series
                .Where(s => s.Id == seriesId)
                .ExecuteUpdateAsync(s => s.SetProperty(x => x.DownloadCount, x => x.DownloadCount + 1), cancellationToken);
        }
    }
}
