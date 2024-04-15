using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Statics
{
    public class MainPage: BaseEntity.BaseEntity
    {
        public string? headerImagePath { get; set; }
        public string? headerText { get; set; }
        public string? welcomeTitle { get; set; }
        public string? welcomeTextImagePath1 { get; set; }
        public string? welcomeTextImagePath2 { get; set; }
        public string? welcomeText { get; set; }
        public int? CustomerCount { get; set; }
        public int? CityCount { get; set; }
        public int? StreamerCount { get; set; }
        public string? NumericDatasImagePath { get; set; }
        public List<MainPageContent>? Contents { get; set; }
    }
}
