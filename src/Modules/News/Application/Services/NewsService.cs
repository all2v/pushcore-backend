using System.Threading;
using Astrum.Account.Services;
using Astrum.News.Aggregates;
using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.DomainServices.ViewModels;
using Astrum.News.Repositories;
using Astrum.News.Specifications;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Application.Helpers;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Repositories;
using Astrum.Storage.Services;
using Astrum.Telegram.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace Astrum.News.Services;

public class NewsService : INewsService
{
    private readonly IMapper _mapper;
    private readonly IMessageService _MessageService;
    private readonly INewsRepository _newsRepository;
    private readonly IAttachmentsRepository _attachmentsRepository;
    private readonly IBannersRepository _bannersRepository;
    private readonly IBannersService _bannersService;
    private readonly IFileStorage _fileStorage;
    private readonly IWidgetService _widgetService;
    private readonly IUserProfileService _userProfileService;
    private readonly IFileRepository _fileRepository;
    private readonly ICommentsService commentsService;
    private readonly ILikesService likesService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public NewsService(INewsRepository newsRepository,IBannersRepository bannersRepository, IMapper mapper, IAttachmentsRepository attachmentsRepository,
        IMessageService MessageService, IBannersService bannersService, IFileStorage fileStorage, IWidgetService widgetService,
        IUserProfileService userProfileService, IFileRepository fileRepository, ICommentsService commentsService,
        ILikesService likesService, IHttpContextAccessor httpContextAccessor)
    {
        _newsRepository = newsRepository;
        _mapper = mapper;
        _MessageService = MessageService;
        _bannersRepository = bannersRepository;
        _bannersService = bannersService;
        _fileStorage = fileStorage;
        _widgetService = widgetService;
        _attachmentsRepository = attachmentsRepository;
        _userProfileService = userProfileService;
        _fileRepository = fileRepository;
        this.commentsService = commentsService;
        this.likesService = likesService;
        this.httpContextAccessor = httpContextAccessor;
    }

    #region INewsService Members

    public async Task<SharedLib.Common.Results.Result<NewsForm>> GetNews(CancellationToken cancellationToken = default)
    {
        var spec = new GetPostsSpec();
        var posts = await _newsRepository.IncludedListBySpecAsync(spec, cancellationToken);
        var postForms = new List<PostForm>();
        foreach (var post in posts)
        {
            var postForm = await MapPostToForm(post);
            var result = await _userProfileService.GetUserProfileSummaryAsync(postForm.From);
            postForm.User = _mapper.Map<UserInfo>(result.Data);
            postForms.Add(postForm);
        }
        await AddUsersToCommentAndLikes(postForms);
        var banners = await _bannersService.GetActiveBanners(cancellationToken);
        var widgets = await _widgetService.GetActiveWidgets(cancellationToken);
        return Result.Success(new NewsForm
        {
            Posts = postForms,
            Banners = banners,
            Widgets = widgets
        });
    }

    public async Task<SharedLib.Common.Results.Result<PostForm>> GetPostById(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetPostByIdSpec(id);
        var post = await _newsRepository.FirstOrDefaultBySpecAsync(spec, cancellationToken);
        if (post == null)
        {
            return Result.NotFound("Пост не найден.");
        }
        var postForm = await MapPostToForm(post);
        var result = await _userProfileService.GetUserProfileSummaryAsync(postForm.From);
        postForm.User = _mapper.Map<UserInfo>(result.Data);
        await AddUsersToCommentAndLikes(postForm);
        return Result.Success(postForm);
    }

