using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Movie : CommonData
    {
        public int Id { get;  set; }
        public string? Title { get;  set; }
        public string? Description { get;  set; }
        public string? ImageUrl { get; set; }
        public int Views { get;  set; }
        public int Downloads { get;  set; }
        public bool? IsFree { get; set; } = true;
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string TrailerUrl { get; set; }

        public void Create(string title, int CategoryId, string? discription = null, bool? isFree = true, string? imageUrl = null, string? createor = null,string? trailerUrl=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            Views = 0;
            Downloads = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createor;
            IsFree = isFree;
            this.CategoryId = CategoryId;
            TrailerUrl = trailerUrl;
        }

        public void Update(string title, int viewCount, int downloadCount, int CategoryId, string? discription = null, bool? isFree = true, string? imageUrl = null, string? updater = null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
            IsFree = isFree; // Bug Fix: was "IsFree = IsFree" (property assigned to itself, parameter was ignored)
            Views = viewCount;
            Downloads = downloadCount;
            this.CategoryId = CategoryId;
        }
    }
}