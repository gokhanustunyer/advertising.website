using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Filter
{
    public class Filter: BaseEntity.BaseEntity
    {
        public List<Advert.Advert>? Adverts { get; set; }
        public string FilterTitle { get; set; }
        public FilterBox FilterBox { get; set; }
    }
}
