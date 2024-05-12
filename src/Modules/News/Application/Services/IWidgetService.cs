using Astrum.News.ViewModels;
using Astrum.Storage.ViewModels;

namespace Astrum.News.Services
{
    public interface IWidgetService
    {
        Task<SharedLib.Common.Results.Result<List<WidgetForm>>> GetWidgets(CancellationToken cancellationToken = default);
        Task<SharedLib.Common.Results.Result<List<WidgetForm>>> GetActiveWidgets(CancellationToken cancellationToken = default);
        Task<SharedLib.Common.Results.Result<WidgetForm>> GetWidgetById(Guid id, CancellationToken cancellationToken = default);
        Task<SharedLib.Common.Results.Result<WidgetForm>> UpdateWidget(Guid id, WidgetForm form, CancellationToken cancellationToken = default);
        Task<SharedLib.Common.Results.Result<WidgetForm>> CreateWidget(WidgetForm form, FileForm image = null, CancellationToken cancellationToken = default);
        Task<SharedLib.Common.Results.Result<WidgetForm>> DeleteWidget(Guid id, CancellationToken cancellationToken = default);
    }
}
