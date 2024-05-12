using Astrum.Articles.Aggregates;
using Astrum.Articles.Requests;
using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Articles.Services
{
    public interface ITagService
    {
        public Task<Result<TagView>> Create(TagRequest request);
        public Task<Result<List<TagView>>> GetAll();
        public Task<Result<List<TagView>>> GetByCategoryId(Guid id);
        public Task<Result<List<TagView>>> GetByPredicate(int count = 10, string predicate = null);
        //public Task<Result<Dictionary<Guid, Dictionary<Guid, int>>>> GetArticlesByTagInfo();
    }
}
