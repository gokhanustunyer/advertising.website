using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Filter;
using tatil.data.Contexts;
using t = tatil.entity;

namespace tatil.data.Concrete.Filter
{
    public class FilterBoxRepository : Repository<t.Filter.FilterBox>, IFilterBoxRepository
    {
        public FilterBoxRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
