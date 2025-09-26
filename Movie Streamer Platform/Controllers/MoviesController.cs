using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using BLL.ViewModels;
using BLL.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie_Streamer_Platform.Controllers
{
    public class MoviesController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;

        public MoviesController(IMovieService movieService, ICategoryService categoryService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
        }


        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }
        public IActionResult Details(int id)
        {
            var movie = _movieService.GetMovieById(id); // service layer
            if (movie == null)
            {
                return NotFound();
            }
            return View(movie);
        }

    }
}
