using Astrum.Infrastructure.Integrations.YouTrack.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.ArticleDTO;
using AutoMapper;

namespace Astrum.Infrastructure.Integrations.YouTrack.Mappings
{
    public class TrackerArticleProfile : Profile
    {
        public TrackerArticleProfile()
        {
            //CreateMap<YouTrackSharp.Generated.Article, TrackerArticle>()
            //    .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Reporter.RingId))
            //    .ForMember(dest => dest.ChildArticlesId,
            //        opts => opts.MapFrom(src => src.ChildArticles.Select(x => x.Id)))
            //    .ForMember(dest => dest.ParentArticleId, opts => opts.MapFrom(src => src.ParentArticle.Id))
            //    .ForMember(dest => dest.ProjectId, opts => opts.MapFrom(src => src.Project.Id))
            //    .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Summary));

            CreateMap<YouTrackSharp.Generated.ArticleAttachment, TrackerAttachment>();
            //CreateMap<YouTrackSharp.Generated.ArticleComment, TrackerArticleComment>()
            //    .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Author.RingId));

            CreateMap<ArticleEvent, TrackerArticle>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Article.Id))
                .ForMember(dest => dest.Summary, opts => opts.MapFrom(src => src.Article.Summary))
                .ForMember(dest => dest.Content, opts => opts.MapFrom(src => src.Article.Content));
        }
    }
}

