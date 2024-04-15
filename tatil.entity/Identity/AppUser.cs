using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.entity.Identity
{
    public class AppUser: IdentityUser<string>
    {
        public string FirstName{ get; set; }
        public string LastName { get; set; }
        public bool isConfirmedForAdverts { get; set; }
        public bool isWaitingForAdverts { get; set; }
        public string? AdvertRequestMessage { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public List<Advert.Advert>? Advrets { get; set; }
    }
}
