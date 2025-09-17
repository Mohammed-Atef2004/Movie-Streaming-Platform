using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
       public bool Update(Category category)
        {
            var categoryFromDb = _context.Categories.FirstOrDefault(u => u.Id == category.Id);
            if (categoryFromDb != null)
            {
                categoryFromDb.Update(category.Name, category.Description);
                _context.Categories.Update(categoryFromDb);
                if (_context.SaveChanges() > 0)
                {
                    return true;
                }
                else return false;
            }
            return false;
        }

        
    }
}
