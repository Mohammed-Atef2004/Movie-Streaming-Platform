using System.Diagnostics;
using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Movie_Streamer_Platform.Models;

namespace Movie_Streamer_Platform.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMovieService _movieService;

        public HomeController(ILogger<HomeController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;
        }

        public async Task<IActionResult> Index()
        {
            var (movies, _) = await _movieService.SearchMoviesAsync(page: 1, pageSize: 8);
            return View(movies);
        }

        public IActionResult Privacy()
        {
            return View();
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
