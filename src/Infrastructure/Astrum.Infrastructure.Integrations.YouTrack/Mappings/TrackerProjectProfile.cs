using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Integrations.YouTrack.Models.TrackerDTO.IssueDTO;
using AutoMapper;
using NetBox.Extensions;
using YouTrackSharp.Generated;
using Issue = YouTrackSharp.Issues.Issue;

namespace Astrum.Infrastructure.Integrations.YouTrack.Mappings
{
    public class TrackerProjectProfile : Profile
    {
        public TrackerProjectProfile()
        {
            //CreateMap<Project, TrackerProject>()
            //    .ForMember(dest => dest.Leader, opts => opts.MapFrom(src => src.Leader.RingId))
            //    .ForMember(dest => dest.TeamId, opts => opts.MapFrom(src => src.Team.Id))
            //    .ForMember(dest => dest.TeamRingId, opts => opts.MapFrom(src => src.Team.RingId));

            //CreateMap<IssueComment, TrackerIssueComment>()
            //    .ForMember(dest => dest.AuthorId, opts => opts.MapFrom(src => src.Author.RingId));

            CreateMap<IssueTag, TrackerIssueTag>();

            //CreateMap<YouTrackSharp.Generated.Issue, TrackerIssue>()
            //    .ForMember(dest => dest.DraftOwnerId, opts => opts.MapFrom(src => src.DraftOwner.RingId))
            //    .ForMember(dest => dest.ProjectId, opts => opts.MapFrom(src => src.Project.Id))
            //    .ForMember(dest => dest.ReporterId, opts => opts.MapFrom(src => src.Reporter.RingId))
            //    .ForMember(dest => dest.UpdaterId, opts => opts.MapFrom(src => src.Updater.RingId))
            //    .ForMember(dest => dest.WatcherIds,
            //        opts => opts.MapFrom(src => src.Watchers.DuplicateWatchers.Select(x => x.Id)))
            //    .ForMember(dest => dest.Subtasks, opts => opts.MapFrom(src => src.Subtasks.Issues.Select(x => x.Id)))
            //    .ForMember(dest => dest.Parent, opts => opts.MapFrom(src => src.Parent.Issues.First().Id));

            CreateMap<YouTrackSharp.Management.User, TrackerProjectMember>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.RingId));

            CreateMap<IssueEvent, TrackerIssue>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Issue.Id))
                .ForMember(dest => dest.Summary, opts => opts.MapFrom(src => src.Issue.Name))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Issue.Description))
                .ForMember(dest => dest.Url, opts => opts.MapFrom(src => src.Issue.Url));
        }
    }
}

