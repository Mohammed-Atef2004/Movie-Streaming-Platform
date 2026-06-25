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
        public IActionResult Create(int movieId, string comment)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var result = _reviewService.CreateReview(userId, movieId, comment);

            if (!result.Item1)
                return BadRequest(result.Item2);

            return Json(new
            {
                success = true,
                comment = comment,
                user = User.Identity.Name
            });
        }

        [HttpGet]
        public IActionResult GetMovieReviews(int movieId)
        {
            var reviews = _reviewService.GetFilmReviews(movieId)
                .Select(r => new
                {
                    user = r.User.UserName,
                    comment = r.Comment,
                    createdAt = r.CreatedAt
                });

            return Json(reviews);
        }
    }
}