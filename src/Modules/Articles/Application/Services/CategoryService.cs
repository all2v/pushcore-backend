using Astrum.Articles.Aggregates;
using Astrum.Articles.Repositories;
using Astrum.Articles.Specifications;
using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;
using Astrum.SharedLib.Common.Results;
using AutoMapper;

namespace Astrum.Articles.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<Result<List<CategoryView>>> GetAll()
        {
            var spec = new GetCategoriesSpec();
            var categories = await _categoryRepository.ListAsync(spec);
            var result = _mapper.Map<List<CategoryView>>(categories);
            return Result.Success(result);
        }

        public async Task<Result<CategoryView>> GetById(Guid id)
        {
            var spec = new GetCategoryByIdSpec(id);
            var category = await _categoryRepository.FirstAsync(spec);
            if (category == null)
            {
                return Result.NotFound("Категория не найдена."); 
            }
            return Result.Success(_mapper.Map<CategoryView>(category));
        }

        public async Task<Result<CategoryView>> CreateAsync(CategoryView categoryView)
        {
            var category = new Category();
            category.Name = categoryView.Name;

            await _categoryRepository.AddAsync(category);
            try
            {
                await _categoryRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при создании категории.");
            }
            return Result.Success(_mapper.Map<CategoryView>(category));
        }

        public async Task<Result<CategoryView>> UpdateAsync(CategoryView categoryView)
        {
            var spec = new GetCategoryByIdSpec(categoryView.Id);
            var category = await _categoryRepository.FirstOrDefaultAsync(spec);
            if (category == null)
                return Result.NotFound("Категория не найдена.");

            category.Name = categoryView?.Name ?? category.Name;
            await _categoryRepository.UpdateAsync(category);
            try
            {
                await _categoryRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при обновлении категории.");
            }
            return _mapper.Map<CategoryView>(category);
        }

        public async Task<Result<CategoryView>> DeleteAsync(Guid id)
        {
            var spec = new GetCategoryByIdSpec(id);

            var category = await _categoryRepository.FirstOrDefaultAsync(spec);
            if (category == null)
                return Result.NotFound("Категория не найдена.");
            try
            {
                await _categoryRepository.DeleteAsync(category);
                await _categoryRepository.UnitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return Result.Error(ex.Message, "Ошибка при удалении категории.");
            }
            return _mapper.Map<CategoryView>(category);
        }

        public async Task<bool> CategoryAlreadyExists(string categoryName)
        {
            var categories = await _categoryRepository.ListAsync();
            return categories.Any(category => category.Name.ToLower().Trim() == categoryName.ToLower().Trim());
        }

        public async Task<Guid> GetOtherCategoryId()
        {
            var spec = new GetOtherCategorySpec();
            var category = await _categoryRepository.FirstOrDefaultAsync(spec);
            return category.Id;
        }

        public async Task<Result<List<CategoryInfo>>> GetInfo()
        {
            var spec = new GetInfoWithIncludesSpec();
            var result = (await _categoryRepository
                .ListAsync(spec))

                .Select(e => new CategoryInfo()
                {
                    Category = _mapper.Map<CategoryView>(e),
                    Tags = e.Tags.Select(t => {
                        var r = _mapper.Map<ArticleCountByTag>(t);
                        r.ArticlesCount = t.ArticleTags.Count;
                        return r;
                    })
                    .Where(e => e.ArticlesCount > 0)
                    .OrderByDescending(e => e.ArticlesCount)
                    .Take(5)
                    .ToList()
                })
                .Where(e=>e.Tags.Count > 0)
                .ToList();
            return Result.Success(result);
        }

        public async Task<Result<FilterResponse>> GetFilters()
        {
            var spec = new GetInfoWithIncludesSpec();
            var resultBlocks = (await _categoryRepository
                .ListAsync(spec))

                .Select(e => new FilterBlock()
                {
                    Name = "tagId",
                    Title = e.Name,
                    FilterItems = e.Tags.Select(t => new FilterItem
                    {
                        Count = t.ArticleTags.Count,
                        Title = t.Name,
                        Value = t.Id.ToString(),
                    })
                    .Where(e => e.Count > 0)
                    .OrderByDescending(e => e.Count)
                    .Take(5)
                    .ToList()
                })
                .Where(e => e.FilterItems.Count > 0)
                .ToList();
            return Result.Success(new FilterResponse { Blocks = resultBlocks });
        }
    }
}
