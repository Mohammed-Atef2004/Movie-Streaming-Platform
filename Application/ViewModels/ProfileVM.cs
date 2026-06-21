using Core.Models;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels
{
    public class ProfileVM
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<UserMovie> FavoriteMovies { get; set; }
        public IEnumerable<UserMovie> WatchListMovies { get; set; }

        public IEnumerable<UserSeries> FavoriteSeries { get; set; }
        public IEnumerable<UserSeries> WatchListSeries { get; set; }
    }
}
