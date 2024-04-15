using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Category;

namespace tatil.business.Abstract.Service
{
    public interface ICategoryService
    {
        Task<bool> AddAsync(string categoryName);
        List<Category> GetAll();
        Task<Category> GetByIdWithAdvertsAsync(string id);
        Task<bool> RemoveAdvertFromCategoryAsync(string categoryId, string advertId);
        Task<bool> UpdateCategoryNameByIdAsync(string id, string categoryName);
        Task<bool> DeleteCategory(string id);
    }
}
