using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RecommendationSite.Models;
using RecommendationSite.Models.Data;
using RecommendationSite.Models.Repo;

namespace RecommendationSite
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("ReviewsAppConnection");

            builder.Services.AddDbContext<RecommendationDbContext>(opt => opt.UseSqlServer(connectionString));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RecommendationDbContext>();

            builder.Services.AddScoped<IRecommendationRepository<User>, EFUserRepository>();
            builder.Services.AddScoped<IRecommendationRepository<Review>, EFReviewRepository>();
            builder.Services.AddScoped<IRecommendationRepository<Tag>, EFTagRepository>();  

            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("registration",
                    "Registration",
                    new { controller = "Home", action = "Registration" });
                endpoints.MapControllerRoute("logIn",
                    "LogIn",
                    new { controller = "Home", action = "LogIn" });
                endpoints.MapControllerRoute("default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });
                

            SeedData.EnsureData(app);

            app.Run();
        }
    }
}