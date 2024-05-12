using Astrum.Appeal.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Appeal.Services;

public interface IAppealService
{
    Task<SharedLib.Common.Results.Result<List<AppealFormResponse>>> GetAppeals(CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<AppealFormResponse>> GetAppealById(Guid id, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<AppealFormResponse>> UpdateAppeal(AppealForm appealForm, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<AppealFormResponse>> CreateAppeal(AppealFormData appealForm, CancellationToken cancellationToken = default);
    Task<SharedLib.Common.Results.Result<AppealFormResponse>> DeleteAppeal(Guid id, CancellationToken cancellationToken = default);
}