using Astrum.Account.Services;
using Astrum.News.Aggregates;
using Astrum.News.DomainServices.ViewModels.Requests;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Telegram.Services;
using AutoMapper;

namespace Astrum.News.Services;

public class CommentsService : ICommentsService
{
    private readonly ICommentsRepository _commentRepository;
    private readonly IMapper _mapper;
    private readonly IMessageService _MessageService;
    private readonly IUserProfileService userProfileService;

    public CommentsService(ICommentsRepository commentsRepository, IMapper mapper,
        IMessageService MessageService, IUserProfileService userProfileService)
    {
        _commentRepository = commentsRepository;
        _mapper = mapper;
        _MessageService = MessageService;
        this.userProfileService = userProfileService;
    }

    #region ICommentsService Members

    public async Task<SharedLib.Common.Results.Result<CommentForm>> GetCommentById(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetCommentByIdSpec(id);
        var comment = await _commentRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (comment == null)
        {
            return Result.NotFound("Комментарий не найден.");
        }

        var response = _mapper.Map<CommentForm>(comment);
        await SetUserToComment(response);
        return Result.Success(response);
    }

    public async Task<int> GetCommentsCountByPost(Guid postId)
    {
        return await _commentRepository.CountAsync(c => c.PostId == postId 
            && c.ReplyCommentId == null);
    }

    public async Task<SharedLib.Common.Results.Result<CommentForm>> UpdateComment(Guid id, CommentForm CommentForm,
        CancellationToken cancellationToken = default)
    {
        var comment = _mapper.Map<Comment>(CommentForm);
        var spec = new GetCommentByIdSpec(id);
        var dbComment = await _commentRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (dbComment == null)
        {
            return Result.NotFound("Комментарий не найден.");
        }
        dbComment.Text = comment.Text;
        //dbComment.Version++;
        try
        {
            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении комментария.");
        }

        var response = _mapper.Map<CommentForm>(dbComment);
        await SetUserToComment(response);
        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<CommentForm>> CreateComment(CommentRequest CommentForm, CancellationToken cancellationToken = default)
    {
        var comment = _mapper.Map<Comment>(CommentForm);

        comment = await _commentRepository.AddAsync(comment, cancellationToken);
        try
        {
            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании комментария.");
        }

        var response = _mapper.Map<CommentForm>(comment);
        await SetUserToComment(response);
        return Result.Success(response);
    }

    public async Task<SharedLib.Common.Results.Result<CommentForm>> DeleteComment(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetCommentByIdSpec(id);
        var comment = await _commentRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (comment == null)
        {
            return Result.NotFound("Комментарий не найден.");
        }
        try
        {
            //TODO: rework on OnDeleted
            await _commentRepository.DeleteAsync(comment, cancellationToken);
            await _commentRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении комментария.");
        }

        var response = _mapper.Map<CommentForm>(comment);
        await SetUserToComment(response);
        return Result.Success(response);
    }

    public async Task SetUserToComment(CommentForm form)
    {
        var fetchedUsers = new Dictionary<Guid, UserInfo>();
        await SetUsersToCommentRecursively(form, fetchedUsers);
    }

    public async Task SetUserToCommentList(List<CommentForm> forms, 
        Dictionary<Guid, UserInfo> fetchedUsers = null)
    {
        if (fetchedUsers == null)
            fetchedUsers = new Dictionary<Guid, UserInfo>();
        foreach (var form in forms)
            await SetUsersToCommentRecursively(form, fetchedUsers);
    }

    private async Task SetUsersToCommentRecursively(CommentForm currentComment,
        Dictionary<Guid, UserInfo> fetchedUsers)
    {
        if (fetchedUsers.ContainsKey(currentComment.From))
            currentComment.User = fetchedUsers[currentComment.From];
        else
        {
            var userProfile = await userProfileService.GetUserProfileSummaryAsync(currentComment.From);
            var userInfo = _mapper.Map<UserInfo>(userProfile.Data);
            fetchedUsers[currentComment.From] = userInfo;
            currentComment.User = userInfo;
        }
        if(currentComment.ChildComments != null && currentComment.ChildComments.Any())
        {
            foreach(var comment in currentComment.ChildComments)
                await SetUsersToCommentRecursively(comment, fetchedUsers);
        }
    }

    #endregion
}