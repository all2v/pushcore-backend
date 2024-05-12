using Astrum.News.Aggregates;
using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.DomainServices.ViewModels.Requests;
using Astrum.News.DomainServices.ViewModels.Responces;
using Astrum.News.ViewModels;
using AutoMapper;
using Mono.Linq.Expressions;

namespace Astrum.News.Mappings;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<Post, PostForm>().ForMember(dest => dest.From, opts => opts.MapFrom(src => ParseGuid(src.CreatedBy)));
        CreateMap<PostForm, Post>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));
        CreateMap<PostForm, PostResponse>();
        CreateMap<UserInfo, UserResponse>();

        CreateMap<Post, PostRequest>().ForMember(dest => dest.From, opts => opts.MapFrom(src => ParseGuid(src.CreatedBy)));
        CreateMap<PostRequest, Post>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));

        CreateMap<Like, LikeForm>().ForMember(dest => dest.From, opts => opts.MapFrom(src => ParseGuid(src.CreatedBy)));
        CreateMap<LikeForm, Like>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));
        CreateMap<LikeRequest, Like>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));

        CreateMap<Comment, CommentForm>().ForMember(dest => dest.From, opts => opts.MapFrom(src => ParseGuid(src.CreatedBy)));
        CreateMap<CommentForm, Comment>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));
        CreateMap<CommentRequest, Comment>().ForMember(dest => dest.CreatedBy, opts => opts.MapFrom(src => src.From.ToString()));

        CreateMap<Banner, BannerForm>().ReverseMap();

        CreateMap<Widget, WidgetForm>().ReverseMap();
    }

    private static Guid ParseGuid(string guid)
    {
        return Guid.TryParse(guid, out var result) ? result : Guid.Empty;
    }
}