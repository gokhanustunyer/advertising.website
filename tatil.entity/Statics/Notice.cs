using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class Notice: BaseEntity.BaseEntity
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime DeleteTime { get; set; }
    }
}
