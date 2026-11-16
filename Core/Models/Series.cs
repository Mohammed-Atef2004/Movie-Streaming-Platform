using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Series : CommonData
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string? ImageUrl { get; set; }
        public int ViewCount { get; private set; }
        public bool? IsFree { get; set; } = true;
        public int DownloadCount { get; private set; }

        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public virtual ICollection<Episode> Episodes { get; set; }
            = new HashSet<Episode>();
        public virtual ICollection<UserSeries> UserSeries { get; set; }
            = new HashSet<UserSeries>();

        public void Create(string title, int viewCount, int downloadCount, int categoryId, string? description = null, bool? isFree = true, string? imageUrl = null, string? creator = null)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            ViewCount = viewCount;
            DownloadCount = downloadCount;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
            IsFree = isFree;
            CategoryId = categoryId;
        }

        public void Update(string title, int categoryId, string? description = null, bool? isFree = true, string? imageUrl = null, string? updater = null)
        {
            Title = title;
            Description = description;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
            IsFree = isFree;
            CategoryId = categoryId;
        }

        public void SetCounters(int viewCount, int downloadCount)
        {
            ViewCount = viewCount;
            DownloadCount = downloadCount;
        }

        public void IncrementViews() => ViewCount++;

        public void IncrementDownloads() => DownloadCount++;
    }
}
