using BLL.Mapping;
using BLL.Services.Abstraction;
using BLL.Services.Implementation;
using DAL.Database;
using DAL.Repositories.Abstraction;
using DAL.Repositories.Implementation;
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

            // Register your services
            builder.Services.AddScoped<IMovieService, MovieService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            var app = builder.Build();

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

            app.UseAuthorization();

            //app.UseStatusCodePagesWithRedirects("/404.html");
            app.UseStatusCodePagesWithReExecute("/Error/{0}");

            app.MapControllerRoute(
                 name: "default",
                 //pattern: "{area=Admin}/{controller=Home}/{action=Index}/{id?}"
                 pattern: "{controller=Home}/{action=Index}/{id?}"
                 );
           

            app.Run();
        }
    }
}
