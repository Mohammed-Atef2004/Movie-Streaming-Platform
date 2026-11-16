using Application.Services.Abstraction;
using Application.ViewModels;
using BLL.Helpers;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserVM vm)
        {
            // Bug fix: the old code never checked ModelState and swallowed all
            // Identity errors, so an empty form just silently re-rendered.
            if (!ModelState.IsValid)
                return View(vm);

            var (user, error) = await _accountService.RegisterAsync(vm);

            if (user is not null)
            {
                TempData["Success"] = "Account created successfully. Please log in.";
                return RedirectToAction("Login");
            }

            ModelState.AddModelError(string.Empty, error ?? "Registration failed. Please try again.");
            return View(vm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserVM loginUserVM)
        {
            if (!ModelState.IsValid)
                return View(loginUserVM);

            bool result = await _accountService.LoginAsync(loginUserVM);

            if (result)
                return RedirectToAction("Index", "Home");

            ModelState.AddModelError("", "Invalid Username Or Password");

            return View(loginUserVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
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
                    await _userMovieService.GetFavoritesAsync(user.Id),

                WatchListMovies =
                    await _userMovieService.GetWatchListAsync(user.Id),

                FavoriteSeries =
                    await _userSeriesService.GetFavoritesAsync(user.Id),

                WatchListSeries =
                    await _userSeriesService.GetWatchListAsync(user.Id)
            };

            return View(vm);
        }

        // Bug fix: the GET action was missing entirely, so the edit form was
        // unreachable (the navbar linked to a 404).
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> EditProfile()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            var vm = new EditProfileVM
            {
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                CurrentImage = user.ImageURL
            };

            return View(vm);
        }

        [Authorize]
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
                try
                {
                    // Bug fix: uploads used to bypass validation and could accept any file.
                    var oldImage = user.ImageURL;
                    user.ImageURL = Load.UploadImage(vm.Image);
                    if (!string.IsNullOrEmpty(oldImage))
                        Load.RemoveFile("Images", oldImage);
                }
                catch (InvalidDataException ex)
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                    vm.CurrentImage = user.ImageURL;
                    return View(vm);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError("", error.Description);

                vm.CurrentImage = user.ImageURL;
                return View(vm);
            }

            TempData["Success"] = "Profile updated successfully.";
            return RedirectToAction(nameof(Profile));
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
