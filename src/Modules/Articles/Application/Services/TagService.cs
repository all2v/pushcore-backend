using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Articles.Requests;
using Astrum.Articles.Repositories;
using Astrum.Identity.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Specifications;
using Microsoft.EntityFrameworkCore;
using MassTransit.Initializers;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Articles.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IArticleTagRepository _articleTagRepository;

        private readonly IMapper _mapper;

        public TagService(ITagRepository tagRepository, IMapper mapper, IArticleTagRepository articleTagRepository)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
            _articleTagRepository = articleTagRepository;
        }
        public async Task<Result<TagView>> Create(TagRequest request)
        {
            var validationResult = Validate(request.Name);
            if (validationResult.Failed)
                return Result.Error(validationResult.MessageWithErrors);

            var newTag = _mapper.Map<Tag>(request);
            var spec = new TagsByNameSpec(request.Name);
            var tag = await _tagRepository.FirstOrDefaultAsync(spec);
            if(tag == null)
            {
                await _tagRepository.AddAsync(newTag);
                try
                {
                    await _tagRepository.UnitOfWork.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Result.Error(ex.Message, "Ошибка при создании тега");
                }

                return Result.Success(_mapper.Map<TagView>(newTag));
            }
            return Result<TagView>.Error("Тег с таким именем уже существует");
        }       

        public Task<Result<List<TagView>>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<Result<List<TagView>>> GetByCategoryId(Guid id)
        {
            var spec = new TagsByCategoryIdSpec(id);
            var tags = await _tagRepository.ListAsync(spec);
            var result = _mapper.Map<List<TagView>>(tags);
            return Result.Success(result);
        }

        public async Task<Result<List<TagView>>> GetByPredicate(int count = 10, string predicate = null)
        {
            var spec = new TagsByCountAndPredicateSpec(count, predicate);
            var tags = await _tagRepository.ListAsync(spec);
            var result = _mapper.Map<List<TagView>>(tags);
            return Result.Success(result);
        }

        private static Result Validate(string name)
        {
            name = name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                return Result.Error("Недопускаются пустые названия");

            if(name.Length > 20)
                return Result.Error("Название тега слишком длинное");

            return Result.Success();
        }
    }
}
