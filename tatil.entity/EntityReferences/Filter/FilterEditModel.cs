using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Filter
{
    public class FilterEditModel
    {
        public string Id { get; set; }
        public string FilterBoxTitle { get; set; }
        public List<entity.Filter.Filter> Filters { get; set; }
        public List<entity.Advert.Advert> Adverts { get; set; }
    }
}
