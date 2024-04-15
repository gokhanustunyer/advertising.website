using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tatil.business.Concrete.Storage
{
    public interface IStorage
    {
        List<(string fileName, string path)> Upload(string pathOrContainerName, IFormFileCollection formFileCollection);
        (string fileName, string path) UploadOne(string pathOrContainerName, IFormFile file);
        Task<List<(string fileName, string path)>> UploadAsync(string pathOrContainerName, IFormFileCollection formFileCollection);
        Task<(string fileName, string path)> UploadOneAsync(string pathOrContainerName, IFormFile file);
        Task DeleteAsync(string pathOrContainerName, string fileName);
        List<string> GetFiles(string pathOrContainerName);
        bool HasFile(string pathOrContainerName, string fileName);
    }
}
