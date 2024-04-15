using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.data.Abstract.Advert;
using tatil.data.Contexts;
using t = tatil.entity;

namespace tatil.data.Concrete.Advert
{
    public class AdvertRepository : Repository<t.Advert.Advert>, IAdvertRepository
    {
        public AdvertRepository(TatilDbContext context) : base(context)
        {
        }
    }
}
