using Astrum.Appeal.ViewModels;

namespace Astrum.Appeal.Application.Services;

public interface IAppealCategoryService
{
    Task<SharedLib.Common.Results.Result<List<AppealCategoryForm>>> GetAppealCategories();
    Task<SharedLib.Common.Results.Result<AppealCategoryForm>> CreateCategory(string name);
    Task<SharedLib.Common.Results.Result<AppealCategoryForm>> UpdateCategory(Guid id, string name);
    Task<SharedLib.Common.Results.Result<AppealCategoryForm>> DeleteCategory(Guid id);
    Task<bool> CategoryAlreadyExists(string categoryName, CancellationToken cancellationToken = default);

}