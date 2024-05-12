using Astrum.Appeal.Application.Services;
using Astrum.Appeal.Services;
using Astrum.Appeal.ViewModels;
using HotChocolate;
using HotChocolate.Types;

namespace Astrum.Appeal.GraphQL;

public class QueryAppeal
{
    [UsePaging(MaxPageSize = 20, IncludeTotalCount = true)]
    [UseSorting]
    [UseFiltering]
    public async Task<List<AppealFormResponse>> GetAppeals([Service] IAppealService appealService,
        CancellationToken cancellationToken)
    {
        return await appealService.GetAppeals(cancellationToken);
    }

    [UseSorting]
    [UseFiltering]
    public async Task<List<AppealCategoryForm>> GetCategories([Service] IAppealCategoryService appealService,
        CancellationToken cancellationToken)
    {
        return await appealService.GetAppealCategories();
    }

    public async Task<AppealFormResponse> GetAppealById([Service] IAppealService appealService, Guid id)
    {
        return await appealService.GetAppealById(id);
    }
}