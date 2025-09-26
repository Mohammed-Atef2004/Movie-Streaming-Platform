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
            throw new NotImplementedException();
        }

        public async Task<(bool, string)> AddBookmarkAsync(string userId, int movieId)
        {
            try
            {
                var existing = await _bookmarkRepository
                    .FindSingleAsync(b => b.UserId == userId && b.MovieId == movieId);

                if (existing != null)
                {
                    return (false, "Bookmark already exists.");
                }

                var newBookmark =  Bookmark.Create(userId, movieId);
                _bookmarkRepository.Add(newBookmark);
                await _bookmarkRepository.SaveAsync();

                return (true, "Bookmark added successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public IEnumerable<BookmarkVM> GetUserBookmarks(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<BookmarkVM>> GetUserBookmarksAsync(string userId)
        {
            try
            {
                var bookmarks = await _bookmarkRepository
                    .GetAllAsync(filter: b => b.UserId == userId, includeProperties: "Movie");

                // Assuming your BookmarkVM is correctly configured in AutoMapper
                var bookmarkVms = _mapper.Map<IEnumerable<BookmarkVM>>(bookmarks);

                return bookmarkVms;
            }
            catch
            {
                return new List<BookmarkVM>();
            }
        }

        public (bool, string) RemoveBookmark(BookmarkVM bookmarkVM)
        {
            throw new NotImplementedException();
        }

        public async Task<(bool, string)> RemoveBookmarkAsync(string userId, int movieId)
        {
            try
            {
                var existing = await _bookmarkRepository
                    .FindSingleAsync(b => b.UserId.Equals(userId) && b.MovieId == movieId);

                if (existing == null)
                {
                    return (false, "Bookmark not found.");
                }

                _bookmarkRepository.Remove(existing);
                await _bookmarkRepository.SaveAsync();

                return (true, "Bookmark removed successfully.");
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        public (bool success, string message) ToggleBookmark(string userId, int movieId)
        {
            throw new NotImplementedException();
        }



        public async Task<(bool success, string message)> ToggleBookmarkAsync(string userId, int movieId)
        {
            var existingBookmark = await _bookmarkRepository.FindSingleAsync(
                b => b.UserId == userId && b.MovieId == movieId
            );

            if (existingBookmark != null)
            {
                _bookmarkRepository.Remove(existingBookmark);
                await _bookmarkRepository.SaveAsync();
                return (true, "Bookmark removed successfully.");
            }
            else
            {
                var newBookmark = Bookmark.Create(userId, movieId);
                _bookmarkRepository.Add(newBookmark);
                await _bookmarkRepository.SaveAsync();
                return (true, "Movie bookmarked successfully!");
            }
        }
    }
}