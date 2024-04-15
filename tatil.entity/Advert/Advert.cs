using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Identity;

namespace tatil.entity.Advert
{
    public class Advert: BaseEntity.BaseEntity
    {
        public AppUser Publisher { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public double Price { get; set; }
        public int Size { get; set; }
        public int HumanCapacity { get; set; }
        public int BedCapacity { get; set; }
        public string? ShortDescription { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Neighbourhood { get; set; }
        public List<Filter.Filter> Filters { get; set; }
        public List<Category.Category> Categories { get; set; }
        public List<AdvertImage> AdvertImages { get; set; }
    }
}
