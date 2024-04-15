using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class LinkBox: BaseEntity.BaseEntity
    {
        public string ImagePath { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
    }
}
