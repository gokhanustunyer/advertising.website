using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class Headers: BaseEntity.BaseEntity
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public string BackgroundImagePath { get; set; }
        public string? Size  { get; set; }
        public string? Url  { get; set; }
    }
}
