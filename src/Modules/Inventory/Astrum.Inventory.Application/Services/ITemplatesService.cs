using Astrum.Inventory.Application.Models;
using Astrum.Inventory.Application.Models.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Inventory.Application.Services
{
    public interface ITemplatesService
    {
        Task<Result<List<TemplateView>>> GetTemplates(CancellationToken cancellationToken = default);
        Task<Result<TemplateView>> GetTemplateById(Guid id, CancellationToken cancellationToken = default);
        Task<Result<TemplateView>> CreateTemplate(TemplateCreateRequest templateCreate, CancellationToken cancellationToken = default);
        Task<Result<TemplateView>> UpdateTemplate(Guid id, TemplateUpdateRequest templateUpdate, CancellationToken cancellationToken = default);
        Task<Result<TemplateView>> DeleteTemplate(Guid id, CancellationToken cancellationToken = default);
        Task<bool> TemplateAlreadyExists(string title, CancellationToken cancellationToken = default);
        Task<bool> TemplateAlreadyExists(string title, Guid templateId, CancellationToken cancellationToken = default);
        Task<Result<FilterInfo>> GetFilterInfo(CancellationToken cancellationToken = default);
        Task<Result<FilterResponse>> GetFilters(CancellationToken cancellationToken = default);
    }
}
