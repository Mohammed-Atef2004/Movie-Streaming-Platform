using AutoMapper;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.User.controller
{
    [Area("User")]
    public class BookmarkController : Controller
    {
        
        private readonly IBookmarkService bookmarkService;
        private readonly UserManager<ApplicationUser> _userManager;


        public BookmarkController(IBookmarkService bookmarkService , UserManager<ApplicationUser> userManager)
        {
            this.bookmarkService = bookmarkService;
            _userManager = userManager;

        }
        public IActionResult Bookmarks(string userId)
        {

            var bookmarks = bookmarkService.GetUserBookmarks(userId);
            if (TempData["SuccessMessage"] != null)
                ViewBag.SuccessMessage = TempData["SuccessMessage"];
            if (TempData["ErrorMessage"] != null)
                ViewBag.ErrorMessage = TempData["ErrorMessage"];
            return View("Bookmarks", bookmarks);
        }
        [HttpPost]
        public IActionResult RemoveBookmark(BookmarkVM bookmarkVM)
        {
           
            var (success, message) = bookmarkService.RemoveBookmark(bookmarkVM);

           
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;

         
            return RedirectToAction(nameof(Bookmarks), new { userId = bookmarkVM.UserId });
        }
        [HttpPost]
        public IActionResult AddBookmark(BookmarkVM bookmarkVM)
        {
            var (success, message) = bookmarkService.AddBookmark(bookmarkVM);
            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;
            return RedirectToAction(nameof(Bookmarks), new { userId = bookmarkVM.UserId });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleBookmark(int movieId)
        {
            var userId = _userManager.GetUserId(User);
            //var userId = User.Fi


            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "You must be logged in to bookmark movies.";
                return RedirectToAction("Login", "Account", new { area = "" });
            }

            var (success, message) = bookmarkService.ToggleBookmark(userId, movieId);

            TempData[success ? "SuccessMessage" : "ErrorMessage"] = message;

            return RedirectToAction(nameof(Index));
        }

    }
}
