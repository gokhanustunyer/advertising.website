using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.EntityReferences.Filter;
using tatil.entity.Filter;

namespace tatil.business.Abstract.Service
{
    public interface IFilterService
    {
        Task<bool> AddAsync(string FilterBoxTitle, List<string> FilterNames);
        List<FilterBox> GetAllFilterBoxes();
        List<Filter> GetAll();
        Task<List<Filter>> GetFiltersByFilterBoxId(string filterBoxId);
        Task<FilterEditModel> GetFilterBoxForEdit(string id);
        Task<bool> UpdateFilterName(string filterId, string filterName);
        Task<bool> AddNewFilterToFilterBox(string filterBoxId, string filterName);
        Task<bool> DeleteFilterFromFilterBox(string filterBoxId, string filterId);
        Task<FilterDetailsModel> GetFilterDetailsModelById(string filterId);
        Task<bool> RemoveProductFromFilter(string filterId, string productId);
        Task<bool> UpdateFilterBox(string filterBoxId, string filterBoxName);
        Task<bool> DeleteFilterBox(string filterBoxId);
    }
}
