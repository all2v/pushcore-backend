using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Projects.Services
{
    public interface ICustomerService
    {
        Task<Result<CustomerView>> Create(CustomerRequest request);
        Task<Result<List<CustomerView>>> GetAll();
        Task<Result<CustomerView>> Update(CustomerView request);
        public Task<Result<CustomerView>> Delete(Guid id);
        public Task<Result<CustomerView>> Get(Guid id);
        Task<bool> Exist(string name);
    }
}
