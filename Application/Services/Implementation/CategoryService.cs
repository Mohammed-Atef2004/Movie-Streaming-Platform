using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Logging;

namespace BLL.Services.Implementation
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<(bool Success, string Message)> CreateCategoryAsync(CategoryVM categoryVM, string? actor = null)
        {
            try
            {
                var category = new Category();
                category.Create(categoryVM.Name, categoryVM.Description, creator: actor);

                if (await _categoryRepository.AddAsync(category))
                {
                    return (true, "Category Created Successfully");
                }
                return (false, "Failed to Create Category");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create category {Name}", categoryVM.Name);
                return (false, "An unexpected error occurred while creating the category.");
            }
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(int id, string? actor = null)
        {
            try
            {
                var category = await _categoryRepository.GetFirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                {
                    return (false, "Category Not Found");
                }

                // Guard: don't leave movies pointing at a deleted category.
                if (await _categoryRepository.HasMoviesAsync(id))
                {
                    return (false, "This category has movies assigned to it. Move or delete them first.");
                }

                if (await _categoryRepository.RemoveAsync(category))
                {
                    return (true, "Category Deleted Successfully");
                }
                return (false, "Failed to Delete Category");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete category {CategoryId}", id);
                return (false, "An unexpected error occurred while deleting the category.");
            }
        }

        public async Task<IEnumerable<CategoryVM>> GetAllCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync(orderBy: q => q.OrderBy(c => c.Name));
            return _mapper.Map<IEnumerable<CategoryVM>>(categories);
        }

        public async Task<CategoryVM?> GetCategoryByIdAsync(int id)
        {
            var category = await _categoryRepository.GetFirstOrDefaultAsync(x => x.Id == id);
            return category == null ? null : _mapper.Map<CategoryVM>(category);
        }

        public async Task<(bool Success, string Message)> UpdateCategoryAsync(CategoryVM categoryVM, string? actor = null)
        {
            try
            {
                var category = await _categoryRepository.GetFirstOrDefaultAsync(x => x.Id == categoryVM.Id);
                if (category == null)
                {
                    return (false, "Category Not Found");
                }

                category.Update(categoryVM.Name, categoryVM.Description, updater: actor);

                if (await _categoryRepository.UpdateAsync(category))
                {
                    return (true, "Category Updated Successfully");
                }
                return (false, "Failed to Update the Category");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update category {CategoryId}", categoryVM.Id);
                return (false, "An unexpected error occurred while updating the category.");
            }
        }
    }
}
