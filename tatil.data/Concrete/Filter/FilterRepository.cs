using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Category;
using tatil.data.Contexts;
using t = tatil.entity;
using d = tatil.data;

namespace tatil.data.Concrete.Filter
{
    public class FilterRepository : Repository<t.Filter.Filter>, IFilterRepository
    {
        public FilterRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
