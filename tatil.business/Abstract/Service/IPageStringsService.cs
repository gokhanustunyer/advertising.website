using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Statics;

namespace tatil.business.Abstract.Service
{
    public interface IPageStringsService
    {
        PageStrings GetSettings();
        Task<bool> UpdateSettings(PageStrings pageStrings);
    }
}
