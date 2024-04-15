using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Net;

namespace tatil.business.Concrete.Storage.Local
{
    public class LocalStorage : Storage, ILocalStorage
    {
        private readonly IHostingEnvironment _hostingEnvironment;
        readonly BlobServiceClient _blobServiceClient;
        BlobContainerClient _blobContainerClient;
        private readonly IConfiguration _configuration;
        public LocalStorage(IHostingEnvironment hostingEnvironment,
                            IConfiguration configuration)
        {
            _hostingEnvironment = hostingEnvironment;
            _blobServiceClient = new(configuration["Storage:Azure"]);
            _configuration = configuration;
        }

        public Task DeleteAsync(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }

        public List<string> GetFiles(string pathOrContainerName)
        {
            throw new NotImplementedException();
        }

        public List<(string fileName, string path)> Upload(string pathOrContainerName, IFormFileCollection formFileCollection)
        {
            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in formFileCollection)
            {
                string fileName = Guid.NewGuid() + "." + "webp";
                string uploadPath = Path.Combine
                        (_hostingEnvironment.WebRootPath, pathOrContainerName, fileName);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    using (var image = Image.Load(fileBytes))
                    {
                        var webpOptions = new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                        {
                            Quality = 100
                        };
                        using (var fileStream = File.Create(uploadPath))
                        {
                            image.Save(fileStream, webpOptions);
                        }
                        datas.Add((fileName,
                            $"/{pathOrContainerName}/{fileName}"));
                    }
                }
            }
            return datas;
        }

        bool IStorage.HasFile(string pathOrContainerName, string fileName)
        {
            throw new NotImplementedException();
        }
        public async Task<List<(string fileName, string path)>> UploadAsync(string containerName, IFormFileCollection files)
        {
            List<(string fileName, string path)> datas = new();
            foreach (IFormFile file in files)
            {
                string fileName = Guid.NewGuid() + "." + "webp";
                string uploadPath = Path.Combine
                        (_hostingEnvironment.WebRootPath, containerName, fileName);
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    using (var image = Image.Load(fileBytes))
                    {
                        var webpOptions = new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                        {
                            Quality = 100
                        };
                        using (var fileStream = File.Create(uploadPath))
                        {
                            image.Save(fileStream, webpOptions);
                        }
                        datas.Add((fileName,
                            $"/{containerName}/{fileName}"));
                    }
                }
            }
            return datas;
        }

        public (string fileName, string path) UploadOne(string pathOrContainerName, IFormFile file)
        {
            (string fileName, string path) datas = new();
            string fileName = Guid.NewGuid() + "." + "webp";
            string uploadPath = Path.Combine
                    (_hostingEnvironment.WebRootPath, pathOrContainerName, fileName);
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                using (var image = Image.Load(fileBytes))
                {
                    var webpOptions = new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                    {
                        Quality = 100
                    };
                    using (var fileStream = File.Create(uploadPath))
                    {
                        image.Save(fileStream, webpOptions);
                    }
                    datas = ((fileName, $"/{pathOrContainerName}/{fileName}"));
                }
            }
       
            return datas;
        }

        public async Task<(string fileName, string path)> UploadOneAsync(string containerName, IFormFile file)
        {
            string fileName = Guid.NewGuid() + "." + "webp";
            string uploadPath = Path.Combine(_hostingEnvironment.WebRootPath, containerName, fileName);
            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                using (var image = Image.Load(fileBytes))
                {
                    var webpOptions = new SixLabors.ImageSharp.Formats.Webp.WebpEncoder
                    {
                        Quality = 100
                    };
                    using (var fileStream = File.Create(uploadPath))
                    {
                        image.Save(fileStream, webpOptions);
                    }
                }
            }
            return (fileName, $"/{containerName}/{fileName}");
        }
    }
}
