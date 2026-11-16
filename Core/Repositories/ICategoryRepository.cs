using Core.Models;

namespace Core.Repositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        /// <summary>True when at least one (non-deleted) movie references the category.</summary>
        Task<bool> HasMoviesAsync(int categoryId, CancellationToken cancellationToken = default);
    }
}
