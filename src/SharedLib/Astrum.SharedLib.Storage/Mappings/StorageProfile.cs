using Astrum.News.Aggregates;
using Astrum.News.ViewModels;
using AutoMapper;

namespace Astrum.News.Mappings;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        CreateMap<Post, PostForm>();
        CreateMap<PostForm, Post>();

        CreateMap<Like, LikeForm>();
        CreateMap<LikeForm, Like>();

        CreateMap<Comment, CommentForm>();
        CreateMap<CommentForm, Comment>();

        CreateMap<Banner, BannerForm>();
        CreateMap<BannerForm, Banner>();

        CreateMap<Widget, WidgetForm>();
        CreateMap<WidgetForm, Widget>();
    }
}