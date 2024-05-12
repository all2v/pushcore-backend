using Astrum.News.Aggregates;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;
using Astrum.Telegram.Services;
using AutoMapper;

namespace Astrum.News.Services;

public class BannersService : IBannersService
{
    private readonly IBannersRepository _bannerRepository;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;
    private readonly IMessageService _MessageService;

    public BannersService(IBannersRepository bannersRepository, IMapper mapper, IFileStorage fileStorage, IMessageService MessageService)
    {
        _bannerRepository = bannersRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _MessageService = MessageService;
    }

    #region IBannersService Members

    public async Task<SharedLib.Common.Results.Result<List<BannerForm>>> GetBanners(CancellationToken cancellationToken = default)
    {
        var banners = await _bannerRepository.ListAsync(cancellationToken);

        var response = _mapper.Map<List<BannerForm>>(banners);
        foreach(var banner in response)
        {
            if (banner.PictureId.HasValue)
                banner.PictureS3Link = await _fileStorage.GetFileUrl(banner.PictureId.Value);
        }

        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<List<BannerForm>>> GetActiveBanners(CancellationToken cancellationToken = default)
    {
        var spec = new GetActiveBannersSpec();
        var banners = await _bannerRepository.ListAsync(spec);

        var response = _mapper.Map<List<BannerForm>>(banners);
        foreach (var banner in response)
        {
            if (banner.PictureId.HasValue)
                banner.PictureS3Link = await _fileStorage.GetFileUrl(banner.PictureId.Value);
        }

        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<BannerForm>> GetBannerById(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetBannerByIdSpec(id);
        var banner = await _bannerRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (banner == null)
        {
            return Result.NotFound("Баннер не найден.");
        }
        return Result.Success(_mapper.Map<BannerForm>(banner));
    }

    public async Task<SharedLib.Common.Results.Result<BannerForm>> UpdateBanner(Guid id, BannerForm BannerForm,
        CancellationToken cancellationToken = default)
    {
        var banner = _mapper.Map<Banner>(BannerForm);
        var spec = new GetBannerByIdSpec(id);
        var dbBanner = await _bannerRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (dbBanner != null)
        {
            return Result.NotFound("Баннер не найден.");
        }
        dbBanner.Title = banner.Title;
        //dbBanner.PictureS3Link = banner.PictureS3Link;
        dbBanner.IsActive = banner.IsActive;
        //dbBanner.Version++;
        try
        {
            await _bannerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении баннера.");
        }
        return Result.Success(_mapper.Map<BannerForm>(dbBanner));
    }

    public async Task<SharedLib.Common.Results.Result<BannerForm>> CreateBanner(BannerForm BannerForm, FileForm image = null, CancellationToken cancellationToken = default)
    {
        var banner = _mapper.Map<Banner>(BannerForm);

        if (image != null)
        {
            var uploadResult = await _fileStorage.UploadFile(image);
            banner.PictureId = uploadResult.UploadedFileId;
        }

        banner = await _bannerRepository.AddAsync(banner, cancellationToken);
        try
        {
            await _bannerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании баннера.");
        }

        var response = _mapper.Map<BannerForm>(banner);
        if (banner.PictureId.HasValue)
            response.PictureS3Link = await _fileStorage.GetFileUrl(banner.PictureId.Value);
        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<BannerForm>> DeleteBanner(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetBannerByIdSpec(id);
        var banner = await _bannerRepository.FirstOrDefaultAsync(spec, cancellationToken);
        try
        {
            await _bannerRepository.DeleteAsync(banner, cancellationToken);
            await _bannerRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении баннера.");
        }
        return Result.Success(_mapper.Map<BannerForm>(banner));
    }

    #endregion
}