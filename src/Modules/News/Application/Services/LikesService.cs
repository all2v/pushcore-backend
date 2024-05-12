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

public class LikesService : ILikesService
{
    private readonly ILikesRepository _likeRepository;
    private readonly IMapper _mapper;
    private readonly IMessageService _MessageService;
    private readonly IUserProfileService userProfileService;

    public LikesService(ILikesRepository likesRepository, IMapper mapper, IMessageService MessageService,
        IUserProfileService userProfileService)
    {
        _likeRepository = likesRepository;
        _mapper = mapper;
        _MessageService = MessageService;
        this.userProfileService = userProfileService;
    }

    #region ILikesService Members

    public async Task<SharedLib.Common.Results.Result<LikeForm>> GetLikeById(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetLikeByIdSpec(id);
        var like = await _likeRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (like == null)
        {
            return Result.NotFound("Лайк не найден.");
        }
        return Result.Success(_mapper.Map<LikeForm>(like));
    }

    public async Task<int> GetLikesCountByPost(Guid postId)
    {
        return await _likeRepository.CountAsync(c => c.PostId == postId);
    }

    public async Task<Guid?> GetLikeIdByPostAndUser(Guid postId, string userId)
    {
        var like = await _likeRepository.FirstOrDefaultAsync(l => l.PostId == postId
            && l.CreatedBy == userId);
        if (like == null)
            return null;
        return like.Id;
    }

    public async Task<SharedLib.Common.Results.Result<LikeForm>> CreateLike(LikeRequest LikeForm, CancellationToken cancellationToken = default)
    {
        var likeExists = await GetLikeIdByPostAndUser(LikeForm.PostId, LikeForm.From.ToString());
        if (likeExists != null)
            return Result.Error("Like already exists!");
        var like = _mapper.Map<Like>(LikeForm);
        like = await _likeRepository.AddAsync(like, cancellationToken);
        try
        {
            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании лайка.");
        }

        return Result.Success(_mapper.Map<LikeForm>(like));
    }

    public async Task<SharedLib.Common.Results.Result<LikeForm>> DeleteLike(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetLikeByIdSpec(id);
        var like = await _likeRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (like == null)
        {
            return Result.NotFound("Лайк не найден.");
        }
        try
        {
            await _likeRepository.DeleteAsync(like, cancellationToken);
            await _likeRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении лайка.");
        }
        return Result.Success(_mapper.Map<LikeForm>(like));
    }

    public async Task<SharedLib.Common.Results.Result<LikeForm>> DeleteLike(LikeDeleteRequest request,
        CancellationToken cancellationToken = default)
    {
        var like = await _likeRepository.FirstOrDefaultAsync(r => r.PostId == request.PostId &&
            r.CreatedBy == request.From.ToString());
        if (like == null)
            return Result.Error("Лайк не найден.");
        return await DeleteLike(like.Id, cancellationToken);
    }

    #endregion

    public async Task<SharedLib.Common.Results.Result<List<LikeForm>>> GetLikes(CancellationToken cancellationToken = default)
    {
        var spec = new GetLikesSpec();
        var likes = await _likeRepository.ListAsync(spec, cancellationToken);
        return Result.Success(_mapper.Map<List<LikeForm>>(likes));
    }

    public async Task SetUserToLike(LikeForm form)
    {
        var fetchedUsers = new Dictionary<Guid, UserInfo>();
        await SetUserToLike(form, fetchedUsers);
    }

    public async Task SetUserToLikesList(List<LikeForm> forms,
        Dictionary<Guid, UserInfo> fetchedUsers = null)
    {
        if (fetchedUsers == null)
            fetchedUsers = new Dictionary<Guid, UserInfo>();
        foreach (var form in forms)
            await SetUserToLike(form, fetchedUsers);
    }

    private async Task SetUserToLike(LikeForm likeForm,
        Dictionary<Guid, UserInfo> fetchedUsers)
    {
        if (fetchedUsers.ContainsKey(likeForm.From))
            likeForm.User = fetchedUsers[likeForm.From];
        else
        {
            var userProfile = await userProfileService.GetUserProfileSummaryAsync(likeForm.From);
            var userInfo = _mapper.Map<UserInfo>(userProfile.Data);
            fetchedUsers[likeForm.From] = userInfo;
            likeForm.User = userInfo;
        }
    }
}