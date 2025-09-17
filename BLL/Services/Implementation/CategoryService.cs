using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using DAL.Repositories.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public (bool, string) CreateCategory(CategoryVM categoryVM)
        {
            var category = _mapper.Map<Category>(categoryVM);
            if (_categoryRepository.Add(category))
            {
                return (true, "Category Created Successfully");
            }
            return (false, "Failed to Create Category");
        }

        public (bool, string) DeleteCategory(int id)
        {
            var category = _categoryRepository.GetFirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                return (false, "Category Not Found");
            }
            if (_categoryRepository.Remove(category))
            {
                return (true, "Category Deleted Successfully");
            }
            return (false, "Failed to Delete Category");

        }

        public IEnumerable<CategoryVM> GetAllCategories()
        {
            var categories = _categoryRepository.GetAll();
            return _mapper.Map<IEnumerable<CategoryVM>>(categories);
        }

        public CategoryVM GetCategoryById(int id)
        {
            var category = _categoryRepository.GetFirstOrDefault(x => x.Id == id);
            return _mapper.Map<CategoryVM>(category);
        }

        public (bool, string) UpdateCategory(CategoryVM categoryVM)
        {
            var categroy = _categoryRepository.GetFirstOrDefault(x => x.Id == categoryVM.Id);
            if (categroy != null)
            {
                categroy.Update(categoryVM.Name, categoryVM.Description);
                _categoryRepository.Update(categroy);
                return (true, "Category Updated Successfully");
            }
            return (false, "Failed to Update the Category");
        }
    }
}
