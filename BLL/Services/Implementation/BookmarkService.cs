using AutoMapper;
using BLL.Services.Abstraction;
using BLL.ViewModels;
using DAL.Models;
using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.Implementation
{
    public class BookmarkService : IBookmarkService
    {
        private readonly IBookmarkRepository _bookmarkRepository;
        
        private readonly IMapper _mapper;
        public BookmarkService(IBookmarkRepository bookmarkRepository, IMapper mapper)
        { 
            _bookmarkRepository = bookmarkRepository;
            _mapper = mapper;
        }

        public (bool, string) AddBookmark(BookmarkVM bookmarkVM)
        {
            try
            {
                
                var bookmark = _mapper.Map<Bookmark>(bookmarkVM);

                
                var existing = _bookmarkRepository
                    .GetAll()
                    .FirstOrDefault(b => b.UserId == bookmark.UserId && b.MovieId == bookmark.MovieId);

                if (existing != null)
                    return (false, "Bookmark already exists.");

                _bookmarkRepository.Add(bookmark);
               

                return (true, "Bookmark added successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public IEnumerable<Bookmark> GetUserBookmarks(int userId)
        {
            try
            {
                return _bookmarkRepository
                    .GetAll()
                    .Where(b => b.UserId == userId)
                    .ToList();
            }
            catch
            {
                return new List<Bookmark>();
            }
        }

        public (bool, string) RemoveBookmark(BookmarkVM bookmarkVM)
        {
            try
            {
              
                var existing = _bookmarkRepository
                    .GetAll()
                    .FirstOrDefault(b => b.UserId == bookmarkVM.UserId && b.MovieId == bookmarkVM.MovieId);

                if (existing == null)
                    return (false, "Bookmark not found.");

                _bookmarkRepository.Remove(existing);
               

                return (true, "Bookmark removed successfully.");
            }
            catch (Exception ex)
            {
                return (false,  ex.Message);
            }
        }
    }
    
}
