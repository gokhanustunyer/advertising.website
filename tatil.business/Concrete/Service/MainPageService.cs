using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tatil.business.Abstract.Service;
using tatil.business.Concrete.Storage.Local;
using tatil.data.Abstract.Statics;
using tatil.entity.EntityReferences.Statics;
using tatil.entity.Statics;

namespace tatil.business.Concrete.Service
{
    public class MainPageService : IMainPageService
    {
        private readonly IMainPageRepository _mainPageRepository;
        private readonly ILocalStorage _localStorage;

        public MainPageService(IMainPageRepository mainPageRepository,
                               ILocalStorage localStorage)
        {
            _mainPageRepository = mainPageRepository;
            _localStorage = localStorage;
        }

        public async Task<bool> CreateLinkBoxAsync(CreateLinkBoxModel model)
        {
            MainPageContent content = new();
            content.CreateDate = DateTime.Now;
            content.UpdateDate = DateTime.Now;
            content.LinkBoxes = new();
            content.Id = Guid.NewGuid();
            content.ContentTitle = model.MainTitle;
            content.IsFullTextContent = false;
            content.IsImageContent = false;
            content.IsLinkBoxContent = true;
            content.LinkBoxesIsBlack = (model.LinkCartFilter == "blackfilter") ? true : false;
            if (model.ButtonLink != null)
            {
                content.ButtonLink = model.ButtonLink;
                content.ButtonText = model.ButtonText;
            }
            if (model.BackgroundImage != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.BackgroundImage);
                content.BackgroundImagePath = imagePath.path;
                content.BackgorundFilterIsWhite = (model.BackgroundFilter == "black") ? false : true;
            }
            if (model.Image1 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.Image1);
                content.LinkBoxes.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Link = model.Link1,
                    ImagePath = imagePath.path,
                    Title = model.Title1
                });
            }
            if (model.Image2 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.Image2);
                content.LinkBoxes.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Link = model.Link2,
                    ImagePath = imagePath.path,
                    Title = model.Title2
                });
            }
            if (model.Image3 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.Image3);
                content.LinkBoxes.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Link = model.Link3,
                    ImagePath = imagePath.path,
                    Title = model.Title3
                });
            }
            if (model.Image4 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.Image4);
                content.LinkBoxes.Add(new()
                {
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                    Id = Guid.NewGuid(),
                    Link = model.Link4,
                    ImagePath = imagePath.path,
                    Title = model.Title4
                });
            }
            MainPage mainPage = _mainPageRepository.Table.Include(m => m.Contents).FirstOrDefault();
            content.Index = mainPage.Contents.Count;
            content.MainPage = mainPage;
            _mainPageRepository.Context.MainPageContent.Add(content);
            await _mainPageRepository.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteContentAsync(string id)
        {
            MainPage mainPage = GetSettings();
            MainPageContent mainPageContent = await _mainPageRepository.Context.MainPageContent.FirstOrDefaultAsync(c => c.Id.ToString() == id);
            mainPage.Contents.Remove(mainPageContent);
            _mainPageRepository.Update(mainPage);
            await _mainPageRepository.SaveAsync();
            return true;
        }

        public async Task<MainPageContent> GetContentForEditAsync(string id)
            => await _mainPageRepository.Context.MainPageContent.Include(c => c.LinkBoxes).FirstOrDefaultAsync(c => c.Id.ToString() == id);

        public MainPage GetSettings()
            =>  _mainPageRepository.Table.Include(m => m.Contents.OrderBy(c => c.Index)).ThenInclude(c => c.LinkBoxes).FirstOrDefault();

        public async Task<bool> IncreateContentIndexAsync(string id)
        {
            MainPage mainPage = _mainPageRepository.Table.Include(m => m.Contents).FirstOrDefault();
            int active_index = mainPage.Contents.FirstOrDefault(c => c.Id.ToString() == id).Index;
            if (active_index != mainPage.Contents.Count - 1)
            {
                mainPage.Contents.FirstOrDefault(c => c.Index == active_index+1).Index -= 1;
                mainPage.Contents.FirstOrDefault(c => c.Id.ToString() == id).Index += 1;
                _mainPageRepository.Update(mainPage);
                await _mainPageRepository.SaveAsync();
            }
            return true;
        }

        public async Task<bool> MinusContentIndexAsync(string id)
        {
            MainPage mainPage = _mainPageRepository.Table.Include(m => m.Contents).FirstOrDefault();
            int active_index = mainPage.Contents.FirstOrDefault(c => c.Id.ToString() == id).Index;
            if (active_index != 0)
            {
                mainPage.Contents.FirstOrDefault(c => c.Index == active_index - 1).Index += 1;
                mainPage.Contents.FirstOrDefault(c => c.Id.ToString() == id).Index -= 1;
                _mainPageRepository.Update(mainPage);
                await _mainPageRepository.SaveAsync();
            }
            return true;
        }

        public async Task<bool> SaveMainPageSettings(SaveMainPageSettingsModel model)
        {
            MainPage mainPage = _mainPageRepository.Table.FirstOrDefault();
            mainPage.headerText = model.HeaderText;
            mainPage.welcomeText = model.WelcomeText;
            mainPage.StreamerCount = model.PublisherCount;
            mainPage.CustomerCount = model.UserCount;
            mainPage.CityCount = model.CityCount;
            mainPage.welcomeTitle = model.WelcomeTitle;
            if (model.NumericsImage != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.NumericsImage);
                mainPage.NumericDatasImagePath = imagePath.path;
            }
            if (model.HeaderImage != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.HeaderImage);
                mainPage.headerImagePath = imagePath.path;
            }
            if (model.WelcomeImage1 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.WelcomeImage1);
                mainPage.welcomeTextImagePath1 = imagePath.path;
            }
            if (model.WelcomeImage2 != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.WelcomeImage2);
                mainPage.welcomeTextImagePath2 = imagePath.path;
            }
            _mainPageRepository.Update(mainPage);
            await _mainPageRepository.SaveAsync();
            return true;
        }

        public async Task<bool> UpdateLinkBoxAsync(CreateLinkBoxModel model)
        {
            MainPageContent content = await _mainPageRepository.Context.MainPageContent.Include(c => c.MainPage).Include(c => c.LinkBoxes).FirstOrDefaultAsync(c => c.Id.ToString() == model.Id);
            content.UpdateDate = DateTime.Now;
            content.ContentTitle = model.MainTitle;
            content.LinkBoxesIsBlack = (model.LinkCartFilter == "blackfilter") ? true : false;
            content.ButtonLink = model.ButtonLink;
            content.ButtonText = model.ButtonText;
            content.BackgorundFilterIsWhite = (model.BackgroundFilter == "black") ? false : true;
            if (content.LinkBoxes == null) { content.LinkBoxes = new(); }
            int count = (4 - content.LinkBoxes.Count);
            for (int i = 0; i < count;i++) { content.LinkBoxes.Add(null); }
            if (model.BackgroundImage != null)
            {
                var imagePath = await _localStorage.UploadOneAsync("img", model.BackgroundImage);
                content.BackgroundImagePath = imagePath.path;
            }
            if (content.LinkBoxes[0] != null)
            {
                if (model.Image1 != null)
                {
                    var imagePath = await _localStorage.UploadOneAsync("img", model.Image1);
                    content.LinkBoxes[0].UpdateDate = DateTime.Now;
                    content.LinkBoxes[0].ImagePath = imagePath.path;
                }
                content.LinkBoxes[0].Link = model.Link1;
                content.LinkBoxes[0].Title = model.Title1;
            }
            if (content.LinkBoxes[1] != null)
            {
                if (model.Image2 != null)
                {
                    var imagePath = await _localStorage.UploadOneAsync("img", model.Image2);
                    content.LinkBoxes[1].UpdateDate = DateTime.Now;
                    content.LinkBoxes[1].ImagePath = imagePath.path;
                }
                content.LinkBoxes[1].Link = model.Link2;
                content.LinkBoxes[1].Title = model.Title2;
            }
            if (content.LinkBoxes[2] != null)
            {
                if (model.Image3 != null)
                {
                    var imagePath = await _localStorage.UploadOneAsync("img", model.Image3);
                    content.LinkBoxes[2].UpdateDate = DateTime.Now;
                    content.LinkBoxes[2].ImagePath = imagePath.path;
                }
                content.LinkBoxes[2].Link = model.Link3;
                content.LinkBoxes[2].Title = model.Title3;
            }
            if (content.LinkBoxes[3] != null)
            {
                if (model.Image4 != null)
                {
                    var imagePath = await _localStorage.UploadOneAsync("img", model.Image4);
                    content.LinkBoxes[3].UpdateDate = DateTime.Now;
                    content.LinkBoxes[3].ImagePath = imagePath.path;
                }
                content.LinkBoxes[3].Link = model.Link4;
                content.LinkBoxes[3].Title = model.Title4;
            }
            if (model.RemoveBackground == "removeBg") { content.BackgroundImagePath = null; content.BackgorundFilterIsWhite = false; }
            _mainPageRepository.Context.MainPageContent.Update(content);
            await _mainPageRepository.SaveAsync();
            return true;
        }
    }
}
