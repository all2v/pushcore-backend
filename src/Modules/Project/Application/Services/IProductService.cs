using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Projects.Services
{
    public interface IProductService
    {
        public Task<Result<ProductView>> Create(ProductRequest request);
        public Task<Result<List<ProductView>>> GetAll();
        public Task<Result<ProductPaginationView>> GetAllPagination(int count, int startIndex,string? predicate);
        public Task<Result<ProductView>> Delete(Guid id);
        public Task<Result<ProductView>> Get(Guid id);
        public Task<Result<ProductView>> Update(Guid id, ProductUpdateDto product);
    }
}
