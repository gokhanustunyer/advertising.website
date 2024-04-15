using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Statics
{
    public class SaveMainPageSettingsModel
    {
        public IFormFile HeaderImage { get; set; }
        public string HeaderText { get; set; }
        public IFormFile WelcomeImage1 { get; set; }
        public IFormFile WelcomeImage2 { get; set; }
        public string WelcomeText { get; set; }
        public string? WelcomeTitle { get; set; }
        public IFormFile NumericsImage { get; set; }
        public int UserCount { get; set; }
        public int CityCount { get; set; }
        public int PublisherCount { get; set; }
    }
}
