using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.business.Concrete.Storage.Local;
using tatil.business.Operations;
using tatil.data.Abstract.Advert;
using tatil.data.Abstract.Category;
using tatil.data.Abstract.Filter;
using tatil.data.Contexts;
using tatil.entity.Advert;
using tatil.entity.Category;
using tatil.entity.EntityReferences.Advert;
using tatil.entity.EntityReferences.Advert.Create;
using tatil.entity.EntityReferences.Advert.Edit;
using tatil.entity.Filter;
using tatil.entity.Identity;

namespace tatil.business.Concrete.Service
{
    public class AdvertService : IAdvertService
    {
        private readonly IAdvertRepository _advertRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFilterRepository _filterRepository;
        private readonly IFilterBoxRepository _filterBoxRepository;
        private readonly ILocalStorage _localStorage;
        private readonly TatilDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly ISeoService _seoService;
        public AdvertService(IAdvertRepository advertRepository,
                             IFilterRepository filterRepository,
                             ICategoryRepository categoryRepository,
                             IFilterBoxRepository filterBoxRepository,
                             ILocalStorage localStorage,
                             UserManager<AppUser> userManager,
                             TatilDbContext context,
                             RoleManager<AppRole> roleManager,
                             ISeoService seoService = null)
        {
            _advertRepository = advertRepository;
            _filterRepository = filterRepository;
            _categoryRepository = categoryRepository;
            _filterBoxRepository = filterBoxRepository;
            _localStorage = localStorage;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
            _seoService = seoService;
        }

        public async Task<bool> CreateAdvert(CreateAdvertReference createAdvertReference)
        {
            AppUser publisher = await _userManager.FindByNameAsync(createAdvertReference.userName);
            Advert advert = new()
            {
                Url = UrlNameOperation.CharacterRegulatory(createAdvertReference.Title, (string url) => _advertRepository.Table.FirstOrDefault(p => p.Url == url) != null),
                BedCapacity = (int)createAdvertReference.BedCapacity,
                HumanCapacity = (int)createAdvertReference.HumanCapacity,
                ShortDescription = createAdvertReference.ShortDescription,
                Description = createAdvertReference.Description,
                PhoneNumber = createAdvertReference.PhoneNumber,
                Price = (double)createAdvertReference.Price,
                Title = createAdvertReference.Title,
                Size = (int)createAdvertReference.Size,
                City = createAdvertReference.City,
                District = createAdvertReference.District,
                Neighbourhood = createAdvertReference.Neighborhood,
                IsActive = true,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
                Id = Guid.NewGuid(),
                Categories = new(),
                Filters = new(),
                Publisher = publisher,
                AdvertImages = new(),
            };
            var imagePaths = await _localStorage.UploadAsync("img", createAdvertReference.PostedFiles);
            int index = 0;
            foreach ((string fileName, string filePath) file in imagePaths)
            {
                advert.AdvertImages.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Advert = new() { advert },
                    Storage = "Local",
                    FileName = file.fileName,
                    Index = index,
                    Path = file.filePath
                });
                index++;
            }
            if (createAdvertReference.CategoryIds != null)
            {
                foreach (string categoryId in createAdvertReference.CategoryIds)
                {
                    Category category = await _categoryRepository.GetByIdAsync(categoryId);
                    advert.Categories.Add(category);
                    await _seoService.Create(new entity.Statics.SEO()
                    {
                        Author = "tatilimibuldum.com",
                        Publisher = "tatilimibuldum.com",
                        Description = advert.Title,
                        Url = "/" + category.Url + "/" + advert.Url,
                        Title = advert.Title,
                        Keywords = "kiralık,villa,daire,tatil,oda,havuz,havuzlu,uçak,bilet,araba,araç,bahçe,yeşil",
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Id = Guid.NewGuid()
                    });
                }
            }

            if (createAdvertReference.FilterIds != null)
            {
                foreach (string filterId in createAdvertReference.FilterIds)
                {
                    Filter filter = await _filterRepository.GetByIdAsync(filterId);
                    advert.Filters.Add(filter);
                }
            }
            
            await _advertRepository.AddAsync(advert);
            await _advertRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAdvert(string id)
        {
            Advert advert = await _advertRepository.Table.FirstOrDefaultAsync
                                                         (p => p.Id.ToString() == id);
            _advertRepository.Remove(advert);
            await _advertRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteAdvertImage(string ImageId, string AdvertId)
        {
            Advert advert = await _advertRepository.Table.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id.ToString() == AdvertId);
            if (advert.AdvertImages.Count() == 1) { return false; }
            AdvertImage image = advert.AdvertImages.FirstOrDefault(ai => ai.Id.ToString() == ImageId);
            int deleteIndex = (int)image.Index;
            advert.AdvertImages.Remove(image);
            for(int i=deleteIndex+1;i<advert.AdvertImages.Count()+1;i++)
            {
                advert.AdvertImages.FirstOrDefault(ai => ai.Index == i).Index -= 1;
            }
            _advertRepository.Update(advert);
            await _advertRepository.SaveAsync();
            return true;
        }

