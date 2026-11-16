using Core.Models;
using Core.Repositories;
using Infrastructure.Presistance.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> HasMoviesAsync(int categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Movies
                .AnyAsync(m => m.CategoryId == categoryId, cancellationToken);
        }
    }
}
