using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Review:CommonData
    {
        private Review() { }
        private Review(string userId, int movieId, string comment)
        {
            this.UserId = userId;
            this.MovieId = movieId;
            this.Comment = comment;
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public string Comment { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
       
        public Review Create(string userId,int movieId,string comment)
        {
            return new Review(userId, movieId, comment);
        }
        public void Update(string comment) {
            this.Comment = comment;
        }
    }
}
