using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t = tatil.entity;

namespace tatil.data.Abstract.Category
{
    public interface ICategoryRepository : IRepository<t.Category.Category>
    {
    }
}
