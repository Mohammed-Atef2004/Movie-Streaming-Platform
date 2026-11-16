using Application.Services.Abstraction;
using Application.Services.Implementation;
using AutoMapper;
using BLL.Mapping;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using Core.Repositories;
using Core.Models;
using Infrastructure.Repositories.Implementation;
using Infrastructure.Presistance.Database;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Movie_Streamer_Platform.Seed;
using System.Security.Claims;

namespace Movie_Streamer_Platform
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(typeof(DomainProfile));

            // Register repositories.
            builder.Services.AddScoped<IMovieRepository, MovieRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
            builder.Services.AddScoped<IEpisodeRepository, EpisodeRepository>();
            builder.Services.AddScoped<IUserMovieRepository, UserMovieRepository>();
            builder.Services.AddScoped<IUserSeriesRepository, UserSeriesRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();

            // Register services.
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeriesService, SeriesService>();
            builder.Services.AddScoped<IEpisodeService, EpisodeService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IUserMovieService, UserMovieService>();
            builder.Services.AddScoped<IUserSeriesService, UserSeriesService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();

            // Identity + cookie authentication configured in ONE place.
            // (The old code registered AddAuthentication(AddCookie) before AddIdentity
            //  and only worked by coincidence.)
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequiredLength = 6;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
            });

            var app = builder.Build();

            // Apply migrations + seed roles/admin/demo data (fully async - the old
            // sync-over-async seeding was deadlock-prone, and the admin credentials
            // were hardcoded in source).
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                var logger = services.GetRequiredService<ILogger<Program>>();

                try
                {
                    await DbSeeder.SeedAsync(context, roleManager, userManager, app.Configuration, logger);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while migrating/seeding the database.");
                    throw;
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapAreaControllerRoute(
                name: "Admin_default",
                areaName: "Admin",
                pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

            app.MapAreaControllerRoute(
                name: "Customer_default",
                areaName: "Customer",
                pattern: "Customer/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "Guest",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            await app.RunAsync();
        }
    }
}
