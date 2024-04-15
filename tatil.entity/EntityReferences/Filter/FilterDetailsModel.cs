using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Filter
{
    public class FilterDetailsModel
    {
        public string FilterId { get; set; }
        public string FilterTitle { get; set; }
        public List<entity.Advert.Advert>? Adverts { get; set; }
    }
}