        public async Task<Advert> GetAdvertDetailsByIdAsync(string id)
            => await _advertRepository.Table.Include(a => a.AdvertImages.OrderBy(a => a.Index)).Include(a => a.Categories).Include(a => a.Filters).ThenInclude(f => f.FilterBox).Include(a => a.Publisher).FirstOrDefaultAsync(a => a.Id.ToString() == id);

        public async Task<Advert> GetAdvertDetailsByUrlAsync(string advertUrl)
            => await _advertRepository.Table.Include(a => a.AdvertImages.OrderBy(a => a.Index)).Include(a => a.Categories).Include(a => a.Filters).ThenInclude(f => f.FilterBox).Include(a => a.Publisher).FirstOrDefaultAsync(a => a.Url == advertUrl);


        public async Task<Advert> GetAdvertForEditAsync(string id)
            => await _advertRepository.Table.Include(a => a.Categories).Include(a => a.Filters).FirstOrDefaultAsync(a => a.Id.ToString() == id);

        public async Task<List<AdvertImage>> GetAdvertImageAsync(string advertId)
        {
            var model = (await _advertRepository.Table.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id.ToString() == advertId)).AdvertImages;
            foreach (AdvertImage image in model)
            {
                image.Advert = new();
            }
            return model.OrderBy(i => i.Index).ToList();
        }

        public async Task<List<Advert>> GetAdvertsByPublisher(string userName)
            => (await _context.Users.Include(u => u.Advrets).ThenInclude(a => a.AdvertImages.OrderBy(ai => ai.Index)).FirstOrDefaultAsync(u => u.UserName == userName)).Advrets;

        public async Task<AdvertListingModel> GetAdvertsForListing(string? city, string? dist, string? neigh, int? bedC, int? humanC, string? categoryUrl, string? search, List<string>? filters, int page=1, int itemPerPage = 15, string sort="current")
        {
            AdvertListingModel model = new();
            if (filters?.Count > 0)
            {
                if (filters[0] != "" && filters[0] != null)
                {
                    filters = filters[0].Split("|").ToList();
                }
                else
                {
                    filters = new();
                }
            }

            model.Adverts = _advertRepository.Table.Include(a => a.Filters).Include(a => a.Categories).Include(a => a.AdvertImages).Where(a => a.IsActive).ToList();
            if (search != null)
            {
                model.Adverts = model.Adverts.Where(a => a.Title.ToLower().Contains(search.ToLower()) || a.Description.ToLower().Contains(search.ToLower())).ToList();
            }
            if (categoryUrl != null){
                if (categoryUrl != "tatil-evleri")
                {
                    model.Adverts = model.Adverts.Where(a => a.Categories.Where(c => c.Url == categoryUrl).Count() > 0).ToList();
                }
            }
            if (filters != null && filters.Count > 0)
            {
                model.Adverts = model.Adverts.Where(a => a.Filters.Where(f => filters.Contains(f.Id.ToString())).Count() > 0).ToList();
            }
            if (city != null)
            {
                model.Adverts = model.Adverts.Where(a => a.City.ToUpper() == city.ToUpper()).ToList();
                if (dist != null)
                {
                    model.Adverts = model.Adverts.Where(a => a.District.ToUpper() == dist.ToUpper()).ToList();
                    if (neigh != null)
                    {
                        model.Adverts = model.Adverts.Where(a => a.Neighbourhood.ToUpper() == neigh.ToUpper()).ToList();
                    }
                }
            }
            if (bedC > 0)
            {
                model.Adverts = model.Adverts.Where(a => a.BedCapacity == bedC).ToList();
            }
            if (humanC > 0)
            {
                model.Adverts = model.Adverts.Where(a => a.HumanCapacity == humanC).ToList();
            }
            if (sort == "current")
            {
                model.Adverts = model.Adverts.OrderByDescending(m => m.CreateDate).ToList();
            }
            else if (sort == "low")
            {
                model.Adverts = model.Adverts.OrderBy(m => m.Price).ToList();
            }
            else if (sort == "high")
            {
                model.Adverts = model.Adverts.OrderByDescending(m => m.Price).ToList();
            }
            model.Categories = _categoryRepository.GetAll().OrderByDescending(c => c.UpdateDate).ToList();
            model.Filters = _filterBoxRepository.Table.Include(fb => fb.Filters).OrderByDescending(f => f.UpdateDate).ToList();
            model.FilterIds = filters;
            model.City = city;
            model.District = dist;
            model.Neigh = neigh;
            model.HumanCount = (int)humanC;
            model.BedCount = (int)bedC;
            model.TotalItemCount = model.Adverts.Count();
            model.ActivePage = page;
            model.TotalPageCount = (int)Math.Ceiling((decimal)model.TotalItemCount / itemPerPage);
            model.Adverts = model.Adverts.Skip((page - 1) * itemPerPage).Take(itemPerPage).ToList();
            return model;
        }

        public AdvertCreateModel GetAllForCreate()
            => new AdvertCreateModel() { FilterBoxes = _filterBoxRepository.Table.Include(a => a.Filters).ToList(), Categories = _categoryRepository.GetAll().ToList() };

        public List<Advert> GetAllForListing(string? search)
        {
            List<Advert> adverts = _advertRepository.Table.Include(a => a.Publisher).Include(a => a.AdvertImages.OrderBy(ai => ai.Index)).OrderByDescending(a => a.CreateDate).ToList();
            if (search != null)
            {
                adverts = adverts.Where(a => a.Title.Contains(search) || a.Description.Contains(search) || a.Id.ToString().Contains(search) || a.Publisher.Email.Contains(search)).ToList();
            }
            return adverts;
        }

        public async Task<List<Advert>> GetLastAdminAdvrets(int count)
        {
            var users = await _userManager.GetUsersInRoleAsync("Admin");
            AppUser user = users.FirstOrDefault();
            if (user != null)
            {
                return _advertRepository.Table.Include(a => a.AdvertImages).Include(a => a.Categories).Include(a => a.Filters).Include(a => a.Publisher).Where(a => (a.Publisher.UserName == user.UserName) && (a.IsActive)).OrderByDescending(a => a.CreateDate).Take(count).ToList();
            }
            else
            {
                return new();
            }
        }

        public async Task<bool> ReIndex(List<ImageAndIndexsModel> alignmentJson, string advertId)
        {
            if (alignmentJson == null || advertId == null)
            {
                return false;
            }
            Advert advert = await _advertRepository.Table.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id.ToString() == advertId);
            foreach (ImageAndIndexsModel model in alignmentJson)
            {
                advert.AdvertImages.FirstOrDefault(ai => ai.Id.ToString() == model.imageId).Index = model.index;
            }
            _advertRepository.Update(advert);
            await _advertRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateAdvertAsync(AdvertEditModel advertEditModel)
        {
            List<Category> categories = new();
            List<Filter> filters = new();
            if (advertEditModel.CategoryIds != null)
            {
                foreach (string id in advertEditModel.CategoryIds)
                {
                    categories.Add(await _categoryRepository.GetByIdAsync(id));
                }
            }
            if (advertEditModel.FilterIds != null)
            {
                foreach (string id in advertEditModel.FilterIds)
                {
                    filters.Add(await _filterRepository.GetByIdAsync(id));
                }
            }
            Advert advert = await _advertRepository.Table.Include(a => a.Filters).Include(a => a.Categories).FirstOrDefaultAsync(a => a.Id.ToString() == advertEditModel.Id);
            if (advert.Title != advertEditModel.Title)
            {
                advert.Url = (advertEditModel.Url != null) ? advertEditModel.Url : UrlNameOperation.CharacterRegulatory(advertEditModel.Title, (string url) => _advertRepository.Table.FirstOrDefault(p => p.Url == url) != null);
            }
            advert.PhoneNumber = advertEditModel.PhoneNumber;
            advert.UpdateDate = DateTime.Now;
            advert.Price = (double)advertEditModel.Price;
            advert.BedCapacity = (int)advertEditModel.BedCapacity;
            advert.HumanCapacity = (int)advertEditModel.HumanCapacity;
            advert.ShortDescription = advertEditModel.ShortDescription;
            advert.Description = advertEditModel.Description;
            advert.Neighbourhood = advertEditModel.Neighborhood;
            advert.District = advertEditModel.District;
            advert.City = advertEditModel.City;
            advert.IsActive = (bool)((advertEditModel.IsActive != null) ? advertEditModel.IsActive : false);
            advert.Size = (int)advertEditModel.Size;
            advert.Title = advertEditModel.Title;
            advert.Categories = categories;
            advert.Filters = filters;
            _advertRepository.Update(advert);
            await _advertRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UploadAdvertImageAsync(string advertId, IFormFileCollection postedFiles)
        {
            Advert advert = await _advertRepository.Table.Include(a => a.AdvertImages).FirstOrDefaultAsync(a => a.Id.ToString() == advertId);
            var datas = await _localStorage.UploadAsync("img", postedFiles);
            int index = 0;
            advert.AdvertImages.AddRange
                (
                    datas.Select(d => new AdvertImage()
                    {
                        FileName = d.fileName,
                        Path = d.path,
                        Storage = "Azure",
                        Advert = new(){ advert },
                        Index = advert.AdvertImages.Count()+index++,
                        CreateDate = DateTime.Now,
                        UpdateDate = DateTime.Now
                    }).ToList()
                );
            _advertRepository.Update(advert);
            await _advertRepository.SaveAsync();
            return true;
        }
    }
}
