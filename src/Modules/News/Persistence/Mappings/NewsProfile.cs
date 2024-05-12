using Astrum.Account.Features.Profile;
using Astrum.News.Aggregates;
using Astrum.News.Entities;
using Astrum.News.ViewModels;
using AutoMapper;

namespace Astrum.News.Mappings;

public class NewsProfile : Profile
{
    public NewsProfile()
    {
        // CreateMap<AppealEntity, AppealAggregate>().ConstructUsing(e=> new AppealAggregate()
        // {
        //     Created = e.Created,
        //     AppealStatus = e.AppealStatus,
        //     From = e.From,
        //     Title = e.Title,
        //     Request = e.Request
        // });  
        CreateMap<PostEntity, Post>();
        CreateMap<Post, PostEntity>();

        CreateMap<LikeEntity, Like>();
        CreateMap<Like, LikeEntity>();

        CreateMap<CommentEntity, Comment>();
        CreateMap<Comment, CommentEntity>();

        CreateMap<BannerEntity, Banner>();
        CreateMap<Banner, BannerEntity>();
    }
}