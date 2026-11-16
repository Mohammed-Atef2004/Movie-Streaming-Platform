using Application.Services.Abstraction;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Movie_Streamer_Platform.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;
        private readonly IUserMovieService _userMovieService;
        private readonly IReviewService _reviewService;

        public MoviesController(
            IMovieService movieService,
            ICategoryService categoryService,
            IUserMovieService userMovieService,
            IReviewService reviewService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
            _userMovieService = userMovieService;
            _reviewService = reviewService;
        }

        // Browse + server-side search/sort/pagination (the filter form already
        // existed in the view but the backend never read its parameters).
        public async Task<IActionResult> Index(string? search, int? categoryId, string? sortBy, int page = 1)
        {
            const int pageSize = 12;

            var (movies, totalCount) = await _movieService.SearchMoviesAsync(search, categoryId, sortBy, page, pageSize);

            ViewBag.Search = search;
            ViewBag.SortBy = sortBy;
            ViewBag.CategoryId = categoryId;
            ViewBag.Page = page;
            ViewBag.PageSize = pageSize;
            ViewBag.TotalCount = totalCount;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
            ViewBag.Categories = await _categoryService.GetAllCategoriesAsync();

            return View(movies);
        }

        public async Task<IActionResult> Details(int id)
        {
            // Bug fix: Reviews used to be fetched before checking the movie exists.
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound();

            ViewBag.Reviews = await _reviewService.GetFilmReviewsAsync(id);
            ViewBag.IsFavorite = false;
            ViewBag.IsInWatchList = false;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                ViewBag.IsFavorite = await _userMovieService.IsFavoriteAsync(userId, id);
                ViewBag.IsInWatchList = await _userMovieService.IsInWatchListAsync(userId, id);
            }

            return View(movie);
        }

        /// <summary>
        /// Watch page (video player). Premium (non-free) movies require login -
        /// this is where a payment/subscription check plugs in later.
        /// </summary>
        [Authorize]
        public async Task<IActionResult> Watch(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null)
                return NotFound();

            if (string.IsNullOrEmpty(movie.MovieUrl))
            {
                TempData["Error"] = "This movie has no video available yet.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await _movieService.IncrementViewsAsync(id);
            return View(movie);
        }

        /// <summary>Streams the movie file when stored locally, otherwise redirects to its URL.</summary>
        [Authorize]
        public async Task<IActionResult> Download(int id)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            if (movie == null || string.IsNullOrEmpty(movie.MovieUrl))
                return NotFound();

            if (!movie.IsFree && !User.IsInRole("Admin"))
            {
                TempData["Error"] = "Downloading premium movies requires a subscription.";
                return RedirectToAction(nameof(Details), new { id });
            }

            await _movieService.IncrementDownloadsAsync(id);

            if (Uri.IsWellFormedUriString(movie.MovieUrl, UriKind.Absolute) &&
                (movie.MovieUrl.StartsWith("http://") || movie.MovieUrl.StartsWith("https://")))
            {
                return Redirect(movie.MovieUrl);
            }

            var fileName = Path.GetFileName(movie.MovieUrl);
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Videos", fileName);
            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/octet-stream", fileName);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userMovieService.IsFavoriteAsync(userId, movieId))
                await _userMovieService.RemoveFromFavoritesAsync(userId, movieId);
            else
                await _userMovieService.AddToFavoritesAsync(userId, movieId);

            // The Details page toggles favorites via AJAX; only fall back to a
            // redirect for regular (form) posts.
            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                return Ok(new { success = true });

            return RedirectToAction(nameof(Details), new { id = movieId });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleWatchList(int movieId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (await _userMovieService.IsInWatchListAsync(userId, movieId))
                await _userMovieService.RemoveFromWatchListAsync(userId, movieId);
            else
                await _userMovieService.AddToWatchListAsync(userId, movieId);

            if (Request.Headers.XRequestedWith == "XMLHttpRequest")
                return Ok(new { success = true });

            return RedirectToAction(nameof(Details), new { id = movieId });
        }
    }
}
