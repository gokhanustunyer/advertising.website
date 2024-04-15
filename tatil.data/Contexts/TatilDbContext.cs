using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Advert;
using tatil.entity.Category;
using tatil.entity.Filter;
using tatil.entity.Identity;
using tatil.entity.PageEntities;
using tatil.entity.Statics;
using t = tatil.entity;

namespace tatil.data.Contexts
{
    public class TatilDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Filter> Filters { get; set; }
        public DbSet<FilterBox> FilterBoxes { get; set; }
        public DbSet<t.File> Files { get; set; }
        public DbSet<t.Log.PageLog> PageLogs { get; set; }
        public DbSet<PageStrings> PageStrings  { get; set; }
        public DbSet<SEO> SEOs  { get; set; }
        public DbSet<Notice> Notices  { get; set; }
        public DbSet<SupportFormModel> SupportMails { get; set; }
        public DbSet<MainPage> MainPages { get; set; }
        public DbSet<MainPageContent> MainPageContent { get; set; }
        public DbSet<Headers> Headers { get; set; }
        public DbSet<SupportMailResponseModel> SupportMailResponses { get; set; }
        public TatilDbContext(DbContextOptions options)
            : base(options) { }

    }
}
