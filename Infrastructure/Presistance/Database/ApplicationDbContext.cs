using Core.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Presistance.Database
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Series> Series { get; set; }
        public DbSet<Episode> Episodes { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<UserMovie> UserMovies { get; set; }
        public DbSet<UserSeries> UserSeries { get; set; }
        public DbSet<Review> Reviews { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserMovie>()
                .HasKey(x => new { x.UserId, x.MovieId });

            builder.Entity<UserSeries>()
                .HasKey(x => new { x.UserId, x.SeriesId });

            builder.Entity<Review>()
                .HasKey(x => new { x.UserId, x.MovieId });

            // Real soft delete: soft-deleted rows stay in the database but are
            // hidden from every query through global query filters.
            builder.Entity<Movie>().HasQueryFilter(m => m.IsDeleted == false);
            builder.Entity<Series>().HasQueryFilter(s => s.IsDeleted == false);
            builder.Entity<Category>().HasQueryFilter(c => c.IsDeleted == false);
            builder.Entity<Episode>().HasQueryFilter(e => e.IsDeleted == false);
            builder.Entity<Review>().HasQueryFilter(r => r.IsDeleted == false);

            // The join rows (favorites/watchlist) must follow their principal:
            // once a movie/series is soft-deleted its favorites and watchlist
            // entries disappear from every query as well. This also removes the
            // "required end of a filtered relationship" model warnings.
            builder.Entity<UserMovie>().HasQueryFilter(um => um.Movie!.IsDeleted == false);
            builder.Entity<UserSeries>().HasQueryFilter(us => us.Series!.IsDeleted == false);

            // Required navigations pointing at filtered entities must be optional
            // (nullable), otherwise EF Core refuses to build the model. The
            // relationships stay required through their non-nullable FK columns,
            // and keep the default Cascade behaviour defined by the migrations.
            builder.Entity<Movie>().HasOne(m => m.Category).WithMany(c => c.Movies).HasForeignKey(m => m.CategoryId);
            builder.Entity<Series>().HasOne(s => s.Category).WithMany().HasForeignKey(s => s.CategoryId);
            builder.Entity<Episode>().HasOne(e => e.Series).WithMany(s => s.Episodes).HasForeignKey(e => e.SeriesId);
            builder.Entity<Review>().HasOne(r => r.Movie).WithMany(m => m.UserReviews).HasForeignKey(r => r.MovieId);
            builder.Entity<Review>().HasOne(r => r.User).WithMany(u => u.Reviews).HasForeignKey(r => r.UserId);
            builder.Entity<UserMovie>().HasOne(um => um.Movie).WithMany(m => m.UserMovies).HasForeignKey(um => um.MovieId);
            builder.Entity<UserMovie>().HasOne(um => um.User).WithMany(u => u.UserMovies).HasForeignKey(um => um.UserId);
            builder.Entity<UserSeries>().HasOne(us => us.Series).WithMany(s => s.UserSeries).HasForeignKey(us => us.SeriesId);
            builder.Entity<UserSeries>().HasOne(us => us.User).WithMany(u => u.UserSeries).HasForeignKey(us => us.UserId);
        }
    }
}
