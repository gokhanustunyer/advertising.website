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
    public class SeoService : ISeoService
    {
        private readonly ISeoRepository _seoRepository;

        public SeoService(ISeoRepository seoRepository)
        {
            _seoRepository = seoRepository;
        }

        public async Task<bool> Create(SEO newSeo)
        {
            SEO check = _seoRepository.Table.FirstOrDefault(s => s.Url == newSeo.Url);
            if (check == null)
            {
                newSeo.Id = Guid.NewGuid();
                newSeo.CreateDate = DateTime.Now;
                newSeo.UpdateDate = DateTime.Now;
                await _seoRepository.AddAsync(newSeo);
                await _seoRepository.SaveAsync();
            }
            return true;
        }

        public async Task<bool> Delete(string url)
        {
            SEO seo = _seoRepository.Table.FirstOrDefault(s => s.Url == url);
            _seoRepository.Remove(seo);
            await _seoRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteById(string id)
        {
            SEO seo = _seoRepository.Table.FirstOrDefault(s => s.Id.ToString() == id);
            _seoRepository.Remove(seo);
            await _seoRepository.SaveAsync();
            return true;
        }

        public List<SEO> GetAll()
            => _seoRepository.GetAll().ToList();

        public SEO GetById(string id)
            => _seoRepository.Table.FirstOrDefault(s => s.Id.ToString() == id);

        public SEO GetByUrl(string url)
            => _seoRepository.Table.FirstOrDefault(s => s.Url == url);

        public async Task<bool> Update(SEO updateSeo)
        {
            SEO seo = _seoRepository.Table.FirstOrDefault(s => s.Id == updateSeo.Id);
            if (seo.Url != updateSeo.Url)
            {
                SEO check = _seoRepository.Table.FirstOrDefault(s => s.Url == updateSeo.Url);
                if (check != null)
                {
                    return false;
                }
            }
            seo.UpdateDate = DateTime.Now;
            seo.Publisher = updateSeo.Publisher;
            seo.Author = updateSeo.Author;
            seo.Title = updateSeo.Title;
            seo.Url = updateSeo.Url;
            seo.Description = updateSeo.Description;
            seo.Keywords = updateSeo.Keywords;
            _seoRepository.Update(seo);
            await _seoRepository.SaveAsync();
            return true;
        }
    }
}