    public async Task<SharedLib.Common.Results.Result<List<PostForm>>> GetPostsByUser(Guid userId, CancellationToken cancellationToken = default)
    {
        var spec = new GetPostsByUserIdSpec(userId);
        var posts = await _newsRepository.IncludedListBySpecAsync(spec, cancellationToken); ;
        var postForms = new List<PostForm>();
        var res = await _userProfileService.GetUserProfileSummaryAsync(userId);
        var user = _mapper.Map<UserInfo>(res.Data);
        foreach (var post in posts)
        {
            var postForm = await MapPostToForm(post);
            postForm.User = user;
            postForms.Add(postForm);
        }
        await AddUsersToCommentAndLikes(postForms);
        return Result.Success(postForms);
    }

    public async Task<SharedLib.Common.Results.Result<List<PostForm>>> GetPostsByUser(Guid userId, int? startIndex, int? count)
    {
        var posts = await GetListPostsByUser(userId, startIndex, count);
        return Result.Success(posts);
    }

    public async Task<SharedLib.Common.Results.Result<PostPaginationView>> GetPostsByUserPagination(Guid userId, int startIndex, int count)
    {
        // возвращает true даже если элементов нет
        // var nextExists = await _newsRepository.AnyAsync(new GetPostsByUserIdPagination(userId, startIndex + count, 1));
        var posts = await GetListPostsByUser(userId, startIndex, count);
        var nextExists = (await _newsRepository.ListAsync(new GetPostsByUserIdPagination(userId, startIndex + count, 1))).Count > 0;
        var result = new PostPaginationView()
        {
            Posts = posts,
            NextExists = nextExists,
            LastIndex = posts.Count == count ? startIndex - 1 + count : (posts.Count == 0 ? 0 : startIndex - 1 + posts.Count)
        };
        return Result.Success(result);
    }

    public async Task<SharedLib.Common.Results.Result<PostForm>> UpdatePost(Guid id, PostForm postForm, CancellationToken cancellationToken = default)
    {
        var post = _mapper.Map<Post>(postForm);
        var spec = new GetPostByIdSpec(id);
        var dbPost = await _newsRepository.FirstOrDefaultBySpecAsync(spec, cancellationToken);
        if (dbPost == null)
        {
            return Result.NotFound("Пост не найден.");
        }
        dbPost.Title = post.Title;
        dbPost.Text = post.Text;
        //dbPost.Version++;
        try
        {
            await _newsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении поста.");
        }
        return Result.Success(_mapper.Map<PostForm>(dbPost));
    }

    public async Task<SharedLib.Common.Results.Result<PostForm>> CreatePost(PostRequest postForm, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(postForm.Text) && (postForm.Attachments == null || postForm.Attachments.Count == 0))
            return Result.Error("В посте нет текста и вложений");
        var post = _mapper.Map<Post>(postForm);
        var chatId = -595493502;

        post = await _newsRepository.AddAsync(post, cancellationToken);
        await _newsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

