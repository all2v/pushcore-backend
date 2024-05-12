using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Services;
using Astrum.Infrastructure.Integrations.YouTrack.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using AutoMapper;
using Mono.Linq.Expressions;

namespace Astrum.TrackerProject.Application.Mappings
{
    public class TrackerProjectProfile : Profile
    {
        public TrackerProjectProfile(/*IUserProfileService? userProfileService*/)
        {
            CreateMap<TrackerIssue, Issue>()
                .ForMember(dest => dest.UpdaterId, opts => opts.MapFrom(src => src.Updater.RingId))
                .ForMember(dest => dest.ReporterId, opts => opts.MapFrom(src => src.Reporter.RingId))
                .ForMember(dest => dest.DraftOwnerId, opts => opts.MapFrom(src => src.DraftOwner.RingId))
                .ForMember(dest => dest.ProjectId, opts => opts.MapFrom(src => src.Project.Id))
                .ForMember(dest => dest.Updated, opts => opts.MapFrom(src => src.UpdatedDate))
                .ForMember(dest => dest.Resolved, opts => opts.MapFrom(src => src.ResolvedDate))
                .ForMember(dest => dest.AssigneeId, opts => opts.MapFrom(src => GetRingId(src, "Assignee")))
                .ForMember(dest => dest.Priority, opts => opts.MapFrom(src => GetName(src, "Priority")))
                .ForMember(dest => dest.State, opts => opts.MapFrom(src => GetName(src, "State")));
            CreateMap<TrackerIssueComment, IssueComment>()
                .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Author.RingId))
                .ForMember(dest => dest.IssueId, opts => opts.MapFrom(src => src.Issue.Id));

            CreateMap<TrackerAttachment, Attachment>();

            CreateMap<TrackerArticle, Article>()
                .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Reporter.RingId))
                .ForMember(dest => dest.ProjectId, opts => opts.MapFrom(src => src.Project.Id))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Summary))
                .ForMember(dest => dest.ChildArticles,
                    opts => opts.MapFrom(src => src.ChildArticles.Select(x => x.Id).ToList()));
            CreateMap<TrackerArticleComment, ArticleComment>()
                .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Author.RingId));

            CreateMap<Infrastructure.Integrations.YouTrack.Models.Project.TrackerProject, Domain.Aggregates.Project>()
                .ForMember(dest => dest.TeamId, opts => opts.MapFrom(src => src.Team.Id))
                .ForMember(dest => dest.LeaderId, opts => opts.MapFrom(src => src.Leader.RingId))
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name));
            CreateMap<TrackerProjectTeam, Team>().ReverseMap();

            CreateMap<Issue, IssueForm>();
            CreateMap<IssueComment, IssueCommentForm>();
            CreateMap<Attachment, AttachmentForm>();
            CreateMap<Article, ArticleForm>();
            CreateMap<ArticleComment, ArticleCommentForm>();
            CreateMap<Domain.Aggregates.Project, ProjectForm>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => (string) src.Id));

            CreateMap<ExternalUser, ExternalUserForm>();
        }

        private static string? GetRingId(TrackerIssue src, string fieldName)
        {
            return src.CustomFields.FirstOrDefault(x=>x.Name.Equals(fieldName))?.Value?.FirstOrDefault()?.RingId;
        }

        private static string? GetName(TrackerIssue src, string fieldName)
        {
            return src.CustomFields.FirstOrDefault(x => x.Name.Equals(fieldName))?.Value?.FirstOrDefault()?.LocalizedName;
        }
    }
}
