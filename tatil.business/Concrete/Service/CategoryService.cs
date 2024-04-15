using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.business.Operations;
using tatil.data.Abstract.Advert;
using tatil.data.Abstract.Category;
using tatil.entity.Advert;
using tatil.entity.Category;

namespace tatil.business.Concrete.Service
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IAdvertRepository _advertRepository;

        public CategoryService(ICategoryRepository categoryRepository, 
                               IAdvertRepository advertRepository)
        {
            _categoryRepository = categoryRepository;
            _advertRepository = advertRepository;
        }

        public async Task<bool> AddAsync(string categoryName)
        {
            await _categoryRepository.AddAsync(new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                Name = categoryName,
                Url = UrlNameOperation.CharacterRegulatory(categoryName, (string url) => _categoryRepository.Table.FirstOrDefault(p => p.Url == url) != null),
            });
            await _categoryRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteCategory(string id)
        {
            await _categoryRepository.RemoveAsync(id);
            await _categoryRepository.SaveAsync();
            return true;
        }

        public List<Category> GetAll()
            => _categoryRepository.GetAll().ToList();

        public async Task<Category> GetByIdWithAdvertsAsync(string id)
           => await _categoryRepository.Table.Include(c => c.Adverts).ThenInclude(a => a.AdvertImages).FirstOrDefaultAsync(c => c.Id.ToString() == id);

        public async Task<bool> RemoveAdvertFromCategoryAsync(string categoryId, string advertId)
        {
            Category category = await _categoryRepository.Table.Include(c => c.Adverts).FirstOrDefaultAsync(c => c.Id.ToString() == categoryId);
            Advert advert = await _advertRepository.Table.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id.ToString() == advertId);
            category.Adverts.Remove(advert);
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateCategoryNameByIdAsync(string id, string categoryName)
        {
            Category category = await _categoryRepository.Table.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            category.Name = categoryName;
            string newUrl = UrlNameOperation.CharacterRegulatory(categoryName, (string url) => _categoryRepository.Table.FirstOrDefault(p => p.Url == url) != null);
            category.Url = newUrl;
            category.UpdateDate = DateTime.Now;
            _categoryRepository.Update(category);
            await _categoryRepository.SaveAsync();
            return true;
        }
    }
}
