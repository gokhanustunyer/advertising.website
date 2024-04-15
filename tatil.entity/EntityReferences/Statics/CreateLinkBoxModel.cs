using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.EntityReferences.Statics
{
    public class CreateLinkBoxModel
    {
        public string? Id { get; set; }
        public string? RemoveBackground { get; set; }
        public IFormFile Image1 { get; set; }
        public IFormFile Image2 { get; set; }
        public IFormFile Image3 { get; set; }
        public IFormFile Image4 { get; set; }
        public string MainTitle { get; set; }
        public string Title1 { get; set; }
        public string Title2 { get; set; }
        public string Title3 { get; set; }
        public string Title4 { get; set; }
        public string Link1 { get; set; }
        public string Link2 { get; set; }
        public string Link3 { get; set; }
        public string Link4 { get; set; }
        public IFormFile BackgroundImage { get; set; }
        public string BackgroundFilter { get; set; }
        public string LinkCartFilter { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
    }
}
