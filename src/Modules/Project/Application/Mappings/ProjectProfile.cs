using Astrum.Infrastructure.Integrations.YouTrack.Models;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Project.Application.ViewModels.Views;
using Astrum.Project.ViewModels.DTO;
using Astrum.Projects.Aggregates;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using AutoMapper;
using AutoMapper.EquivalencyExpression;

namespace Astrum.Projects.Mappings
{
    public class ProjectProfile:Profile
    {
        public ProjectProfile()
        {
            CreateMap<ProjectRequest, Aggregates.Project>().ReverseMap();

            CreateMap<ProjectView, Aggregates.Project>().ReverseMap();
            CreateMap<ProjectView, ProjectRequest>();

            CreateMap<ProjectShortView, Aggregates.Project>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<Aggregates.Project, ProjectShortView>();
            CreateMap<Aggregates.Project, MemberShortView>()
                .ForMember(destination => destination.ProjectId, opt => opt.MapFrom(source => source.Id))
                .ForMember(destination => destination.ProjectName, opt => opt.MapFrom(source => source.Name));

            CreateMap<ProjectUpdateDto, Aggregates.Project>();

            CreateMap<CustomField, CustomFieldView>().ReverseMap();
            CreateMap<CustomFieldView, CustomFieldRequest>();

            CreateMap<CustomFieldRequest, CustomField>().ReverseMap();

            CreateMap<ProductRequest, Product>();
            CreateMap<Product, ProductRequest>();

            CreateMap<ProductView, Product>().ReverseMap();

            CreateMap<ProductUpdateDto, Product>();

            CreateMap<CustomerRequest, Customer>().ReverseMap();

            CreateMap<CustomerView, Customer>().ReverseMap();

            CreateMap<MemberRequest, Member>();
            CreateMap<Member, MemberView>().ReverseMap();
            CreateMap<Member, MemberUpdate>().ReverseMap();

            CreateMap<TrackerProject, ProjectView>();
            CreateMap<TrackerProjectMember, MemberView>();
            CreateMap<MemberView, MemberRequest>();

            CreateMap<TrackerIssue, IssueView>();
            CreateMap<TrackerIssueTag, IssueTagView>();
            CreateMap<TrackerIssueComment, IssueCommentView>();
            CreateMap<TrackerAttachment, IssueAttachmentView>();
        }
    }
}
