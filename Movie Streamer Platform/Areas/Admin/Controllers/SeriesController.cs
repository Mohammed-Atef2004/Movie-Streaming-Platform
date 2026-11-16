using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class SeriesController : Controller
    {
        private readonly ISeriesService _seriesService;
        private readonly ICategoryService _categoryService;

        public SeriesController(ISeriesService seriesService, ICategoryService categoryService)
        {
            _seriesService = seriesService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var series = await _seriesService.GetAllSeriesAsync();
            return View(series);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            SeriesVM seriesVM = new();
            seriesVM.CategoryList = await BuildCategoryListAsync();
            return View(seriesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SeriesVM seriesVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                seriesVM.Image = file;
                var (isSuccess, message) = await _seriesService.CreateSeriesAsync(seriesVM, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    seriesVM.CategoryList = await BuildCategoryListAsync(seriesVM.CategoryId);
                    ModelState.AddModelError("", message);
                    return View(seriesVM);
                }
            }
            seriesVM.CategoryList = await BuildCategoryListAsync(seriesVM.CategoryId);
            return View(seriesVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (_, message) = await _seriesService.DeleteSeriesAsync(id, actor: User.Identity?.Name);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var series = await _seriesService.GetSeriesByIdAsync(id);
            if (series != null)
            {
                series.CategoryList = await BuildCategoryListAsync(series.CategoryId);
                return View(series);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SeriesVM seriesVM, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                seriesVM.CategoryList = await BuildCategoryListAsync(seriesVM.CategoryId);
                return View(seriesVM);
            }

            if (file != null)
            {
                seriesVM.Image = file;
            }

            var (isSuccess, message) = await _seriesService.UpdateSeriesAsync(seriesVM, seriesVM.Id, actor: User.Identity?.Name);
            if (isSuccess)
            {
                TempData["Message"] = message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", message);
            seriesVM.CategoryList = await BuildCategoryListAsync(seriesVM.CategoryId);

            return View(seriesVM);
        }

        private async Task<IEnumerable<SelectListItem>> BuildCategoryListAsync(int? selectedId = null)
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return categories.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = selectedId.HasValue && i.Id == selectedId.Value
            });
        }
    }
}
