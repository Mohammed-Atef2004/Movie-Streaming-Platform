using BLL.ViewModels;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    // SECURITY FIX: this controller previously had no [Authorize] at all - any
    // anonymous visitor could list users, change roles and delete accounts.
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // GET: Admin/User
        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.OrderBy(u => u.UserName).ToListAsync();
            var userList = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName ?? string.Empty,
                    Email = user.Email ?? string.Empty,
                    Roles = roles.ToList(),
                    IsBanned = await _userManager.IsLockedOutAsync(user)
                });
            }
            return View(userList);
        }

        // GET: Admin/User/ManageRoles/{id}
        public async Task<IActionResult> ManageRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var allRoles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var model = new EditUserRolesVM
            {
                UserId = user.Id,
                UserName = user.UserName ?? string.Empty,
                Email = user.Email ?? string.Empty,
                CurrentRoles = userRoles.ToList(),
                AllRoles = allRoles
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRoles(EditUserRolesVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (result.Succeeded)
                result = await _userManager.AddToRolesAsync(user, model.CurrentRoles ?? new List<string>());

            if (!result.Succeeded)
            {
                TempData["Message"] = string.Join(" ", result.Errors.Select(e => e.Description));
                return RedirectToAction(nameof(ManageRoles), new { id = model.UserId });
            }

            TempData["Message"] = $"Roles updated for {user.UserName}.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string id)
        {
            // Guard: an admin must not delete their own account while logged in.
            if (id == User.FindFirstValue(System.Security.Claims.ClaimTypes.NameIdentifier))
            {
                TempData["Message"] = "You cannot delete your own account while logged in.";
                return RedirectToAction(nameof(Index));
            }

            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    TempData["Message"] = string.Join(" ", result.Errors.Select(e => e.Description));
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
