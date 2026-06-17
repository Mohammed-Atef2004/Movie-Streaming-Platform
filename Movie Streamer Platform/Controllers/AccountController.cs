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

        public AccountController(IAccountService accountService, UserManager<ApplicationUser> userManager)
        {
            _accountService = accountService;
            _userManager = userManager;
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

            return View(user);
        }
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
