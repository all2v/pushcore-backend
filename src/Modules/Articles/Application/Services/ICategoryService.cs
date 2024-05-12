using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Articles.Services
{
    public interface ICategoryService
    {
        public Task<Result<List<CategoryView>>> GetAll();
        public Task<Result<CategoryView>> GetById(Guid id);
        public Task<Result<CategoryView>> CreateAsync(CategoryView categoryView);
        public Task<Result<CategoryView>> UpdateAsync(CategoryView categoryView);
        public Task<Result<CategoryView>> DeleteAsync(Guid id);
        public Task<bool> CategoryAlreadyExists(string categoryName);
        public Task<Result<List<CategoryInfo>>> GetInfo();
        public Task<Result<FilterResponse>> GetFilters();
        public Task<Guid> GetOtherCategoryId();
    }
}
