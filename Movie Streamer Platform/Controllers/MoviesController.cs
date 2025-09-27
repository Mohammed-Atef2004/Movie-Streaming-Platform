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


        public IActionResult Index(string search, string sortBy)
        {
            var movies = _movieService.GetAllMovies();

            if (!string.IsNullOrEmpty(search))
            {
                movies = movies
                    .Where(m => m.Title.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            switch (sortBy)
            {
                case "name_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "views":
                    movies = movies.OrderByDescending(m => m.Views);
                    break;
                case "downloads":
                    movies = movies.OrderByDescending(m => m.Downloads);
                    break;
                case "rating":
                    //movies = movies.OrderByDescending(m => m.Rating);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Title); // Default sort
                    break;
            }

            return View(movies.ToList());
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
