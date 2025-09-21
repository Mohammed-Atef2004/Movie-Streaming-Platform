using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Episode : CommonData
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public string? VideoUrl { get; private set; }
        public int? ViewCount { get; private set; }
        public int DownloadCount { get; private set; }

        [ForeignKey("Series")]
        public int SeriesId { get; set; }
        public Series? Series { get; set; }

        public void Create(string title, string? description = null, string? videoUrl = null, string? creator = null)
        {
            Title = title;
            Description = description;
            VideoUrl = videoUrl;
            ViewCount = 0;
            DownloadCount = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = creator;
        }

        public void Update(string title, string? description = null, string? videoUrl = null, string? updater = null)
        {
            Title = title;
            Description = description;
            VideoUrl = videoUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }
    }

}
