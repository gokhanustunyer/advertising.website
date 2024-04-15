using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Advert;
using tatil.data.Abstract.Category;
using tatil.data.Abstract.Filter;
using tatil.data.Abstract.Statics;
using tatil.data.Concrete.Advert;
using tatil.data.Concrete.Category;
using tatil.data.Concrete.Filter;
using tatil.data.Concrete.Statics;
using tatil.data.Contexts;
using tatil.data.Services.Abstract;
using tatil.data.Services.Concrete;

namespace tatil.data
{
    public static class ServiceRegistration
    {
        public static void AddDataServices(this IServiceCollection services, IConfiguration configuration)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            services.AddDbContext<TatilDbContext>(options => options.UseMySql("server=localhost;port=3306;user=root;password=;database=tatildb;", serverVersion));
            services.AddScoped<IAdvertRepository, AdvertRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFilterRepository, FilterRepository>();
            services.AddScoped<IFilterBoxRepository, FilterBoxRepository>();
            services.AddScoped<ICitiesDbService, CitiesDbService>();
            services.AddScoped<ISeoRepository, SeoRepository>();
            services.AddScoped<IPageStringsRepository, PageStringsRepository>();
            services.AddScoped<IMainPageRepository, MainPageRepository>();
        }
    }
}
