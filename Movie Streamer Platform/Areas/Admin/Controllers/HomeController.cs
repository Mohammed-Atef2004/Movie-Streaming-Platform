using BLL.Services.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;
        //private readonly IUserService _userService;
        //private readonly IPaymentService _paymentService;

        public HomeController(IMovieService movieService, ISeriesService seriesService)
        {
            _movieService = movieService;
            _seriesService = seriesService;
            //_userService = userService;
            //_paymentService = paymentService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            //ViewBag.RecentMovies = recentMovies;
            ViewBag.MovieCount = _movieService.GetAllMovies().Count(); ;
            ViewBag.SeriesCount = _seriesService.GetAllSeries().Count();
            //ViewBag.UserCount = _userService.GetAll().Count();
            //ViewBag.PaymentCount = _paymentService.GetAll().Count();
            ViewBag.RecentMovies = _movieService.GetRecentMovies(5); // آخر 5 أفلام
            return View();
        }
    }

}
