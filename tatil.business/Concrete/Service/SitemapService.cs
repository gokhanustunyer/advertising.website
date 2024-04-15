using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.data.Abstract.Advert;
using tatil.data.Abstract.Category;

namespace tatil.business.Concrete.Service
{
    public class SitemapService : ISitemapService
    {
        private static string PATH = "http://tatilimibuldum.com/";
        private readonly ICategoryRepository _categoryRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        private readonly IAdvertRepository _advertRepository;

        public SitemapService(ICategoryRepository categoryRepository,
                             IHostingEnvironment hostingEnvironment,
                             IConfiguration configuration,
                             IAdvertRepository advertRepository)
        {
            _categoryRepository = categoryRepository;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
            _advertRepository = advertRepository;
        }

        public void CreateSitemap()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            //xml += "<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">";
            xml += "<rss version=\"2.0\">";
            xml += "<channel>";

            var categoryService = _categoryRepository.Table.Select(x => new
            {
                Link = PATH + x.Url,
                Priority = "",
                Title = x.Name.ToLower().Replace('&', '-'),
                Description = x.Name.ToLower().Replace('&', '-')
            });
            var advertService = _advertRepository.Table.Select(x => new
            {
                Link = PATH + x.Url,
                Priority = "",
                Title = x.Title.ToLower().Replace('&', '-'),
                Description = x.ShortDescription.ToLower().Replace('&', '-')
            });

            var designedPages = new List<dynamic>(){
                new { Link=PATH ,  Priority = "1.0", Title = GetTitle("/").Title, Description = GetTitle("/").Description },
                new { Link=Path.Combine(PATH, "hakkimizda") ,  Priority = "", Title = "Hakkımızda | tatilimibuldum.com", Description = "Hakkımızda | tatilimi buldum.com"},
                new { Link=Path.Combine(PATH, "iletisim") ,  Priority = "", Title = "İletişim | tatilimibludum.com", Description = "İletişim | tatilimibuldum.com"},

            };

            var data = designedPages.Union(categoryService).Union(advertService);

            foreach (var item in data)
            {
                if (item.Link == PATH)
                {
                    xml += "<title>" + item.Title + "</title>";
                    xml += "<link>" + item.Link + "</link>";
                    xml += "<description>" + item.Description + "</description>";
                    xml += "<language>tr</language>";
                }
                else
                {
                    xml += "<item>";
                    xml += "<title>" + item.Title + "</title>";
                    xml += "<link>" + item.Link + "</link>";
                    xml += "<description>" + item.Description + "</description>";
                    xml += "</item>";
                }

            }
            xml += "</channel>";
            xml += "</rss>";

            var filePath = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "sitemap.xml");
            File.WriteAllText(filePath, xml);
        }

        private TitleDes GetTitle(string path)
        {
            var result = new TitleDes();
            var info = _categoryRepository.Table.FirstOrDefault(x => x.Url == path);
            if (info != null)
            {
                result.Title = info.Name.ToLower().Replace('&', '-');
                result.Description = info.Name.ToLower().Replace('&', '-');
            }

            return result;
        }

    }

    public class TitleDes
    {
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
