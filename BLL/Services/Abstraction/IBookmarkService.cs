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
        IEnumerable<Bookmark> GetUserBookmarks(int userId);
    }
}
