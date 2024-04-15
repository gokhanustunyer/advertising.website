using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Statics;
using tatil.data.Contexts;

namespace tatil.data.Concrete.Statics
{
    public class PageStringsRepository : Repository<entity.Statics.PageStrings>, IPageStringsRepository
    {
        public PageStringsRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
