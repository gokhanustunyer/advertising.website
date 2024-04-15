using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class PageStrings: BaseEntity.BaseEntity
    {
        public string? AboutUs { get; set; }
        public string? EmailAddress { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WPNumber { get; set; }
        public string? ContactAddress { get; set; }
        public string? FooterDescription { get; set; }
    }
}
