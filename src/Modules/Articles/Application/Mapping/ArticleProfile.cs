using Astrum.Articles.Aggregates;
using Astrum.Articles.Application.Models.Requests;
using Astrum.Articles.Requests;
using Astrum.Articles.ViewModels;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using AutoMapper;

namespace Astrum.Articles.Mapping
{
    public class ArticleProfile : Profile
    {
        public ArticleProfile()
        {
            CreateMap<Guid, Author>().ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src));
            CreateMap<ArticleCreateRequest, Article>();
            CreateMap<ArticleEditRequest, Article>();
                //.ForMember(x => x.CoverUrl,
                //    x => x.MapFrom(x => Guid.NewGuid() + Path.GetExtension(x.CoverImage.FileName)));
            CreateMap<Article, ArticleSummary>()
                .ForMember(dest => dest.Slug, opts => opts.MapFrom(src => src.Slug.FullSlug));

            CreateMap<Article, ArticleView>()
                .ForMember(dest =>dest.Content, opts => opts.MapFrom(src=>src.Content.Html))
                .ForMember(dest=> dest.Slug, opts => opts.MapFrom(src=> src.Slug.FullSlug));
            CreateMap<Category, CategoryView>();

            CreateMap<Tag, TagView>();
            CreateMap<Tag, ArticleCountByTag>();
            CreateMap<TagRequest, Tag>();
            CreateMap<ArticleContent, ArticleContentDto>().ReverseMap();

            CreateMap<TrackerArticle, Article>()
                .ForMember(dest => dest.TrackerArticleId, opts => opts.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Summary))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Content));
        }
    }
}