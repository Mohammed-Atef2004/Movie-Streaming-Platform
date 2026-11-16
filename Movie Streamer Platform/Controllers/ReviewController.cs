using Application.Services.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Movie_Streamer_Platform.Controllers
{
    [Authorize]
    public class ReviewController : Controller
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int movieId, string comment, int rating = 5)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return BadRequest("Review cannot be empty.");

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Rating is clamped to 1..5 by the review entity itself.
            var result = await _reviewService.CreateReviewAsync(userId, movieId, comment, rating);

            if (!result.Success)
                return BadRequest(result.Message);

            return Json(new
            {
                success = true,
                comment = comment,
                rating = Math.Clamp(rating, 1, 5),
                user = User.Identity?.Name ?? "User"
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMovieReviews(int movieId)
        {
            var reviews = await _reviewService.GetFilmReviewsAsync(movieId);
            return Json(reviews.Select(r => new
            {
                user = r.User?.UserName ?? "Unknown User",
                comment = r.Comment,
                rating = r.Rating,
                createdAt = r.CreatedAt
            }));
        }
    }
}
