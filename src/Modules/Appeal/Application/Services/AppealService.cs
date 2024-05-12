using Astrum.Account.Services;
using Astrum.Appeal.Aggregates;
using Astrum.Appeal.Domain.Aggregates;
using Astrum.Appeal.Repositories;
using Astrum.Appeal.Specifications;
using Astrum.Appeal.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using Astrum.Storage.ViewModels;
using AutoMapper;

namespace Astrum.Appeal.Services;

//TODO: отрефакторить всё связанное с категориями
public class AppealService : IAppealService
{
    private readonly IAppealCategoryRepository _appealCategoryRepository;
    private readonly IAppealRepository _appealRepository;
    private readonly IMapper _mapper;
    private readonly IUserProfileService _userProfileService;

    private readonly string AppealStorageBucketName = "appeal";
    private readonly IFileStorage _fileStorage;

    public AppealService(IAppealRepository appealRepository, IAppealCategoryRepository appealCategoryRepository,
        IUserProfileService userProfileService, IFileStorage fileStorage,
        IMapper mapper)
    {
        _appealRepository = appealRepository;
        _mapper = mapper;
        _appealCategoryRepository = appealCategoryRepository;
        _userProfileService = userProfileService;
        _fileStorage = fileStorage;
    }

    #region IAppealService Members

    public async Task<SharedLib.Common.Results.Result<List<AppealFormResponse>>> GetAppeals(CancellationToken cancellationToken = default)
    {
        var spec = new GetAppealAsNoTrackingSpec();
        var appeals = await _appealRepository.ListAsync(spec, cancellationToken);
        var appealForms = _mapper.Map<List<AppealFormResponse>>(appeals);
        foreach (var form in appealForms) 
            form.Categories = appeals.First(x => x.Id == form.Id).AppealCategories
            .Select(cat => new AppealCategoryForm {Category = cat.Category.Category, Id = cat.Category.Id}).ToList();
        foreach (var appealForm in appealForms)
        {
            await DefineSenderAndReceiver(appealForm);
        }
        return Result.Success(appealForms);
    }

    public async Task<SharedLib.Common.Results.Result<AppealFormResponse>> GetAppealById(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetAppealByIdSpec(id);
        var appeal = await _appealRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (appeal == null)
        {
            return Result.NotFound("Заявка не найдена.");
        }
        var appealForm = _mapper.Map<AppealFormResponse>(appeal);
        appealForm.Categories = appeal.AppealCategories
            .Select(cat => new AppealCategoryForm { Category = cat.Category.Category, Id = cat.Category.Id }).ToList();
        await DefineSenderAndReceiver(appealForm);
        return Result.Success(appealForm);
    }

    public async Task<SharedLib.Common.Results.Result<AppealFormResponse>> UpdateAppeal(AppealForm appealForm,
        CancellationToken cancellationToken = default)
    {
        var spec = new GetAppealByIdSpec(appealForm.Id);
        var appeal = await _appealRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (appeal is null)
            return Result.NotFound("Заявка не найдена.");
        var categories = appeal.AppealCategories.Select(c => c.Category);
        _mapper.Map(appealForm, appeal);
        GetCategoriesFromForm(appealForm.Body, appeal);
        //foreach (var category in appeal.AppealCategories)
        //    if (appealForm.Body.Categories != null && appealForm.Body.Categories.Any(c => c.Id == category.Id))
        //        category.Category.Category = appealForm.Body.Categories.FirstOrDefault(c => c.Id == category.Id)?.Category;
        try
        {
            await _appealRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении заявки.");
        }

        var response = _mapper.Map<AppealFormResponse>(appeal);
        response.Categories =
            categories.Select(c => new AppealCategoryForm { Category = c.Category, Id = c.Id }).ToList();
        await SetAppealImageUrl(response);
        await DefineSenderAndReceiver(response);
        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<AppealFormResponse>> CreateAppeal(AppealFormData appealForm, CancellationToken cancellationToken)
    {
        var appeal = _mapper.Map<Aggregates.Appeal>(appealForm);
        appeal = await _appealRepository.AddAsync(appeal, cancellationToken);
        //TODO: должен быть общий метод для этого
        var image = appealForm.Image;
        if (image != null)
        {
            var file = _mapper.Map<FileForm>(image);
            byte[] imageData;
            var fileBytes = image.FileBytes;
            using (var binaryReader = new BinaryReader(fileBytes.OpenReadStream())) 
                imageData = binaryReader.ReadBytes((int)fileBytes.Length);
            file.FileBytes = imageData;
            var res = await _fileStorage.UploadFile(file, cancellationToken);
            if (res is { Success: true })
                appeal.CoverImageId = res.UploadedFileId;
        }
        GetCategoriesFromForm(appealForm, appeal);
        try
        {
            await _appealRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании заявки.");
        }
        var newForm = _mapper.Map<AppealFormResponse>(appeal);
        newForm.Categories = appealForm.Categories;
        //await DefineSenderAndReceiver(newForm);
        return Result.Success(newForm);
    }

    public async Task<SharedLib.Common.Results.Result<AppealFormResponse>> DeleteAppeal(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetAppealByIdAsNoTrackingSpec(id);
        var appeal = await _appealRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (appeal == null)
        {
            return Result.NotFound("Заявка не найдена.");
        }
        try
        {
            await _appealRepository.DeleteAsync(appeal, cancellationToken);
            await _appealRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении заявки.");
        }
        return Result.Success(_mapper.Map<AppealFormResponse>(appeal));
    }

    #endregion

    //private async Task SetCategories(AppealForm appealForm, CancellationToken cancellationToken)
    //{
    //    var categories = await GetCategories(appealForm, cancellationToken);
    //    appeal.Categories.AddRange(categories);
    //    await _appealRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
    //}

    //private async Task<List<AppealCategory>> GetCategories(AppealForm appealForm, CancellationToken cancellationToken)
    //{
    //    var categoryIds = appealForm.Body.Categories.Select(cat => cat.Id);
    //    var spec = new GetCategoriesAsNoTrackingSpec(categoryIds);
    //    var categories = await _appealCategoryRepository.ListAsync(spec, cancellationToken);
    //    return categories;
    //}

    private static void GetCategoriesFromForm(AppealFormData appealForm, Aggregates.Appeal appeal)
    {
        appeal.AppealCategories = appealForm.Categories.Select(c => new AppealAppealCategory
        {
            AppealCategoryId = c.Id,
            AppealId = appeal.Id
        }).ToList();
    }

    private async Task DefineSenderAndReceiver(AppealFormResponse appealForm)
    {
        appealForm.FromName = await GetUserInfo(appealForm.From);
        appealForm.ToName = await GetUserInfo(appealForm.To);
    }

    private async Task<string> GetUserInfo(Guid id)
    {
        var user = await _userProfileService.GetUserProfileAsync(id);
        if (user == null)
            throw new ArgumentException("Пользователь не найден");
        return string.Join(" ", user.Data.Surname, user.Data.Name);
    }

    public async Task SetAppealImageUrl(AppealFormResponse appeal)
    {
        if(appeal.CoverImageId.HasValue)
            appeal.CoverUrl = await _fileStorage.GetFileUrl(appeal.CoverImageId.Value);
    }
}