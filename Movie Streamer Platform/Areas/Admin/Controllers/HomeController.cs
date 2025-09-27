using BLL.Services.Abstraction;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]

    public class HomeController : Controller
    {
        private readonly IMovieService _movieService;
        private readonly ISeriesService _seriesService;
        UserManager<ApplicationUser> _userManager;
        //private readonly IUserMangerService _userService;
        //private readonly IPaymentService _paymentService;

        public HomeController(IMovieService movieService, ISeriesService seriesService, UserManager<ApplicationUser> userManager)
        {
            _movieService = movieService;
            _seriesService = seriesService;
            _userManager = userManager;
            //_paymentService = paymentService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //ViewBag.RecentMovies = recentMovies;
            ViewBag.MovieCount = _movieService.GetAllMovies().Count(); ;
            ViewBag.SeriesCount = _seriesService.GetAllSeries().Count();
            ViewBag.UserCount = await _userManager.Users.CountAsync();
            //ViewBag.PaymentCount = _paymentService.GetAll().Count();
            ViewBag.RecentMovies = _movieService.GetRecentMovies(5); // آخر 5 أفلام
            return View();
        }
    }

}
