using BLL.Services;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult View(int Id)
        {
            var movie=_movieService.GetMovieById(Id);
            if (movie == null)
            {
                return NotFound();
            }
            movie.Views = movie.Views + 1;
            _movieService.UpdateMovie(movie, movie.Id);
            return View(movie);
        }
        public IActionResult Download(int id)
        {
            var movie = _movieService.GetMovieById(id);
            if (movie == null || string.IsNullOrEmpty(movie.ImageUrl))
            {
                return NotFound("No File Exist");
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Images", movie.ImageUrl);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }
            var fileBytes = System.IO.File.ReadAllBytes(filePath);
            var fileName = Path.GetFileName(filePath);
            movie.Downloads = movie.Downloads + 1;
            _movieService.UpdateMovie(movie, movie.Id);
            return File(fileBytes, "application/octet-stream", fileName);

        }
    }

}
