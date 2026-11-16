using BLL.ViewModels;

namespace BLL.Services.Abstraction
{
    public interface ICategoryService
    {
        Task<(bool Success, string Message)> CreateCategoryAsync(CategoryVM categoryVM, string? actor = null);
        Task<(bool Success, string Message)> UpdateCategoryAsync(CategoryVM categoryVM, string? actor = null);
        Task<(bool Success, string Message)> DeleteCategoryAsync(int id, string? actor = null);
        Task<IEnumerable<CategoryVM>> GetAllCategoriesAsync();
        Task<CategoryVM?> GetCategoryByIdAsync(int id);
    }
}
