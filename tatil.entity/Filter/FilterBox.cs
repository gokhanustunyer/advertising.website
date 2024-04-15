using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Filter
{
    public class FilterBox: BaseEntity.BaseEntity
    {
        public string FilterBoxTitle { get; set; }
        public List<Filter> Filters { get; set; }
    }
}
