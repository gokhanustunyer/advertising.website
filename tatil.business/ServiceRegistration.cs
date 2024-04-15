using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.business.Concrete.Service;
using tatil.business.Concrete.Storage.Local;

namespace tatil.business
{
    public static class ServiceRegistration
    {
        public static void AddBusinessServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IAdvertService, AdvertService>();
            serviceCollection.AddScoped<ICategoryService, CategoryService>();
            serviceCollection.AddScoped<IFilterService, FilterService>();
            serviceCollection.AddScoped<ILocalStorage, LocalStorage>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<ICityDbService, CityDbService>();
            serviceCollection.AddScoped<IEmailService, EmailService>();
            serviceCollection.AddScoped<ISeoService, SeoService>();
            serviceCollection.AddScoped<IPageStringsService, PageStringService>();
            serviceCollection.AddScoped<IFormService, FormService>();
            serviceCollection.AddScoped<ISitemapService, SitemapService>();
            serviceCollection.AddScoped<IMainPageService, MainPageService>();
        }
    }
}
