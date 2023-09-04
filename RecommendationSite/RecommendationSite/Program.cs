using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.MicrosoftAccount;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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

            builder.Services.AddScoped<IRecommendationRepository<User>, EFUserRepository>();
            builder.Services.AddScoped<IRecommendationRepository<Review>, EFReviewRepository>();
            builder.Services.AddScoped<IRecommendationRepository<Tag>, EFTagRepository>();

            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = MicrosoftAccountDefaults.AuthenticationScheme;
                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/Home/LogIn";
                    options.LogoutPath = "/Home/LogOut";
                    options.AccessDeniedPath = "/Home/Error";
                    options.ReturnUrlParameter = "ReturnUrl";
                })
                .AddFacebook(facebookOpt =>
                {
                    facebookOpt.AppId = builder.Configuration["Authentication:Facebook:AppId"];
                    facebookOpt.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"];
                })
                .AddGoogle(googleOpt =>
                {
                    googleOpt.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                    googleOpt.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                })
                .AddMicrosoftAccount(microsoftOpt =>
                {
                    microsoftOpt.ClientId = builder.Configuration["Authentication:Microsoft:ClientId"];
                    microsoftOpt.ClientSecret = builder.Configuration["Authentication:Microsoft:ClientSecret"];
                });

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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            SeedData.EnsureData(app);

            app.Run();
        }
    }
}