using Application.Services.Abstraction;
using Application.ViewModels;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserMovieService _userMovieService;
        private readonly IUserSeriesService _userSeriesService;

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager, IUserMovieService userMovieService, IUserSeriesService userSeriesService)
        {
            _accountService = accountService;
            _userManager = userManager;
            _userMovieService = userMovieService;
            _userSeriesService = userSeriesService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM vm)
        {
            var user = await _accountService.Register(vm);

            if (user is not null)
                return RedirectToAction("Login");

            return View(vm);
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM loginUserVM)
        {
            if (!ModelState.IsValid)
                return View(loginUserVM);

            bool result = await _accountService.Login(loginUserVM);

            if (result)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid UserName Or Password");

            return View(loginUserVM);
        }
        [HttpPost]
        public async Task<IActionResult> Logout(LoginUserVM loginUserVM)
        {
            await _accountService.Logout(loginUserVM);
            return RedirectToAction("Login");
        }

       
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            var vm = new ProfileVM
            {
                User = user,

                FavoriteMovies =
                    _userMovieService.GetFavorites(user.Id),

                WatchListMovies =
                    _userMovieService.GetWatchList(user.Id),

                FavoriteSeries =
                    _userSeriesService.GetFavorites(user.Id),

                WatchListSeries =
                    _userSeriesService.GetWatchList(user.Id)
            };

            return View(vm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(EditProfileVM vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            user.UserName = vm.UserName;
            user.Email = vm.Email;

            if (vm.Image is not null)
            {
                var fileName = Guid.NewGuid() +
                               Path.GetExtension(vm.Image.FileName);

                var path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/Images",
                    fileName);

                using var stream = new FileStream(path, FileMode.Create);

                await vm.Image.CopyToAsync(stream);

                user.ImageURL = fileName;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                return View(vm);
            }

            return RedirectToAction(nameof(Profile));
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
