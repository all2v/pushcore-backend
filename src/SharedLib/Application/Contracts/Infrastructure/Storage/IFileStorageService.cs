namespace Astrum.SharedLib.Application.Contracts.Infrastructure.Storage;

public interface IFileStorageService
{
    Task DeleteAsync(string filePath, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string filePath, CancellationToken cancellationToken = default);
    Task<Stream> OpenReadAsync(string filePath, CancellationToken cancellationToken = default);

    Task WriteAsync(string filePath, Stream dataStream, bool overwrite = false,
        CancellationToken cancellationToken = default);
}