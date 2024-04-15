using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Category;
using tatil.data.Concrete;
using tatil.data.Contexts;
using t = tatil.entity;
using d = tatil.data;

namespace tatil.data.Concrete.Category
{
    public class CategoryRepository : Repository<t.Category.Category>, ICategoryRepository
    {
        public CategoryRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
