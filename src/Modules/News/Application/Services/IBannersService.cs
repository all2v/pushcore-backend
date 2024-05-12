using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.ViewModels;

namespace Astrum.News.Services;

public interface IBannersService
{
    Task<SharedLib.Common.Results.Result<List<BannerForm>>> GetBanners(CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<List<BannerForm>>> GetActiveBanners(CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<BannerForm>> GetBannerById(Guid id, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<BannerForm>> UpdateBanner(Guid id, BannerForm form, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<BannerForm>> CreateBanner(BannerForm form, FileForm image = null, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<BannerForm>> DeleteBanner(Guid id, CancellationToken cancellationToken = default);
}