using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Advert.Create
{
    public class AdvertCreateModel
    {
        public List<entity.Filter.FilterBox> FilterBoxes { get; set; }
        public List<Category.Category> Categories { get; set; }
    }
}
