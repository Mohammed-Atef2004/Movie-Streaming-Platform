namespace Core.Models
{
    public class UserMovie
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int MovieId { get; set; }
        public Movie? Movie { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsInWatchList { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
