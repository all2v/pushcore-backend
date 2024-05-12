using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Repositories;
using Astrum.Articles.Requests;
using Astrum.Articles.Services;
using Astrum.Articles.Specifications;
using Astrum.Articles.ViewModels;
using Astrum.Identity.Contracts;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Articles.Application.Features.Commands.Articles.UpdateArticleCommand
{
    public class UpdateArticleCommandHandler : CommandResultHandler<UpdateArticleCommand, ArticleView>
    {
        private readonly IArticleRepository _articleRepository;
        private readonly IArticleService _articleService;
        private readonly IAuthenticatedUserService _user;
        private readonly IMapper _mapper;

        public UpdateArticleCommandHandler(IArticleRepository articleRepository,
            IAuthenticatedUserService user, IMapper mapper, IArticleService articleService)
        {
            _articleService = articleService;
            _articleRepository = articleRepository;
            _user = user;
            _mapper = mapper;
        }

        public override async Task<Result<ArticleView>> Handle(UpdateArticleCommand command, CancellationToken cancellationToken = default)
        {
            var spec = new GetArticleByIdSpec(command.ArticleId);
            var article = await _articleRepository.FirstOrDefaultAsync(spec);
            if (article == null)
            {
                return Result.NotFound("Статья не найдена.");
            }
            if (article.Author.UserId != _user.UserId)
            {
                return Result.Forbidden();
            }

            var response = await _articleService.Update(article.Id, command.Article, cancellationToken);
            return response;
        }
    }
}
