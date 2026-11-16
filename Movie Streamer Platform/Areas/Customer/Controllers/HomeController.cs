using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;
        public HomeController(IMovieService movieService, ISeriesService seriesService)
        {
            _movieService = movieService;
            _seriesService = seriesService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // Bug fix: these actions used to render views that did not exist in the
        // Customer area (Series/MovieDetails/SeriesDetails -> 500) or duplicated
        // the public browse pages with weaker features. They now delegate to the
        // real, searchable/paged pages under /Movies and /Series.

        public async Task<IActionResult> Movies()
        {
            var movies = await _movieService.GetAllMoviesAsync();
            return View(movies);
        }

        public IActionResult Series()
        {
            return RedirectToAction("Index", "Series", new { area = "" });
        }

        public IActionResult MovieDetails(int id)
        {
            return RedirectToAction("Details", "Movies", new { area = "", id });
        }

        public IActionResult SeriesDetails(int id)
        {
            return RedirectToAction("Details", "Series", new { area = "", id });
        }

        public IActionResult MoviesSortedByViews()
        {
            return RedirectToAction("Index", "Movies", new { area = "", sortBy = "views" });
        }

        public IActionResult SeriesSortedByViews()
        {
            return RedirectToAction("Index", "Series", new { area = "", sortBy = "views" });
        }

        public IActionResult MoviesSortedByDownloads()
        {
            return RedirectToAction("Index", "Movies", new { area = "", sortBy = "downloads" });
        }

        public IActionResult SeriesSortedByDownloads()
        {
            return RedirectToAction("Index", "Series", new { area = "", sortBy = "downloads" });
        }
    }
}
