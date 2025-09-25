using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using BLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Movie_Streamer_Platform.Areas.User.controller
{
    [Area("User")]
    public class BookmarkController : Controller
    {
        
        private readonly IBookmarkService bookmarkService;

        public BookmarkController(IBookmarkService bookmarkService)
        {
            bookmarkService = bookmarkService;
        }
        public IActionResult Bookmarks(int userId)
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

    }
}
