using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using t = tatil.entity;

namespace tatil.data.Abstract.Advert
{
    public interface IAdvertRepository: IRepository<t.Advert.Advert>
    {
    }
}
