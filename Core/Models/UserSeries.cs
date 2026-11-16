namespace Core.Models
{
    public class UserSeries
    {
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int SeriesId { get; set; }
        public Series? Series { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsInWatchList { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
