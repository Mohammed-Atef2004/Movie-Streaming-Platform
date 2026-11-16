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
        private static readonly IReadOnlyDictionary<string, string> DemoMovieArtwork =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Big Buck Bunny"] = "https://peach.blender.org/wp-content/uploads/title_anouncement.jpg?x11217",
                ["Elephants Dream"] = "https://orange.blender.org/wp-content/themes/orange/images/common/ed_header.jpg",
                ["Sintel"] = "https://img.youtube.com/vi/eRsGyueVLvQ/hqdefault.jpg",
                ["Tears of Steel"] = "https://mango.blender.org/wp-content/uploads/2013/05/01_thom_celia_bridge.jpg",
                ["For Bigger Blazes"] = "https://images.unsplash.com/photo-1536440136628-849c177e76a1?auto=format&fit=crop&w=900&q=85",
                ["For Bigger Escapes"] = "https://images.unsplash.com/photo-1440404653325-ab127d49abc1?auto=format&fit=crop&w=900&q=85"
            };

        private const string DemoSeriesArtwork =
            "https://images.unsplash.com/photo-1489599849927-2ee91cede3ba?auto=format&fit=crop&w=1200&q=85";

        private static readonly DemoMovie[] AdditionalDemoMovies =
        {
            new("Midnight Lights", "Drama", "A quiet city becomes the setting for an unexpected reunion.", true, 2022, 7.4,
                "ForBiggerJoyrides", "https://images.unsplash.com/photo-1519608487953-e999c86e7455?auto=format&fit=crop&w=900&q=85"),
            new("The Last Journey", "Action", "A long road home turns into a test of courage.", false, 2023, 8.1,
                "SubaruOutbackOnStreetAndDirt", "https://images.unsplash.com/photo-1500530855697-b586d89ba3ee?auto=format&fit=crop&w=900&q=85"),
            new("Beyond the Blue", "Sci-Fi", "An explorer follows a signal beyond the edge of the known world.", false, 2024, 8.3,
                "WeAreGoingOnBullrun", "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&w=900&q=85"),
            new("A Little More Sunshine", "Comedy", "A small-town detour makes room for a fresh start.", true, 2021, 7.0,
                "ForBiggerFun", "https://images.unsplash.com/photo-1470252649378-9c29740c9fa8?auto=format&fit=crop&w=900&q=85")
        };

        private static readonly DemoSeries[] DemoSeriesCatalog =
        {
            new(
                "Blender Studio Series",
                "Sci-Fi",
                "A demo series assembled from independent animated stories.",
                DemoSeriesArtwork,
                true,
                new[]
                {
                    new DemoEpisode("Episode 1 - The Beginning", 1, 1, "ForBiggerJoyrides"),
                    new DemoEpisode("Episode 2 - The Journey", 1, 2, "ForBiggerFun"),
                    new DemoEpisode("Episode 3 - The Finale", 1, 3, "ForBiggerMeltdowns"),
                    new DemoEpisode("A New Signal", 2, 1, "WeAreGoingOnBullrun"),
                    new DemoEpisode("Into the Unknown", 2, 2, "SubaruOutbackOnStreetAndDirt"),
                    new DemoEpisode("The Next Chapter", 2, 3, "WhatCarCanYouGetForAGrand")
                }),
            new(
                "Stories from the Studio",
                "Drama",
                "Character-driven short stories from the Streamify demo collection.",
                "https://images.unsplash.com/photo-1519608487953-e999c86e7455?auto=format&fit=crop&w=1200&q=85",
                true,
                new[]
                {
                    new DemoEpisode("First Light", 1, 1, "ForBiggerEscapes"),
                    new DemoEpisode("The Long Way Around", 1, 2, "WeAreGoingOnBullrun"),
                    new DemoEpisode("Home Again", 1, 3, "SubaruOutbackOnStreetAndDirt")
                }),
            new(
                "Beyond the Frame",
                "Action",
                "A collection of quick adventures, one episode at a time.",
                "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&w=1200&q=85",
                false,
                new[]
                {
                    new DemoEpisode("The First Clue", 1, 1, "ForBiggerBlazes"),
                    new DemoEpisode("Crossing Over", 1, 2, "VolkswagenGTIReview"),
                    new DemoEpisode("Open Roads", 1, 3, "ForBiggerJoyrides")
                })
        };

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
                    movie.Create(m.Title, $"Demo movie: {m.Title}. A Blender Open Movie used as sample content for the platform.", DemoMovieArtwork[m.Title], m.IsFree, m.CategoryId, m.Trailer, m.Video, m.Rating, m.Year, creator: "system");
                    context.Movies.Add(movie);
                }

                if (!await context.Series.AnyAsync())
                {
                    var series = new Series();
                    series.Create("Blender Studio Series", 0, 0, categories[3].Id, "A demo series assembled from the Blender Open Movies.", isFree: true, imageUrl: DemoSeriesArtwork, creator: "system");
                    context.Series.Add(series);
                    await context.SaveChangesAsync();

                    var episodes = new[]
                    {
                        (Title: "Episode 1 - The Beginning", SeasonNumber: 1, EpisodeNumber: 1, Video: SampleVideo("ForBiggerJoyrides")),
                        (Title: "Episode 2 - The Journey", SeasonNumber: 1, EpisodeNumber: 2, Video: SampleVideo("ForBiggerFun")),
                        (Title: "Episode 3 - The Finale", SeasonNumber: 1, EpisodeNumber: 3, Video: SampleVideo("ForBiggerMeltdowns")),
                    };

                    foreach (var e in episodes)
                    {
                        var episode = new Episode();
                        episode.Create(
                            e.Title,
                            series.Id,
                            description: $"Demo episode of {series.Title}.",
                            videoUrl: e.Video,
                            creator: "system",
                            seasonNumber: e.SeasonNumber,
                            episodeNumber: e.EpisodeNumber);
                        context.Episodes.Add(episode);
                    }
                }

                await context.SaveChangesAsync();
            }

            await SeedAdditionalDemoCatalogAsync(context);
            await SeedMissingArtworkAsync(context);
        }

        private static async Task SeedAdditionalDemoCatalogAsync(ApplicationDbContext context)
        {
            var categoryNames = AdditionalDemoMovies.Select(movie => movie.Category)
                .Concat(DemoSeriesCatalog.Select(series => series.Category))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
            var categories = await context.Categories
                .Where(category => category.IsDeleted != true && categoryNames.Contains(category.Name))
                .ToDictionaryAsync(category => category.Name, StringComparer.OrdinalIgnoreCase);

            foreach (var categoryName in categoryNames)
            {
                if (!categories.ContainsKey(categoryName))
                {
                    var category = CreateCategory(categoryName);
                    context.Categories.Add(category);
                    categories[categoryName] = category;
                }
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();

            foreach (var item in AdditionalDemoMovies)
            {
                if (await context.Movies.AnyAsync(movie => movie.Title == item.Title))
                    continue;

                var movie = new Movie();
                movie.Create(
                    item.Title,
                    item.Description,
                    item.ImageUrl,
                    item.IsFree,
                    categories[item.Category].Id,
                    trailerUrl: null,
                    movieUrl: SampleVideo(item.Video),
                    rating: item.Rating,
                    year: item.Year,
                    creator: "system");
                context.Movies.Add(movie);
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();

            foreach (var item in DemoSeriesCatalog)
            {
                var series = await context.Series.FirstOrDefaultAsync(existing => existing.Title == item.Title);
                if (series == null)
                {
                    series = new Series();
                    series.Create(
                        item.Title,
                        0,
                        0,
                        categories[item.Category].Id,
                        item.Description,
                        item.IsFree,
                        item.ImageUrl,
                        creator: "system");
                    context.Series.Add(series);
                    await context.SaveChangesAsync();
                }
                else if (string.IsNullOrWhiteSpace(series.ImageUrl))
                {
                    series.ImageUrl = item.ImageUrl;
                }

                foreach (var episodeSpec in item.Episodes)
                {
                    var episode = await context.Episodes.FirstOrDefaultAsync(existing =>
                        existing.SeriesId == series.Id && existing.Title == episodeSpec.Title);

                    if (episode != null)
                    {
                        if (episode.SeasonNumber == 0 || episode.EpisodeNumber == 0)
                        {
                            episode.SeasonNumber = episodeSpec.SeasonNumber;
                            episode.EpisodeNumber = episodeSpec.EpisodeNumber;
                        }
                        continue;
                    }

                    episode = new Episode();
                    episode.Create(
                        episodeSpec.Title,
                        series.Id,
                        description: $"Episode {episodeSpec.EpisodeNumber} of season {episodeSpec.SeasonNumber}.",
                        videoUrl: SampleVideo(episodeSpec.Video),
                        creator: "system",
                        seasonNumber: episodeSpec.SeasonNumber,
                        episodeNumber: episodeSpec.EpisodeNumber);
                    context.Episodes.Add(episode);
                }
            }

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }

        private static async Task SeedMissingArtworkAsync(ApplicationDbContext context)
        {
            var demoTitles = DemoMovieArtwork.Keys.ToArray();
            var movies = await context.Movies
                .Where(movie => (movie.ImageUrl == null || movie.ImageUrl == "") &&
                    movie.Title != null && demoTitles.Contains(movie.Title))
                .ToListAsync();

            foreach (var movie in movies)
                movie.ImageUrl = DemoMovieArtwork[movie.Title!];

            var series = await context.Series.FirstOrDefaultAsync(item =>
                item.Title == "Blender Studio Series" &&
                (item.ImageUrl == null || item.ImageUrl == ""));
            if (series != null)
                series.ImageUrl = DemoSeriesArtwork;

            if (context.ChangeTracker.HasChanges())
                await context.SaveChangesAsync();
        }

        private static Category CreateCategory(string name)
        {
            var category = new Category();
            category.Create(name, $"Sample {name} category.", creator: "system");
            return category;
        }

        private static string SampleVideo(string name) =>
            $"https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/{name}.mp4";

        private sealed record DemoMovie(
            string Title,
            string Category,
            string Description,
            bool IsFree,
            int Year,
            double Rating,
            string Video,
            string ImageUrl);

        private sealed record DemoSeries(
            string Title,
            string Category,
            string Description,
            string ImageUrl,
            bool IsFree,
            DemoEpisode[] Episodes);

        private sealed record DemoEpisode(string Title, int SeasonNumber, int EpisodeNumber, string Video);
    }
}
