using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class MainPageContent: BaseEntity.BaseEntity
    {
        public string? BackgroundImagePath { get; set; }
        public string? ImagePath { get; set; }
        public string? TextContent { get; set; }
        public bool? IsLinkBoxContent { get; set; }
        public bool? IsImageContent { get; set; }
        public bool? IsFullTextContent { get; set; }
        public bool? BackgorundFilterIsWhite { get; set; }
        public bool? ButtonIsOutline { get; set; }
        public bool? ButtonIsDark { get; set; }
        public bool? LinkBoxesIsBlack { get; set; }
        public string? ContentTitle { get; set; }
        public string? ButtonText { get; set; }
        public string? ButtonLink { get; set; }
        public int Index { get; set; }
        public List<LinkBox>? LinkBoxes { get; set; }
        public MainPage MainPage { get; set; }
    }
}
