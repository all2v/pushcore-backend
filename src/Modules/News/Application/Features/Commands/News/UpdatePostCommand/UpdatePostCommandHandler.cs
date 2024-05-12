using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Contracts;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using AutoMapper;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.News.Application.Features.Commands.News.UpdatePostCommand
{
    public class UpdatePostCommandHandler : CommandResultHandler<UpdatePostCommand, PostForm>
    {
        private readonly INewsRepository _newsRepository;
        private readonly IAuthenticatedUserService _user;
        private readonly IMapper _mapper;

        public UpdatePostCommandHandler(INewsRepository newsRepository, IAuthenticatedUserService user,
            IMapper mapper) 
        {
            _newsRepository = newsRepository;
            _user = user;
            _mapper = mapper;
        }

        public override async Task<SharedLib.Common.Results.Result<PostForm>> Handle(UpdatePostCommand command, CancellationToken cancellationToken = default)
        {
            var spec = new GetPostByIdSpec(command.PostId);
            var post = await _newsRepository.FirstOrDefaultAsync(spec);

            if (post == null)
                return Result.NotFound("Новость не найдена.");

            var postHasAuthor = Guid.TryParse(post.CreatedBy, out var authorId);
            if (postHasAuthor && authorId != _user.UserId)
                return Result.Forbidden();

            post.Title = command.Post.Title ?? post.Title;
            post.Text = command.Post.Text ?? post.Text;
            // TODO: прикрепление файлов при редактировании?

            await _newsRepository.UpdateAsync(post, cancellationToken);
            await _newsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            var response = _mapper.Map<PostForm>(post);
            return Result.Success(response);
        }
    }
}
