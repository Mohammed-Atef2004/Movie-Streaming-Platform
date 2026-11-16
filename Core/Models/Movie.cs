using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Movie : CommonData
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int Views { get; set; }
        public int Downloads { get; set; }
        public bool IsFree { get; set; } = true;
        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        // Bug fix: removed the bogus [ForeignKey("ApplicationUser")] that was here -
        // these are plain URL strings, not foreign keys.
        public string? TrailerUrl { get; set; }
        public string? MovieUrl { get; set; }
        public double Rating { get; set; } = 5;
        public int Year { get; set; }

        public virtual ICollection<UserMovie> UserMovies { get; set; }
            = new HashSet<UserMovie>();
        public virtual ICollection<Review> UserReviews { get; set; }
            = new HashSet<Review>();

        public void Create(
            string title,
            string? description,
            string? imageUrl,
            bool isFree,
            int categoryId,
            string? trailerUrl,
            string? movieUrl,
            double rating,
            int year,
            string? creator = null)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            IsFree = isFree;
            CategoryId = categoryId;
            TrailerUrl = trailerUrl;
            MovieUrl = movieUrl;
            Rating = rating;
            Year = year;

            Views = 0;
            Downloads = 0;

            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
        }

        public void Update(
            string title,
            string? description,
            string? imageUrl,
            bool isFree,
            int categoryId,
            string? trailerUrl,
            string? movieUrl,
            double rating,
            int year,
            int views,
            int downloads,
            string? updater = null)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            IsFree = isFree;
            CategoryId = categoryId;
            TrailerUrl = trailerUrl;
            MovieUrl = movieUrl;
            Rating = rating;
            Year = year;

            Views = views;
            Downloads = downloads;

            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }

        public void IncrementViews() => Views++;

        public void IncrementDownloads() => Downloads++;
    }
}
