using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserMovie
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsInWatchList { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
