using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.data.Abstract.Statics;
using tatil.entity.Statics;

namespace tatil.business.Concrete.Service
{
    public class PageStringService: IPageStringsService
    {
        private readonly IPageStringsRepository _pageStringsRepository;

        public PageStringService(IPageStringsRepository pageStringsRepository)
        {
            _pageStringsRepository = pageStringsRepository;
        }

        public PageStrings GetSettings()
            => _pageStringsRepository.Table.FirstOrDefault();

        public async Task<bool> UpdateSettings(PageStrings pageStrings)
        {
            PageStrings defaultPs = _pageStringsRepository.Table.FirstOrDefault();
            defaultPs.InstagramLink = pageStrings.InstagramLink;
            defaultPs.FacebookLink = pageStrings.FacebookLink;
            defaultPs.WPNumber = pageStrings.WPNumber;
            defaultPs.PhoneNumber = pageStrings.PhoneNumber;
            defaultPs.AboutUs = pageStrings.AboutUs;
            defaultPs.TwitterLink = pageStrings.TwitterLink;
            defaultPs.EmailAddress = pageStrings.EmailAddress;
            defaultPs.FooterDescription = pageStrings.FooterDescription;
            defaultPs.ContactAddress = pageStrings.ContactAddress;
            defaultPs.UpdateDate = DateTime.Now;
            _pageStringsRepository.Update(defaultPs);
            await _pageStringsRepository.SaveAsync();
            return true;
        }
    }
}
