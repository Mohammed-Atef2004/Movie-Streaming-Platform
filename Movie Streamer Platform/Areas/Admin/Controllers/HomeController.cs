using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;
        private readonly IEpisodeService _episodeService;
        private readonly ICategoryService _categoryService;

        public HomeController(
            IMovieService movieService,
            ISeriesService seriesService,
            IEpisodeService episodeService,
            ICategoryService categoryService)
        {
            _movieService = movieService;
            _seriesService = seriesService;
            _episodeService = episodeService;
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // Bug fix: the old dashboard materialised every row just to count
            // them (GetAllMovies().Count()); these are plain COUNT(*) queries now.
            ViewBag.MovieCount = await _movieService.GetMoviesCountAsync();
            ViewBag.SeriesCount = await _seriesService.GetSeriesCountAsync();
            ViewBag.EpisodeCount = (await _episodeService.GetAllEpisodesAsync()).Count();
            ViewBag.CategoryCount = (await _categoryService.GetAllCategoriesAsync()).Count();
            ViewBag.RecentMovies = await _movieService.GetRecentMoviesAsync(5);
            return View();
        }
    }
}
