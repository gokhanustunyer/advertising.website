using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Statics;
using tatil.entity.Identity;

namespace tatil.data.Concrete
{
    public static class SeedDatabase
    {
        public async static Task Seed(IApplicationBuilder app)
        {
            var scope = app.ApplicationServices.CreateScope();
            var rolesManager = scope.ServiceProvider.GetService<RoleManager<AppRole>>();
            var userManager = scope.ServiceProvider.GetService<UserManager<AppUser>>();
            var pageStringsService = scope.ServiceProvider.GetService<IPageStringsRepository>();
            var mainPageService = scope.ServiceProvider.GetService<IMainPageRepository>();
            if (rolesManager.Roles.Count() < 1)
            {
                await rolesManager.CreateAsync(new() { Name = "Admin" });
                await rolesManager.CreateAsync(new() { Name = "User" });
            }
            if (userManager.Users.Count() < 1)
            {
                AppUser user = new AppUser()
                {
                    UserName = "admin",
                    Email = "admin@hotmail.com",
                    FirstName = "admin",
                    LastName = "admin",
                    Id = Guid.NewGuid().ToString(),
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    PhoneNumber = "5555555555",
                    isWaitingForAdverts = false,
                    isConfirmedForAdverts = true,
                    UpdateDate = DateTime.Now,
                    CreateDate = DateTime.Now,
                };
                IdentityResult result = await userManager.CreateAsync(user, "Gokhan949..");
                await userManager.AddToRoleAsync(user, "Admin");
                await userManager.UpdateAsync(user);
            }
            if (pageStringsService.Table.ToList().Count() < 1)
            {
                pageStringsService.Table.Add(new entity.Statics.PageStrings()
                {
                    AboutUs = "tatilimibuldum.com",
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FacebookLink = "facebook.com",
                    Id = Guid.NewGuid(),
                    InstagramLink = "instagram.com",
                    PhoneNumber = "+905555555",
                    WPNumber = "+905555555",
                    EmailAddress = "c",
                    TwitterLink = "twitter.com",
                    ContactAddress = "Adres",
                    FooterDescription = "alt açıklama"
                });
                pageStringsService.Save();
            }
            if (mainPageService.Table.ToList().Count < 1)
            {
                mainPageService.Table.Add(new()
                {
                    Contents = new(),
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    headerImagePath = "",
                    headerText = "",
                    welcomeText = "",
                    welcomeTitle = "tatilburada.com'a hoşgeldiniz",
                    welcomeTextImagePath1 = "",
                    welcomeTextImagePath2 = "",
                    CityCount = 81,
                    CustomerCount = 500,
                    StreamerCount = 50,
                    NumericDatasImagePath = "",
                });
                mainPageService.Save();
            }
        }
    }
}
