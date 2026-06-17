using DAL.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.ViewModels
{
    public class UserVM
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
        public bool IsBanned { get; set; }
        public string ImageURL { get; set; }
        public IFormFile Image { get; set; }
        public ICollection<Movie> WatchedMovies { get; set; } = new HashSet<Movie>();
        public ICollection<Series> WatchedSeries { get; set; } = new HashSet<Series>();
        public ICollection<Movie> FavouriteMovies { get; set; } = new HashSet<Movie>();
        public ICollection<Series> FavouriteSeries { get; set; } = new HashSet<Series>();
    }
}
