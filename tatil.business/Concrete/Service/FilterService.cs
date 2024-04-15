using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.data.Abstract.Advert;
using tatil.data.Abstract.Category;
using tatil.data.Abstract.Filter;
using tatil.entity.Advert;
using tatil.entity.EntityReferences.Filter;
using tatil.entity.Filter;

namespace tatil.business.Concrete.Service
{
    public class FilterService : IFilterService
    {
        private readonly IFilterRepository _filterRepository;
        private readonly IFilterBoxRepository _filterBoxRepository;
        private readonly IAdvertRepository _advertRepository;

        public FilterService(IFilterRepository filterRepository,
                             IFilterBoxRepository filterBoxRepository,
                             IAdvertRepository advertRepository)
        {
            _filterRepository = filterRepository;
            _filterBoxRepository = filterBoxRepository;
            _advertRepository = advertRepository;
        }

        public async Task<bool> AddAsync(string filterBoxName, List<string> filterNames)
        {
            FilterBox filterBox = new()
            {
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                FilterBoxTitle = filterBoxName,
            };
            List<Filter> filters = new();
            foreach (string filterName in filterNames)
            {
                filters.Add(new Filter()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    FilterBox = filterBox,
                    FilterTitle = filterName,
                    Id = Guid.NewGuid(),
                });
            }
            filterBox.Filters = filters;
            await _filterBoxRepository.AddAsync(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }
        public List<FilterBox> GetAllFilterBoxes()
            => _filterBoxRepository.Table.Include(fb => fb.Filters).ToList();
        public List<Filter> GetAll()
            => _filterRepository.GetAll().ToList();

        public async Task<List<Filter>> GetFiltersByFilterBoxId(string filterBoxId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            for (int i = 0; i < filterBox.Filters.Count(); i++)
            {
                filterBox.Filters[i].FilterBox = null;
            }
            return filterBox.Filters.ToList();
        }

        public async Task<FilterEditModel> GetFilterBoxForEdit(string id)
        {
            var result = new FilterEditModel();
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == id);
            List<string> filterIds = filterBox.Filters.Select(f => f.Id.ToString()).ToList();
            List<Advert> adverts = _advertRepository.Table.Include(p => p.AdvertImages).Include(p => p.Filters).Where(p => p.Filters.Where(f => filterIds.Contains(f.Id.ToString())).ToList().Count > 0).ToList();
            return new()
            {
                Filters = filterBox.Filters.ToList(),
                FilterBoxTitle = filterBox.FilterBoxTitle,
                Id = filterBox.Id.ToString(),
                Adverts = adverts
            };
        }

        public async Task<bool> UpdateFilterName(string filterId, string filterName)
        {
            Filter filter = await _filterRepository.Table.FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            filter.UpdateDate = DateTime.Now;
            filter.FilterTitle = filterName;
            _filterRepository.Update(filter);
            await _filterRepository.SaveAsync();
            return true;
        }

        public async Task<bool> AddNewFilterToFilterBox(string filterBoxId, string filterName)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            Filter filter = new()
            {
                Id = Guid.NewGuid(),
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                FilterTitle = filterName,
                Adverts = new(),
                FilterBox = filterBox,
            };
            await _filterRepository.AddAsync(filter);
            await _filterRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteFilterFromFilterBox(string filterBoxId, string filterId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            Filter filter = await _filterRepository.Table.FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            filterBox.Filters.Remove(filter);
            _filterRepository.Table.Remove(filter);
            await _filterRepository.SaveAsync();
            return true;
        }

        public async Task<FilterDetailsModel> GetFilterDetailsModelById(string filterId)
        {
            Filter filter = await _filterRepository.Table.Include(f => f.Adverts).ThenInclude(p => p.AdvertImages).FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            return new() { FilterTitle = filter.FilterTitle, FilterId = filter.Id.ToString(), Adverts = filter.Adverts };
        }
        public async Task<bool> RemoveProductFromFilter(string filterId, string productId)
        {
            Filter filter = await _filterRepository.Table.Include(f => f.Adverts).FirstOrDefaultAsync(f => f.Id.ToString() == filterId);
            Advert advert = await _advertRepository.GetByIdAsync(productId);
            filter.Adverts.Remove(advert);
            _filterRepository.Update(filter);
            await _filterRepository.SaveAsync();
            return true;
        }
        public async Task<bool> UpdateFilterBox(string filterBoxId, string filterBoxName)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            filterBox.FilterBoxTitle = filterBoxName;
            _filterBoxRepository.Update(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }
        public async Task<bool> DeleteFilterBox(string filterBoxId)
        {
            FilterBox filterBox = await _filterBoxRepository.Table.Include(fb => fb.Filters).FirstOrDefaultAsync(fb => fb.Id.ToString() == filterBoxId);
            _filterRepository.RemoveRange(filterBox.Filters.ToList());
            await _filterRepository.SaveAsync();
            _filterBoxRepository.Remove(filterBox);
            await _filterBoxRepository.SaveAsync();
            return true;
        }
    }
}