        var attachments = new List<PostFileAttachment>();
        if (postForm.Attachments != null && postForm.Attachments.Any())
        {

            foreach (var attachmentForm in postForm.Attachments)
            {
                var res = await _fileStorage.UploadFile(attachmentForm);
                if (res != null && res.Success)
                {
                    var attachment = new PostFileAttachment();
                    attachment.PostId = post.Id;
                    attachment.FileId = res.UploadedFileId.Value;
                    attachments.Add(attachment);
                }
            }

            await _attachmentsRepository.AddRangeAsync(attachments, cancellationToken);
            try
            {
                await _attachmentsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);

            }
            catch (Exception ex)
            {
                return Result.Error("Ошибка при создании поста.");
            }
        }

        await SendMessageToTelegramAsync(chatId, post, postForm.Attachments);

        var result = await MapPostToForm(post);
        var resUsers = await _userProfileService.GetUserProfileSummaryAsync(result.From);
        result.User = _mapper.Map<UserInfo>(resUsers.Data);
        await AddUsersToCommentAndLikes(result);
        return Result.Success(result);
    }

    private async Task<List<PostForm>> GetListPostsByUser(Guid userId, int? startIndex, int? count)
    {
        if (startIndex < 0)
            throw new ArgumentException(null, nameof(startIndex));
        var postForms = new List<PostForm>();
        if (count <= 0)
            return postForms;
        var spec = new GetPostsByUserIdPagination(userId, startIndex, count);
        var posts = await _newsRepository.IncludedListBySpecAsync(spec);
        var res = await _userProfileService.GetUserProfileSummaryAsync(userId);
        var user = _mapper.Map<UserInfo>(res.Data);
        foreach (var post in posts)
        {
            var postForm = await MapPostToForm(post);
            postForm.User = user;
            postForms.Add(postForm);
        }
        await AddUsersToCommentAndLikes(postForms);
        return postForms;
    }

    private async Task SendMessageToTelegramAsync(long chatId, Post post, List<IFormFile> attachments)
    {
        var txt = $"<strong>{post.Title}</strong>\n\n" +
                  $"{post.Text}";
        IFormFile image;

        if (attachments != null && (image = attachments.FirstOrDefault(file => file.ContentType.StartsWith("image"))) != null)
        {
            using (var ms = new MemoryStream())
            {
                await image.CopyToAsync(ms);
                ms.Seek(0, SeekOrigin.Begin);
                await _MessageService.SendPhoto(chatId, ms, image.FileName, txt);
            }
        }
        else
        {
            await _MessageService.SendMessage(chatId, txt);
        }
    }

    public async Task<SharedLib.Common.Results.Result<PostForm>> DeletePost(Guid id, CancellationToken cancellationToken = default)
    {
        var spec = new GetPostByIdSpec(id);
        var post = await _newsRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (post == null)
        {
            return Result.NotFound("Пост не найден.");
        }
        try
        {
            await _newsRepository.DeleteAsync(post, cancellationToken);
            await _newsRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении поста");
        }
        return Result.Success(_mapper.Map<PostForm>(post));
    }

    private async Task AddUsersToCommentAndLikes(List<PostForm> posts)
    {
        var fetchedUsers = new Dictionary<Guid, UserInfo>();
        foreach (var post in posts)
        {
            await commentsService.SetUserToCommentList(post.Comments, fetchedUsers);
            await likesService.SetUserToLikesList(post.Likes, fetchedUsers);
        }
    }

    private async Task AddUsersToCommentAndLikes(PostForm post)
    {
        var fetchedUsers = new Dictionary<Guid, UserInfo>();
        await commentsService.SetUserToCommentList(post.Comments, fetchedUsers);
        await likesService.SetUserToLikesList(post.Likes, fetchedUsers);
    }

    private async Task<PostForm> MapPostToForm(Post post)
    {
        //TODO: очень не оптимизированно. Нужно брать кол-во лайков, лайкнул ли юзер
        //и подгружать только несколько последних лайкнувших. Возможно стоит использовать
        //чистый SQL, чтобы попробовать сделать это одним запросом
        var postForm = _mapper.Map<PostForm>(post);
        await AddAttachmentsToForm(post, postForm);
        await AddLikesInfo(post, postForm);
        return postForm;
    }

    //Works only from api
    private async Task AddLikesInfo(Post post, PostForm postForm)
    {
        postForm.LikesCount = await likesService.GetLikesCountByPost(post.Id); 
        var userId = JwtManager.GetUserIdFromRequest(httpContextAccessor.HttpContext.Request);
        if (userId != null)
            postForm.LikeId = await likesService.GetLikeIdByPostAndUser(post.Id, userId.Value.ToString());
    }

    private async Task AddAttachmentsToForm(Post post, PostForm postForm)
    {
        foreach (var attachmentForm in post.FileAttachments)
        {
            var file = await _fileRepository.GetByIdAsync(attachmentForm.FileId);
            var attachment = new AttachmentFileForm { Name = file.FileName, Type = file.ContentType };
            attachment.Url = await _fileStorage.GetFileUrl(attachmentForm.FileId);
            postForm.Attachments.Add(attachment);
        }
    }

    #endregion
}