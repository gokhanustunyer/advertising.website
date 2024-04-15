using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Category
{
    public class Category: BaseEntity.BaseEntity
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Advert.Advert>? Adverts { get; set; }
    }
}
