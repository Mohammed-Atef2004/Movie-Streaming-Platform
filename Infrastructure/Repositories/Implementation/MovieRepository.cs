using Core.Models;
using Core.Repositories;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories.Implementation
{
    public class MovieRepository : GenericRepository<Movie>, IMovieRepository
    {
        public MovieRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<(IEnumerable<Movie> Items, int TotalCount)> SearchAsync(
            string? searchTerm = null,
            int? categoryId = null,
            string? sortBy = null,
            int page = 1,
            int pageSize = 12,
            CancellationToken cancellationToken = default)
        {
            page = Math.Max(1, page);
            pageSize = Math.Clamp(pageSize, 1, 100);

            var query = _context.Movies.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(m => m.Title!.ToLower().Contains(searchTerm.Trim().ToLower()));

            if (categoryId.HasValue)
                query = query.Where(m => m.CategoryId == categoryId.Value);

            query = sortBy?.ToLowerInvariant() switch
            {
                "views" => query.OrderByDescending(m => m.Views),
                "downloads" => query.OrderByDescending(m => m.Downloads),
                "rating" => query.OrderByDescending(m => m.Rating),
                "newest" => query.OrderByDescending(m => m.CreatedAt),
                _ => query.OrderByDescending(m => m.CreatedAt),
            };

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Include(m => m.Category)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task IncrementViewsAsync(int movieId, CancellationToken cancellationToken = default)
        {
            await _context.Movies
                .Where(m => m.Id == movieId)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.Views, m => m.Views + 1), cancellationToken);
        }

        public async Task IncrementDownloadsAsync(int movieId, CancellationToken cancellationToken = default)
        {
            await _context.Movies
                .Where(m => m.Id == movieId)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.Downloads, m => m.Downloads + 1), cancellationToken);
        }

        public async Task UpdateRatingAsync(int movieId, double rating, CancellationToken cancellationToken = default)
        {
            await _context.Movies
                .Where(m => m.Id == movieId)
                .ExecuteUpdateAsync(s => s.SetProperty(m => m.Rating, rating), cancellationToken);
        }
    }
}
