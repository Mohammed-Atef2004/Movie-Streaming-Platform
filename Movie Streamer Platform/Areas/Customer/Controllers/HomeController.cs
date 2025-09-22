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
        public IActionResult Movies()
        {
            var movies = _movieService.GetAllMovies();
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }
        public IActionResult Series()
        {
            var series = _seriesService.GetAllSeries();
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }
        public IActionResult MovieDetails(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }
        public IActionResult SeriesDetails(int id)
        {
            var series = _seriesService.GetSeriesById(id);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }
        public IActionResult MoviesSortedByViews()
        {
            var movies = _movieService.GetAllMovies().OrderByDescending(m => m.ViewCount);
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }
        public IActionResult SeriesSortedByViews()
        {
            var series = _seriesService.GetAllSeries().OrderByDescending(s => s.ViewCount);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }
        public IActionResult MoviesSortedByDownloads()
        {
            var movies = _movieService.GetAllMovies().OrderByDescending(m => m.DownloadCount);
            if (movies == null)
            {
                return NotFound();
            }
            return View(movies);
        }
        public IActionResult SeriesSortedByDownloads()
        {
            var series = _seriesService.GetAllSeries().OrderByDescending(s => s.DownloadCount);
            if (series == null)
            {
                return NotFound();
            }
            return View(series);
        }
    }
}
