using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Bookmark : CommonData
    {
        public int Id { get; private set; }
       
        [ForeignKey("User")]
        public string UserId { get; private set; }
        public User? User { get; private set; }
        [ForeignKey("Movie")]
        public int MovieId { get; private set; }
        public Movie? Movie { get; private set; }
        public static Bookmark Create(string userId, int movieId, string? creator = null)
        {
            return new Bookmark
            {
                UserId = userId,
                MovieId = movieId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = creator
            };

        }
        public void Update(int movieId, string? updateby = null)
        {
            MovieId = movieId;            
            UpdatedAt = DateTime.UtcNow;  
            UpdatedBy = updateby;        
        }
    }


}
