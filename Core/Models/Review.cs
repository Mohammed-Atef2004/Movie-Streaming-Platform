using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Review : CommonData
    {
        private Review() { }
        private Review(string userId, int movieId, string comment, int rating)
        {
            UserId = userId;
            MovieId = movieId;
            Comment = comment;
            Rating = rating;
        }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        [ForeignKey(nameof(Movie))]
        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public string Comment { get; set; } = string.Empty;

        /// <summary>Star rating given with this review (1-5).</summary>
        public int Rating { get; set; } = 5;

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;

        public static Review Create(string userId, int movieId, string comment, int rating = 5)
        {
            // Keep the rating within sane bounds no matter what the caller sends.
            var clampedRating = Math.Clamp(rating, 1, 5);
            return new Review(userId, movieId, comment, clampedRating);
        }

        public void Update(string comment, int rating = 5)
        {
            Comment = comment;
            Rating = Math.Clamp(rating, 1, 5);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
