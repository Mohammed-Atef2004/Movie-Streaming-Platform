    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

namespace DAL.Models
{
    public class Movie: CommonData
    {
        public int Id { get; private set; }
        public string? Title { get; private set; }
        public string? Description { get; private set; }
        public string? ImageUrl { get;  set; }
        public int? Views { get; private set; } = 0;
        public int? Downloads { get; private set; } = 0;
        public bool? IsFree { get; set; } = true;
        [ForeignKey("Category")]
        
        public int? CategoryId { get; set; }
        public Category? Category { get; set; }


      
        public void Create(string title, string? discription = null, bool? isFree = true, string? imageUrl = null,string ?createor=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            Views = 0;
            Downloads = 0;
            CreatedAt = DateTime.UtcNow;
            CreatedBy = createor;
            IsFree = isFree;
        }
        public void Update( string title, string? discription,int? views,int? dowloads,int? categoryId, bool? isFree = true, string? imageUrl = null,string ?updater=null)
        {
            Title = title;
            Description = discription;
            ImageUrl = imageUrl;
            UpdatedAt = DateTime.UtcNow;
            UpdatedBy = updater;
            IsFree = IsFree;
            CategoryId = categoryId;
            Views = views == null ? 1 : views;
            Downloads = dowloads == null ? 1 : dowloads;
        }
      

     
    }
}
