using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Abstraction
{
    public interface IBookmarkRepository : IGenericRepository<Bookmark>
    {
        //bool Update(Bookmark bookmark);
        IEnumerable<Bookmark> GetUserBookmarks(Bookmark bookmark);
        bool AddBookmark(Bookmark bookmark);
        bool RemoveBookmark(Bookmark bookmark);
    }
}
