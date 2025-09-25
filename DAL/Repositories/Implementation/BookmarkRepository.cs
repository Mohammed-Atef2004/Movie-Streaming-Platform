using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implementation
{
    public class BookmarkRepository : GenericRepository<Bookmark> , IBookmarkRepository
    {
        private readonly ApplicationDbContext _context;
        public BookmarkRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public bool AddBookmark(Bookmark bookmark)
        {
            if (_context.Bookmark.Any(b => b.UserId == bookmark.UserId && b.MovieId == bookmark.MovieId))
                return false;

            Add(bookmark);         
            _context.SaveChanges(); 
            return true;

        }

        public IEnumerable<Bookmark> GetUserBookmarks(Bookmark bookmark)
        {
            return _context.Bookmark
                            .Include(b => b.Movie)  
                            .Where(b => b.UserId == bookmark.UserId)
                            .ToList();
        }

        public bool RemoveBookmark(Bookmark bookmark)
        {
            var existing = _context.Bookmark
                          .FirstOrDefault(b => b.UserId == bookmark.UserId && b.MovieId == bookmark.MovieId);

            if (existing == null) return false;

            Remove(existing);
            _context.SaveChanges();
            return true;
        }
    }
}
