using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.entity.Advert;
using tatil.entity.EntityReferences.Advert;
using tatil.entity.EntityReferences.Advert.Create;
using tatil.entity.EntityReferences.Advert.Edit;

namespace tatil.business.Abstract.Service
{
    public interface IAdvertService
    {
        AdvertCreateModel GetAllForCreate();
        Task<bool> CreateAdvert(CreateAdvertReference createAdvertReference);
        Task<Advert> GetAdvertForEditAsync(string id);
        Task<List<AdvertImage>> GetAdvertImageAsync(string advertId);
        Task<bool> ReIndex(List<ImageAndIndexsModel> alignmentJson, string advertId);
        Task<bool> DeleteAdvertImage(string ImageId, string AdvertId);
        Task<bool> UploadAdvertImageAsync(string advertId, IFormFileCollection postedFiles);
        Task<bool> UpdateAdvertAsync(AdvertEditModel advert);
        Task<bool> DeleteAdvert(string id);
        Task<List<Advert>> GetAdvertsByPublisher(string userName);
        Task<AdvertListingModel> GetAdvertsForListing(string? city, string? dist, string? neigh, int? bedC, int? humanC, string? categoryUrl, string search, List<string>? filters, int page = 1, int itemPerPage = 15, string sort = "current");
        Task<Advert> GetAdvertDetailsByIdAsync(string id);
        Task<Advert> GetAdvertDetailsByUrlAsync(string advertUrl);
        List<Advert> GetAllForListing(string? Search);
        Task<List<Advert>> GetLastAdminAdvrets(int count);
    }
}
