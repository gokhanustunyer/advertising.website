using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Statics;

namespace tatil.business.Abstract.Service
{
    public interface ISeoService
    {
        SEO GetByUrl(string url);
        List<SEO> GetAll();
        Task<bool> Create(SEO newSeo);
        Task<bool> Delete(string url);
        Task<bool> Update(SEO updateSeo);
        Task<bool> DeleteById(string id);
        SEO GetById(string id);
    }
}
