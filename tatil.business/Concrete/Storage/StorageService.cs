using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.business.Concrete.Storage
{
    public class StorageService : IStorageService
    {
        readonly IStorage _storage;

        public StorageService(IStorage storage)
        {
            _storage = storage;
        }
        public string StorageName { get => _storage.GetType().Name; }
        public async Task DeleteAsync(string pathOrContainerName, string fileName)
            => await _storage.DeleteAsync(pathOrContainerName, fileName);
        public List<string> GetFiles(string pathOrContainerName)
            => _storage.GetFiles(pathOrContainerName);
        public bool HasFile(string pathOrContainerName, string fileName)
            => _storage.HasFile(pathOrContainerName, fileName);
        public List<(string fileName, string path)> Upload(string pathOrContainerName, IFormFileCollection files)
            => _storage.Upload(pathOrContainerName, files);
        public async Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFileCollection)
            => await _storage.UploadAsync(pathOrContainerName, formFileCollection);
        public (string fileName, string path) UploadOne(string pathOrContainerName, IFormFile file)
            => _storage.UploadOne(pathOrContainerName, file);
        public async Task<(string fileName, string path)> UploadOneAsync(string pathOrContainerName, IFormFile file)
            => await _storage.UploadOneAsync(pathOrContainerName, file);
    }
}
