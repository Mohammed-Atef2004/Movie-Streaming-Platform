using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _categoryService.GetAllCategoriesAsync();
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = await _categoryService.CreateCategoryAsync(categoryVM, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", message);
                    return View(categoryVM);
                }
            }
            return View(categoryVM);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category != null)
            {
                return View(category);
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryVM categoryVM)
        {
            if (ModelState.IsValid)
            {
                var (isSuccess, message) = await _categoryService.UpdateCategoryAsync(categoryVM, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    TempData["Message"] = message;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", message);
                    return View(categoryVM);
                }
            }
            return View(categoryVM);
        }

        // Bug fix: the old GET action deleted immediately, which made the existing
        // confirmation view (Delete.cshtml) unreachable and allowed one-click deletion.
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return RedirectToAction("Index");
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var (_, message) = await _categoryService.DeleteCategoryAsync(id, actor: User.Identity?.Name);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }
    }
}
