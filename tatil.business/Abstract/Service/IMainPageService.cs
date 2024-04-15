using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.EntityReferences.Statics;
using tatil.entity.Statics;

namespace tatil.business.Abstract.Service
{
    public interface IMainPageService
    {
        MainPage GetSettings();
        Task<bool> SaveMainPageSettings(SaveMainPageSettingsModel model);
        Task<bool> CreateLinkBoxAsync(CreateLinkBoxModel model);
        Task<bool> DeleteContentAsync(string id);
        Task<bool> IncreateContentIndexAsync(string id);
        Task<bool> MinusContentIndexAsync(string id);
        Task<MainPageContent> GetContentForEditAsync(string id);
        Task<bool> UpdateLinkBoxAsync(CreateLinkBoxModel model);
    }
}
