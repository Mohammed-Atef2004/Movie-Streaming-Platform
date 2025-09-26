using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManger;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManger, SignInManager<ApplicationUser> singInManager)
        {
            _userManger = userManger;
            _signInManager = singInManager;

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterUserVM registerUserVM)
        {
            var user = new ApplicationUser()
            {
                UserName = registerUserVM.UserName,
                Email = registerUserVM.Email,

            };

            var result = await _userManger.CreateAsync(user, registerUserVM.Password);

            if (result.Succeeded)
            {
                await _userManger.AddToRoleAsync(user, "User");
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("Password", item.Description);
                }
                return View(registerUserVM);
            }
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginUserVM loginUserVMl)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginUserVMl.UserName, loginUserVMl.Password, loginUserVMl.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid UserName Or Password");
                    return View(loginUserVMl);
                }
            }
            return View(loginUserVMl);
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userManger.GetUserAsync(User);
            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
