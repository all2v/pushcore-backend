using Astrum.Module.Project.Application.ViewModels;
using Astrum.Module.Project.Application.ViewModels.DTO;
using Astrum.Projects.ViewModels;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Projects.Services
{
    public interface IProjectService
    {
        Task<Result<ProjectView>> Create(ProjectRequest request);
        Task<Result<List<ProjectView>>> GetAll();
        public Task<Result<ProjectView>> Delete(Guid id);
        public Task<Result<ProjectView>> Get(Guid id);
        public Task<Result<ProjectView>> GetWithProductName(Guid id);
        public Task<Result<List<ProjectView>>> GetByMember(Guid memberId);
        public Task<Result<List<MemberShortView>>> GetMemberShortInfo(Guid memberId);
        public Task<Result<ProjectView>> Update(Guid id, ProjectUpdateDto project);
        public Task<Result<ProjectView>> AddMembers(AddMembersDto changeMembersDto);
        public Task<Result<List<MemberView>>> RemoveMembers(RemoveMembersDto changeMembersDto);
    }
}
