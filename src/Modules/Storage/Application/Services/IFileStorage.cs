using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Storage.Application.Models;
using Astrum.Storage.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Astrum.Storage.Services
{
    public interface IFileStorage
    {
        public Task CreateBucket(string bucketName);
        public Task<List<string>> GetBucketNames();
        public Task<UploadResult> UploadFile(FileForm fileForm, CancellationToken cancellationToken = default);
        public Task<UploadResult> UploadFile(IFormFile fileForm, CancellationToken cancellationToken = default);
        public Task<FileForm> GetFile(Guid fileId);
        public Task<string> GetFileUrl(Guid fileId);
        public Task<bool> FileExists(Guid id);
    }
}
