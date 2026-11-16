using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Core.Models;
using Core.Repositories;
using System.Linq.Expressions;
using Infrastructure.Presistance.Database;

namespace Infrastructure.Repositories.Implementation
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        protected virtual IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? predicate,
            string? includeWords,
            bool asNoTracking)
        {
            IQueryable<T> query = _dbSet;

            if (predicate != null)
                query = query.Where(predicate);

            if (!string.IsNullOrWhiteSpace(includeWords))
            {
                foreach (var include in includeWords.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
                    query = query.Include(include);
            }

            if (asNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        public virtual async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWords = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default)
        {
            var query = BuildQuery(predicate, includeWords, asNoTracking: true);
            if (orderBy != null)
                query = orderBy(query);
            return await query.ToListAsync(cancellationToken);
        }

        public virtual async Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWords = null,
            CancellationToken cancellationToken = default)
        {
            // Tracking query by default so the fetched entity can be updated/removed afterwards.
            var query = BuildQuery(predicate, includeWords, asNoTracking: false);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public virtual async Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(predicate, cancellationToken);
        }

        public virtual async Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default)
        {
            return predicate == null
                ? await _dbSet.CountAsync(cancellationToken)
                : await _dbSet.CountAsync(predicate, cancellationToken);
        }

        public virtual async Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            // The entity is expected to be tracked (fetched via GetFirstOrDefaultAsync);
            // Update() also re-attaches detached entities, which keeps the old behaviour working.
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            if (entity is CommonData auditable)
            {
                // Real soft delete: keep the row, flag it as deleted (a global query filter hides it).
                auditable.SoftDelete();
                _dbSet.Update(entity);
            }
            else
            {
                _dbSet.Remove(entity);
            }

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var materialized = entities as ICollection<T> ?? entities.ToList();

            foreach (var entity in materialized.OfType<CommonData>())
                entity.SoftDelete();

            if (materialized.All(e => e is CommonData))
                _dbSet.UpdateRange(materialized);
            else
                _dbSet.RemoveRange(materialized);

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}
