using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Projects.Services
{
    public interface ICustomFieldService
    {
        Task<Result<CustomFieldView>> Create(CustomFieldRequest request);
        public Task<Result<CustomFieldView>> Delete(Guid id);
    }
}
