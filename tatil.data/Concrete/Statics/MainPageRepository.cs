using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Statics;
using tatil.data.Contexts;

namespace tatil.data.Concrete.Statics
{
    internal class MainPageRepository : Repository<entity.Statics.MainPage>, IMainPageRepository
    {
        public MainPageRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
