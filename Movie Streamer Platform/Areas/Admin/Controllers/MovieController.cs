using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;
        public MovieController(IMovieService movieService, ICategoryService categoryService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return View(movies);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            MovieVM movieVM = new();
            movieVM.CategoryList = await BuildCategoryListAsync();
            return View(movieVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MovieVM movieVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                movieVM.Image = file;
                var (isSuccess, message) = await _movieService.CreateMovieAsync(movieVM, actor: User.Identity?.Name);
                if (isSuccess)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    movieVM.CategoryList = await BuildCategoryListAsync(movieVM.CategoryId);
                    ModelState.AddModelError("", message);
                    return View(movieVM);
                }
            }
            movieVM.CategoryList = await BuildCategoryListAsync(movieVM.CategoryId);
            return View(movieVM);
        }

        // Bug fix: deletion used to be a plain GET link - one accidental URL visit
        // (or a prefetching browser) could destroy content. Now it is a POST with
        // an anti-forgery token and a confirmation dialog in the view.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var (_, message) = await _movieService.DeleteMovieAsync(id, actor: User.Identity?.Name);
            TempData["Message"] = message;
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie != null)
            {
                movie.CategoryList = await BuildCategoryListAsync(movie.CategoryId);
                return View(movie);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MovieVM movieVM, IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                movieVM.CategoryList = await BuildCategoryListAsync(movieVM.CategoryId);
                return View(movieVM);
            }

            if (file != null)
            {
                movieVM.Image = file;
            }

            var (isSuccess, message) = await _movieService.UpdateMovieAsync(movieVM, movieVM.Id, actor: User.Identity?.Name);
            if (isSuccess)
            {
                TempData["Message"] = message;
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", message);
            movieVM.CategoryList = await BuildCategoryListAsync(movieVM.CategoryId);

            return View(movieVM);
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
