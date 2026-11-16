using Core.Models;
using Infrastructure.Presistance.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Movie_Streamer_Platform.Seed
{
    /// <summary>
    /// Applies pending migrations and seeds roles, the initial admin (credentials
    /// come from configuration - never from source code) and a small demo catalogue
    /// so a fresh clone is immediately usable.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(
            ApplicationDbContext context,
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ILogger logger)
        {
            // 1) Apply pending migrations automatically (safe no-op when up to date).
            await context.Database.MigrateAsync();

            // 2) Roles.
            string[] roles = { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            // 3) Admin user - only when credentials are configured.
            var adminEmail = configuration["Seed:AdminEmail"];
            var adminPassword = configuration["Seed:AdminPassword"];

            if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
            {
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, adminPassword);
                    if (result.Succeeded)
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    else
                        logger.LogWarning("Failed to seed the admin user: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            // 4) Demo catalogue (only for an empty database).
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    CreateCategory("Action"), CreateCategory("Drama"),
                    CreateCategory("Comedy"), CreateCategory("Sci-Fi"),
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();

                var sampleMovies = new[]
                {
                    (Title: "Big Buck Bunny", CategoryId: categories[0].Id, IsFree: true, Video: SampleVideo("BigBuckBunny"), Trailer: "https://www.youtube.com/embed/aqz-KE-bpKQ", Year: 2008, Rating: 8.0),
                    (Title: "Elephants Dream", CategoryId: categories[1].Id, IsFree: true, Video: SampleVideo("ElephantsDream"), Trailer: "https://www.youtube.com/embed/Tl6qmpvEMK0", Year: 2006, Rating: 7.0),
                    (Title: "Sintel", CategoryId: categories[3].Id, IsFree: false, Video: SampleVideo("Sintel"), Trailer: "https://www.youtube.com/embed/eRsGyueVLvQ", Year: 2010, Rating: 9.0),
                    (Title: "Tears of Steel", CategoryId: categories[3].Id, IsFree: false, Video: SampleVideo("TearsOfSteel"), Trailer: "https://www.youtube.com/embed/R6MlUcmOul8", Year: 2012, Rating: 8.5),
                    (Title: "For Bigger Blazes", CategoryId: categories[2].Id, IsFree: true, Video: SampleVideo("ForBiggerBlazes"), Trailer: null, Year: 2015, Rating: 6.5),
                    (Title: "For Bigger Escapes", CategoryId: categories[1].Id, IsFree: true, Video: SampleVideo("ForBiggerEscapes"), Trailer: null, Year: 2015, Rating: 6.0),
                };

                foreach (var m in sampleMovies)
                {
                    var movie = new Movie();
                    movie.Create(m.Title, $"Demo movie: {m.Title}. A Blender Open Movie used as sample content for the platform.", null, m.IsFree, m.CategoryId, m.Trailer, m.Video, m.Rating, m.Year, creator: "system");
                    context.Movies.Add(movie);
                }

                if (!await context.Series.AnyAsync())
                {
                    var series = new Series();
                    series.Create("Blender Studio Series", 0, 0, categories[3].Id, "A demo series assembled from the Blender Open Movies.", isFree: true, imageUrl: null, creator: "system");
                    context.Series.Add(series);
                    await context.SaveChangesAsync();

                    var episodes = new[]
                    {
                        (Title: "Episode 1 - The Beginning", Video: SampleVideo("ForBiggerJoyrides")),
                        (Title: "Episode 2 - The Journey", Video: SampleVideo("ForBiggerFun")),
                        (Title: "Episode 3 - The Finale", Video: SampleVideo("ForBiggerMeltdowns")),
                    };

                    foreach (var e in episodes)
                    {
                        var episode = new Episode();
                        episode.Create(e.Title, series.Id, description: $"Demo episode of {series.Title}.", videoUrl: e.Video, creator: "system");
                        context.Episodes.Add(episode);
                    }
                }

                await context.SaveChangesAsync();
            }
        }

        private static Category CreateCategory(string name)
        {
            var category = new Category();
            category.Create(name, $"Sample {name} category.", creator: "system");
            return category;
        }

        private static string SampleVideo(string name) =>
            $"https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/{name}.mp4";
    }
}
