using Core.Models;
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
        public virtual ICollection<UserMovie> UserMovies { get; set; }
            = new HashSet<UserMovie>();

        public virtual ICollection<UserSeries> UserSeries { get; set; }
            = new HashSet<UserSeries>();
        public virtual ICollection<Review> Reviews { get; set; }

    }
}
