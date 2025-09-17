using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MovieController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public MovieController(IMovieService movieService, IMapper mapper, IWebHostEnvironment webHostEnvironment)
        {
            _movieService = movieService;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult GetAll()
        {
            var movies=_movieService.GetAllMovies();
            return View(movies);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(MovieVM movieVM,IFormFile file)
        {
            if (ModelState.IsValid)
            {
                movieVM.Image= file;
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
            var movie=_movieService.GetMovieById(Id);
            if (movie != null)
            {
                return View(movie);
            }
            return NotFound();
        }
        [HttpPost]
        public IActionResult Edit(MovieVM movieVM, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                movieVM.Image = file;
                var (isSuccess, message) = _movieService.UpdateMovie(movieVM, movieVM.Id);
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
       
    }
  
}
