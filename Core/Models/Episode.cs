using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class Episode : CommonData
    {
        public int Id { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string? VideoUrl { get; private set; }
        public int? ViewCount { get; private set; }
        public int DownloadCount { get; private set; }

        [ForeignKey(nameof(Series))]
        public int SeriesId { get; set; }
        public Series? Series { get; set; }

        public void Create(string title, int seriesId, string? description = null, string? videoUrl = null, string? creator = null)
        {
            Title = title;
            SeriesId = seriesId;
            Description = description;
            VideoUrl = videoUrl;
            ViewCount = 0;
            DownloadCount = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
        }

        public void Update(string title, string? description = null, string? videoUrl = null, int? seriesId = null, string? updater = null)
        {
            Title = title;
            Description = description;
            VideoUrl = videoUrl;
            if (seriesId.HasValue)
                SeriesId = seriesId.Value;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }

        public void IncrementViews() => ViewCount = (ViewCount ?? 0) + 1;

        public void IncrementDownloads() => DownloadCount++;
    }
}
