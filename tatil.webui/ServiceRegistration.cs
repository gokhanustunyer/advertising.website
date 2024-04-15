using Microsoft.AspNetCore.Identity;
using tatil.data.Contexts;
using tatil.entity.Identity;

namespace tatil.webui
{
    public static class ServiceRegistration
    {
        public static void AddCookieSettings(this IServiceCollection services)
        {
            _ = services.AddIdentity<AppUser, AppRole>
                (options =>
                {
                    options.Password.RequiredLength = 8;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = true;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.User.RequireUniqueEmail = true;
                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<TatilDbContext>().AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/giris";
                options.LogoutPath = "/cikis";
                options.AccessDeniedPath = "/accessdenied";
                options.ExpireTimeSpan = TimeSpan.FromDays(365);
                options.SlidingExpiration = false;
                options.Cookie = new CookieBuilder
                {
                    HttpOnly = true,
                    Name = ".tatil.Security.Cookie",
                    SameSite = SameSiteMode.Strict,
                };
            });
        }
    }
}
