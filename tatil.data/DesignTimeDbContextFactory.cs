using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Contexts;

namespace tatil.data
{
    public class DesginTimeDbContextFactory : IDesignTimeDbContextFactory<TatilDbContext>
    {


        public TatilDbContext CreateDbContext(string[] args)
        {
            var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
            DbContextOptionsBuilder<TatilDbContext> dbContextOptionsBuilder = new();
            dbContextOptionsBuilder.UseMySql("server=localhost;port=3306;user=root;password=;database=tatildb;", serverVersion);
            return new TatilDbContext(dbContextOptionsBuilder.Options);
        }
    }
}
