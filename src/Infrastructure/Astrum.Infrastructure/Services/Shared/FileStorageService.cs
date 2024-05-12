using Astrum.SharedLib.Application.Contracts.Infrastructure.Storage;
using Astrum.SharedLib.Common.Options;
using Microsoft.Extensions.Options;
using NetBox.Extensions;
using Storage.Net;
using Storage.Net.Blobs;

namespace Astrum.Infrastructure.Services.Shared;

public class FileStorageService : IFileStorageService
{
    private readonly string _basePath;
    private readonly IBlobStorage _storage;

    public FileStorageService(IOptions<FileStorageOptions> options)
    {
        _basePath = options.Value.BasePath;
        _storage = StorageFactory.Blobs.DirectoryFiles(_basePath);
    }

    #region IFileStorageService Members

    public async Task DeleteAsync(string filePath, CancellationToken cancellationToken = default)
    {
        await _storage.DeleteAsync(filePath.AsEnumerable(), cancellationToken);
    }

    public async Task<bool> ExistsAsync(string filePath, CancellationToken cancellationToken = default)
    {
        var result = await _storage.ExistsAsync(filePath.AsEnumerable(), cancellationToken);
        return result.First();
    }

    public async Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default)
    {
        return new FileStream(filePath, FileMode.Open);
    }

    public async Task WriteAsync(string filePath, Stream dataStream, bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        await using FileStream fs = new(filePath, FileMode.Create);
        await dataStream.CopyToAsync(fs, cancellationToken);
        dataStream.Close();
    }

    #endregion
}