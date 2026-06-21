using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class UserSeries
    {
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int SeriesId { get; set; }
        public Series Series { get; set; }

        public bool IsFavorite { get; set; }
        public bool IsInWatchList { get; set; }

        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    }
}
