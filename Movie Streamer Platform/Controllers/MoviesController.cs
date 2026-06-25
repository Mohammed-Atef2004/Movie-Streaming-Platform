using Application.Services.Abstraction;
using BLL.Services;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            ViewBag.Reviews = _reviewService.GetFilmReviews(id);
            var movie = _movieService.GetMovieById(id);

            if (movie == null)
                return NotFound();
            ViewBag.IsFavorite = false;
            ViewBag.IsInWatchList = false;

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = User.FindFirst(
                    System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

                ViewBag.IsFavorite =
                    _userMovieService.IsFavorite(userId, id);

                ViewBag.IsInWatchList =
                    _userMovieService.IsInWatchList(userId, id);
            }

            return View(movie);
        }
        [HttpPost]
        public IActionResult ToggleFavorite(int movieId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (_userMovieService.IsFavorite(userId, movieId))
                _userMovieService.RemoveFromFavorites(userId, movieId);
            else
                _userMovieService.AddToFavorites(userId, movieId);

            return RedirectToAction(nameof(Details), new { id = movieId });
        }
        [HttpPost]
        public IActionResult ToggleWatchList(int movieId)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var userId = User.FindFirst(
                System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;

            if (_userMovieService.IsInWatchList(userId, movieId))
                _userMovieService.RemoveFromWatchList(userId, movieId);
            else
                _userMovieService.AddToWatchList(userId, movieId);

            return RedirectToAction(nameof(Details), new { id = movieId });
        }

    }
}
