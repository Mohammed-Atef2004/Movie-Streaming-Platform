// Areas/Admin/Controllers/UserController.cs

using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Movie_Streamer_Platform.Areas.Admin.Controllers
{
    [Area("Admin")]
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
            var users = await _userManager.Users.ToListAsync();
            var userList = new List<UserVM>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userList.Add(new UserVM
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
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
                UserName = user.UserName,
                Email = user.Email,
                CurrentRoles = userRoles.ToList(),
                AllRoles = allRoles
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRoles(EditUserRolesVM model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (!result.Succeeded) { /* handle errors */ }

            result = await _userManager.AddToRolesAsync(user, model.CurrentRoles);

            if (!result.Succeeded) { /* handle errors */ }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    // handle error
                }
            }
            return RedirectToAction(nameof(Index));
        }
    }
}