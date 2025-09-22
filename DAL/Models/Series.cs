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
        public int ?ViewCount { get; private set; }
        public int DownloadCount { get; private set; }
        [ForeignKey("Category")]
        public int ?CategoryId { get; set; }
        public Category? Category { get; set; }
        public void Create(string title, string? discription = null, string? imageUrl = null,string ?createor=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            ViewCount = 0;
            DownloadCount = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createor;
        }
        public void Update( string title, string? discription = null, string? imageUrl = null,string ?updater=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
        }
    }
}
