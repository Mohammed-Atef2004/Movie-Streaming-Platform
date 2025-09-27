using BLL.Mapping;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using DAL.Database;
using DAL.Models;
using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Movie_Streamer_Platform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddAutoMapper(typeof(DomainProfile)); // Use this overload to avoid ambiguity
            // Register repositories 
            builder.Services.AddScoped<IMovieRepository,MovieRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ISeriesRepository, SeriesRepository>();
            builder.Services.AddScoped<IBookmarkRepository,BookmarkRepository>();

            // Register your services
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISeriesService, SeriesService>();
            builder.Services.AddScoped<IBookmarkService, BookmarkService>();


            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultTokenProviders();
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //});



            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
           .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
            options =>
            {
                options.LoginPath = new PathString("/Account/Login");
                options.AccessDeniedPath = new PathString("/Account/AccessDenied");
            });
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            //builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            //.AddEntityFrameworkStores<ApplicationDbContext>()
            //.AddDefaultTokenProviders();
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.LoginPath = "/Account/Login";
            //    options.AccessDeniedPath = "/Account/AccessDenied";
            //});



            var app = builder.Build();
            // Seeding Roles & Admin User
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                string[] roles = { "Admin", "User" };

                foreach (var role in roles)
                {
                    if (!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                    {
                        roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
                    }
                }

                string adminEmail = "admin@gmail.com";
                string adminPassword = "Admin@123";

                var adminUser = userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();
                if (adminUser == null)
                {
                    adminUser = new ApplicationUser
                    {
                        UserName = adminEmail,
                        Email = adminEmail,
                        EmailConfirmed = true
                    };

                    var result = userManager.CreateAsync(adminUser, adminPassword).GetAwaiter().GetResult();
                    if (result.Succeeded)
                    {
                        userManager.AddToRoleAsync(adminUser, "Admin").GetAwaiter().GetResult();
                    }
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin_default",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");
                endpoints.MapAreaControllerRoute(
                   name: "Identity",
                   areaName: "Admin",
                   pattern: "Identity/{controller=Home}/{action=Index}/{id?}");

                // Route الأساسي
                endpoints.MapAreaControllerRoute(
                    name: "default",
                    areaName: "Customer",
                    pattern: "Customer/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "Guest",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

            });




            app.Run();
        }
    }
}
