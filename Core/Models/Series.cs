using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Series: CommonData
    {
        public int Id { get; private set; }
        public string Title { get; private set; }
        public string? Description { get; private set; }
        public string? ImageUrl { get;  set; }
        public int ViewCount { get; private set; }
        public bool? IsFree { get; set; } = true;

        public int DownloadCount { get; private set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public void Create(string title, int viewCount, int downloadCount,int CategoryId, string? discription = null,bool? isFree=true ,string? imageUrl = null,string ?createor=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            ViewCount = viewCount;
            DownloadCount = downloadCount;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createor;
            IsFree = isFree;
            this.CategoryId = CategoryId;
        }
        public void Update( string title,int viewCount,int downloadCount, int CategoryId, string? discription = null, bool? isFree = true, string? imageUrl = null,string ?updater=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
            IsFree = isFree;
            this.ViewCount = viewCount;
            this.DownloadCount = downloadCount;
            this.CategoryId = CategoryId;
        }
    }
}
