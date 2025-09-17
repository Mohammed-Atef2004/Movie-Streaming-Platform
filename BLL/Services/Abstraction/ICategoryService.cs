using BLL.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface ICategoryService
    {
        (bool, string) CreateCategory(CategoryVM categoryVM);
        (bool, string) UpdateCategory(CategoryVM categoryVM);
        (bool, string) DeleteCategory(int id);
        CategoryVM GetCategoryById(int id);
        IEnumerable<CategoryVM> GetAllCategories();
    }
}
