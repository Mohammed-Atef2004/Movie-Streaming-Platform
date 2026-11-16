using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string? ImageURL { get; set; }
        public virtual ICollection<UserMovie> UserMovies { get; set; }
            = new HashSet<UserMovie>();

        public virtual ICollection<UserSeries> UserSeries { get; set; }
            = new HashSet<UserSeries>();
        public virtual ICollection<Review> Reviews { get; set; }
            = new HashSet<Review>();
    }
}
