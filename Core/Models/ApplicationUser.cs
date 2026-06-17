using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string ImageURL { get; set; }
        public ICollection<Movie> WatchedMovies { get; set; } = new HashSet<Movie>();
        public ICollection<Series> WatchedSeries { get; set; } = new HashSet<Series>();
        public ICollection<Movie> FavouriteMovies { get; set; } = new HashSet<Movie>();
        public ICollection<Series> FavouriteSeries { get; set; } = new HashSet<Series>();

    }
}
