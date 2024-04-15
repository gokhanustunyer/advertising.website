using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Statics;
using tatil.data.Contexts;

namespace tatil.data.Concrete.Statics
{
    public class SeoRepository : Repository<entity.Statics.SEO>, ISeoRepository
    {
        public SeoRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
