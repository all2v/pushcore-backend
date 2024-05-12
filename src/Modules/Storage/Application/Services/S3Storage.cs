using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Astrum.Storage.Aggregates;
using Astrum.Storage.Application.Models;
using Astrum.Storage.Models;
using Astrum.Storage.Repositories;
using Astrum.Storage.ViewModels;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Minio;
using Minio.Exceptions;
using static System.Net.WebRequestMethods;

namespace Astrum.Storage.Services
{
    public class S3Storage : IFileStorage
    {
        private readonly IFileRepository _fileRepository;
        private readonly IMapper _mapper;
        private readonly string BucketName;
        private readonly string S3Link;

        public S3Storage(IOptionsMonitor<S3StorageRequisites> optionsMonitor, IFileRepository fileRepository, IMapper mapper)
        {
            _mapper = mapper;
            _fileRepository = fileRepository;
            var requisites = optionsMonitor.CurrentValue;
            S3Client = new MinioClient()
                .WithEndpoint(requisites.Endpoint)
                .WithCredentials(requisites.AccessKey, requisites.SecretKey)
                .WithSSL(requisites.Secure)
                .Build();
            S3Link = requisites.Link;
            BucketName = requisites.Bucket;
        }

        public MinioClient S3Client { get; set; }

        public async Task CreateBucket(string bucketName)
        {
            var mbArgs = new MakeBucketArgs()
                .WithBucket(bucketName);
            await S3Client.MakeBucketAsync(mbArgs);
        }

        public async Task<List<string>> GetBucketNames()
        {
            var buckets = await S3Client.ListBucketsAsync();
            return buckets.Buckets.Select(b => b.Name).ToList();
        }
        public async Task<UploadResult> UploadFile(IFormFile fileForm, CancellationToken cancellationToken = default)
        {
            var file = _mapper.Map<FileForm>(fileForm);
            byte[] imageData = null;
            using (var binaryReader = new BinaryReader(fileForm.OpenReadStream()))
            {
                imageData = binaryReader.ReadBytes((int)fileForm.Length);
            }
            file.FileBytes = imageData;

            return await UploadFile(file, cancellationToken);
        }
        public async Task<UploadResult> UploadFile(FileForm fileForm, CancellationToken cancellationToken = default)
        {
            var result = new UploadResult();
            try
            {
                if (fileForm == null || fileForm.FileBytes == null || fileForm.FileBytes.Length == 0)
                    return result;
                fileForm.HashSum = fileForm.FileBytes.GetHashCode().ToString();
                // Make a bucket on the server, if not already present.
                var file = _mapper.Map<StorageFile>(fileForm);

                file = await _fileRepository.AddAsync(file, cancellationToken);
                await _fileRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
                var beArgs = new BucketExistsArgs()
                    .WithBucket(BucketName);
                bool found = await S3Client.BucketExistsAsync(beArgs).ConfigureAwait(false);
                if (!found)
                {
                    var mbArgs = new MakeBucketArgs()
                        .WithBucket(BucketName);
                    await S3Client.MakeBucketAsync(mbArgs).ConfigureAwait(false);
                }
                using (MemoryStream memStream = new MemoryStream(fileForm.FileBytes))
                {
                    //await memStream.WriteAsync(fileForm.FileBytes/*, 0, fileForm.FileBytes.Length,*/);

                    // Upload a file to bucket.
                    var putObjectArgs = new PutObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(file.Id.ToString())
                        .WithStreamData(memStream)
                        .WithObjectSize(memStream.Length)
                        .WithContentType(file.ContentType);
                    await S3Client.PutObjectAsync(putObjectArgs, cancellationToken).ConfigureAwait(false);
                }
                result.Success = true;
                result.UploadedFileId = file.Id;
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine("File Upload Error: {0}", e.Message);
                return result;
            }
        }

        public async Task<FileForm> GetFile(Guid fileId)
        {
            try
            {
                var file = await _fileRepository.GetByIdAsync(fileId);
                if (file == null)
                    return null;
                var fileForm = _mapper.Map<FileForm>(file);
                using (MemoryStream memStream = new MemoryStream())
                {
                    byte[] bytes;
                    Console.WriteLine("Running example for API: GetObjectAsync");
                    var args = new GetObjectArgs()
                        .WithBucket(BucketName)
                        .WithObject(fileForm.Id.ToString())
                        .WithCallbackStream((s) => {
                            //bytes = new byte[s.Length];
                                s.CopyTo(memStream);
                            });
                    var stat = await S3Client.GetObjectAsync(args);
                    bytes = memStream.ToArray();
                    //var a = await memStream.ReadAsync(bytes, 0, bytes.Length);
                    //await memStream.ReadAsync(bytes, 0, bytes.Length);
                    fileForm.FileBytes = bytes;
                    fileForm.FileStream = null;
                    return fileForm;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"[Bucket]  Exception: {e}");
                return null;
            }
        }

        public async Task<bool> FileExists(Guid id)
        {
            var file = await _fileRepository.FirstOrDefaultAsync(c => c.Id == id);
            if (file == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<string> GetFileUrl(Guid fileId)
        {
            return $"{S3Link}/{BucketName}/{fileId}";
        }
    }
}
