using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Services;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Repositories;
using Astrum.Articles.Services;
using Astrum.Articles.Specifications;
using Astrum.Articles.ViewModels;
using Astrum.Module.Articles.Application.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;

namespace Astrum.Articles.Application.Features.Queries.GetArticleBySlug
{
    public class GetArticleBySlugQueryHandler : QueryResultHandler<GetArticleBySlugQuery, ArticleView>
    {
        private readonly IMapper _mapper;
        private readonly IFileStorage _fileStorage;
        private readonly IArticleRepository _articleRepository;
        private readonly IUserProfileService _userProfileService;

        public GetArticleBySlugQueryHandler(IMapper mapper, IFileStorage fileStorage, IArticleRepository articleRepository,
            IUserProfileService profileService)
        {
            _mapper = mapper;
            _fileStorage = fileStorage;
            _articleRepository = articleRepository;
            _userProfileService = profileService;
        }

        public override async Task<Result<ArticleView>> Handle(GetArticleBySlugQuery query, CancellationToken cancellationToken = default)
        {
            var slug = new ArticleSlug(query.Username, query.ArticleName);
            var spec = new GetArticleBySlugSpec(slug);

            var article = await _articleRepository.FirstOrDefaultAsync(spec, cancellationToken);
            if (article == null)
                return Result.NotFound($"Статья пользователя {query.Username} под названием {query.ArticleName} не найдена.");
            var response = _mapper.Map<ArticleView>(article);

            if (article.CoverImageId.HasValue)
                response.CoverUrl = await _fileStorage.GetFileUrl(article.CoverImageId.Value);

            var authorResult = await _userProfileService.GetUserProfileSummaryAsync(article.Author.UserId);
            if (authorResult.Failed)
                return Result.Error(authorResult.MessageWithErrors);

            response.Author = _mapper.Map<AuthorView>(authorResult.Data);
            return Result.Success(response);
        }
    }
}
