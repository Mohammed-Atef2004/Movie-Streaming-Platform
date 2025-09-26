using BLL.ViewModels;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Abstraction
{
    public interface IBookmarkService
    {
        (bool, string) AddBookmark(BookmarkVM bookmarkVM);
        (bool, string) RemoveBookmark(BookmarkVM bookmarkVM);
        IEnumerable<BookmarkVM> GetUserBookmarks(string userId);
        (bool success, string message) ToggleBookmark(string userId, int movieId);
        public Task<(bool success, string message)> ToggleBookmarkAsync(string userId, int movieId);
    }
}
