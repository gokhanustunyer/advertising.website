using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Advert
{
    public class AdvertListingModel
    {
        public List<entity.Advert.Advert> Adverts { get; set; }
        public List<entity.Filter.FilterBox> Filters { get; set; }
        public List<Category.Category> Categories  { get; set; }
        public List<string> FilterIds { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neigh { get; set; }
        public int BedCount { get; set; }
        public int HumanCount { get; set; }
        public string SearchValue { get; set; }
        public int TotalItemCount { get; set; }
        public int TotalPageCount { get; set; }
        public int ActivePage { get; set; }
    }
}
