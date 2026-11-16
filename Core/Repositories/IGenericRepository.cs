using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWords = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            CancellationToken cancellationToken = default);

        Task<T?> GetFirstOrDefaultAsync(
            Expression<Func<T, bool>>? predicate = null,
            string? includeWords = null,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsAsync(
            Expression<Func<T, bool>> predicate,
            CancellationToken cancellationToken = default);

        Task<int> CountAsync(
            Expression<Func<T, bool>>? predicate = null,
            CancellationToken cancellationToken = default);

        Task<bool> AddAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> RemoveAsync(T entity, CancellationToken cancellationToken = default);
        Task<bool> RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    }
}
