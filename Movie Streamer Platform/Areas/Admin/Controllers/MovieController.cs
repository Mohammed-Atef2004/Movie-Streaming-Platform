using AutoMapper;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]

    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ICategoryService _categoryService;
        public MovieController(IMovieService movieService, ICategoryService categoryService)
        {
            _movieService = movieService;
            _categoryService = categoryService;
        }

        //[HttpGet("1")]
        public IActionResult Index()
        {
            var movies = _movieService.GetAllMovies();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            MovieVM movieVM = new MovieVM();
            movieVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View(movieVM);
        }
        [HttpPost]
        public IActionResult Create(MovieVM movieVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                movieVM.Image = file;
                var (isSuccess, message) = _movieService.CreateMovie(movieVM);
                if (isSuccess)
                {
                    ViewBag.Success = message;
                    return RedirectToAction("GetAll");
                }
                else
                {
                    ViewBag.Error = message;
                    return View(movieVM);
                }
            }
            return View(movieVM);

        }
        public IActionResult Delete(int Id)
        {
            _movieService.DeleteMovie(Id);
            return RedirectToAction("GetAll");
        }
        [HttpGet]
        public IActionResult Edit(int Id)
        {
            var movie = _movieService.GetMovieById(Id);
            if (movie != null)
            {
                movie.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                return View(movie);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(MovieVM movieVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    movieVM.Image = file;
                }
            }

            var (isSuccess, message) = _movieService.UpdateMovie(movieVM, movieVM.Id);
            if (isSuccess)
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Error = message;
            }


            movieVM.CategoryList = _categoryService.GetAllCategories().Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString(),
                Selected = i.Id == movieVM.CategoryId
            });

            return View(movieVM);
        }

    }

}
